using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Microsoft.AspNet.SignalR;

namespace OnlineConsultant
{
    using Services;
    using Models;

    [Authorize]
    public class ChatHub : Hub
    {
        private static ConcurrentDictionary<string, UserDetail> ConnectedUsers = new ConcurrentDictionary<string, UserDetail>();

        private static ListQueue<UserDetail> QueueUsers = new ListQueue<UserDetail>();

        private static ConsultantList ConsultantUser = new ConsultantList();

        private readonly IQuestionService _questionService;

        public ChatHub(IQuestionService service)
        {
            _questionService = service;
        }


        public void Connect(string Id)
        {
            AddConnect(new UserDetail
            {
                Id = Context.User.GetUserId(),
                Name = Context.User.Identity.Name,
                IsSpec = Context.User.IsSpec(),
                ConnectionId = Id
            });
            
            //Обновляем список пользователей, только для специалистов
            RefreshUserOnlySpecialist();
        }

        private void AddConnect(UserDetail detail)
        {
            if (!detail.IsSpec)
                QueueUsers.Enqueue(detail);
            else
                ConsultantUser.AddFree(detail);

            ConnectedUsers.AddOrUpdate(detail.ConnectionId, detail, (key, val) => detail);
        }
        
        
        public void OnProcessed()
        {
            var hub = Clients;
            var currentConnectionId = Context.ConnectionId;
            if (!ConnectedUsers.ContainsKey(currentConnectionId) && !Context.User.IsSpec()) return;

            var consultant = ConsultantUser.FindById(currentConnectionId);
            if(consultant != null && consultant.HandlingUser != null)
            {
                var client = consultant.HandlingUser;

                hub.Client(client.ConnectionId).onClientResolve();
                hub.Client(consultant.User.ConnectionId).onConsultantResolve();

                _questionService.CloseQuestion(consultant.HandlingUser.Id);
                ConsultantUser.SetFree(consultant.HandlingUser);

                ConnectedUsers.TryRemove(client.ConnectionId, out client);
            }
        }

        public void Disconnected(bool stateReload)
        {
            var id = Context.ConnectionId;
            if (!ConnectedUsers.ContainsKey(id)) return;

            var currentUser = ConnectedUsers[id];

            //Освобождаем консультанта
            if (currentUser.IsSpec)
            {
                var consultant = ConsultantUser.FindById(currentUser.ConnectionId);
                ConsultantUser.Remove(currentUser.ConnectionId);

                if (consultant.HandlingUser != null)
                {
                    QueueUsers.Enqueue(consultant.HandlingUser);
                    //Уведомляем пользователя о разрыве связи
                    Clients.Client(consultant.HandlingUser.ConnectionId).onConsultanDisconect();
                }

                var user = consultant.User;
            }
            //Освобождаем пользователя
            else if(!currentUser.IsSpec)
            {
                var consultant = ConsultantUser.GetAll().FirstOrDefault(x => x.HandlingUser != null && x.HandlingUser.ConnectionId == id);
                if(consultant != null)
                {
                    ConsultantUser.SetFree(currentUser);
                    Clients.Client(consultant.User.ConnectionId).onClentDisconect();
                }

               
                QueueUsers.Remove(currentUser);
            }

            ConnectedUsers.TryRemove(id, out currentUser);
            RefreshUserOnlySpecialist();
        }

     
        public override Task OnDisconnected(bool stopCalled)
        {
            if(stopCalled)
                Disconnected(true);

            return base.OnDisconnected(stopCalled);
        }

        //Получаем пользователя из очереди
        public bool ConnectionUserFromQueue()
        {
            var hub = Clients;
            var userDetail = new UserDetail();

            //Если очередь пустая
            if (QueueUsers.IsEmpty())
            {
                hub.Client(Context.ConnectionId).onHasEmptyQueue();
                return false;
            }

            QueueUsers.Dequeue(out userDetail);
            var question = _questionService.FindQuestion(userDetail.Id);

            hub.Client(Context.ConnectionId).onSendUser(new { Id = userDetail.ConnectionId, Name = userDetail.Name });
            hub.Client(userDetail.ConnectionId).onSendUser(new
            {
                Id = Context.ConnectionId,
                Name = ConnectedUsers[Context.ConnectionId].Name
            });

            //Занимаем консультанта
            ConsultantUser.SetWork(Context.ConnectionId, userDetail);

            hub.Client(userDetail.ConnectionId).onConsultantConnecting();

            SendMessage(userDetail.ConnectionId, Context.ConnectionId, question.Description);
            RefreshUserOnlySpecialist();

            return true;
        }

        public void SendMessageToChat(string Id, string message)
        {
            var hub = Clients;

            SendMessage(Context.ConnectionId, Id, message);
        }

        private void SendMessage(string fromUser, string toUser, string message)
        {
            var hub = Clients;


            hub.Client(fromUser).GetMessage(new { From = fromUser, To = toUser, Message = message, IsSelf = true });
            hub.Client(toUser).GetMessage(new { From = fromUser, To = toUser, Message = message, IsSelf = false });
        }


        private void RefreshUserOnlySpecialist()
        {
            //Получаем подключения для специалистов
            var allSpecialist = ConsultantUser.GetAll()
                                            .Select(x => x.User.ConnectionId)
                                            .ToArray();
           

            //Получаем всех пользователей, кроме своего пользователя
            var allUsers = QueueUsers.Select(x => new
            {
                ConnectId = x.ConnectionId,
                Name = x.Name,
                Type = "Client"
            })
            .ToArray();

            var listUserIDs = ConnectedUsers.Where(x => !x.Value.IsSpec).Select(x => x.Key).ToArray(); 


            var allConsultan = ConsultantUser.GetAll()
                                             .Select(x => new {
                                                 ConnectId = x.User.ConnectionId,
                                                 Name = x.User.Name,
                                                 IsFree = x.IsFree,
                                                 Type = "Consultant"
                                             })
                                             .ToArray();

            Clients.Clients(ConsultantUser.GetConnectionIDs())
                    .onNewUserConnected(allUsers);
            Clients.Clients(listUserIDs)
                    .onNewUserConnected(allConsultan);
        }
    }
}
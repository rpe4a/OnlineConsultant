using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineConsultant
{
    using Models;

    public class ConsultantList
    {
        private readonly List<Consultant> _list = new List<Consultant>();

        private object _sync = new object();

        public void AddFree(UserDetail cons)
        {
            lock(_sync)
            {
                _list.Add(new Consultant
                {
                    User = cons,
                    IsFree = true
                });
            }
        }

        public Consultant FindById(string connectionId)
        {
            lock(_sync)
            {
                var result = _list.SingleOrDefault(x => x.User.ConnectionId == connectionId);
                return result;
            }
        }

        public string [] GetConnectionIDs()
        {
            lock(_sync)
            {
                return _list.Select(x => x.User.ConnectionId).ToArray();
            }
        }

        public void SetWork(string connectionId, UserDetail user)
        {
            lock(_sync)
            {
                var consultant = FindById(connectionId);
                if (!consultant.IsFree) throw new Exception("Консультант уже занят");

                consultant.HandlingUser = user;
                consultant.IsFree = false;
            }
        }

        public void SetFree(UserDetail user)
        {
            lock(_sync)
            {
                var consultant = _list.SingleOrDefault(x => x.HandlingUser != null &&
                                                            x.HandlingUser.ConnectionId == user.ConnectionId);
                if(consultant != null)
                {
                    consultant.IsFree = true;
                    consultant.HandlingUser = null;
                }
            }
        }

        public void Remove(string connectionId)
        {
            lock(_sync)
            {
                var consultant = FindById(connectionId);
                _list.Remove(consultant);
            }
        }

        public IEnumerable<Consultant> GetAll() { return _list.ToList(); }
    }

}
import {
    Type
} from '../Utils/index';

export default function (chat, chatController) {
    chat.on('GetMessage', function (MessagePackege) {
        chatController.UpdateChatBody(MessagePackege)
    });

    chat.on('onNewUserConnected', function (users) {
        chatController.UpdateOnlineUsers(users)
    });

    chat.on('onSendUser', function (user) {
        chatController.UpdateUserFrom(user)
    });
    
    chat.on('onClientResolve', function () {
        chatController.ClientDiconected('Консультация со специалистом окончена. Ваш вопрос успешно решен и исключен из очереди.', Type.Success)
        chatController.SetAlert('Специалист отключил вас от системы. Через несколько секунд вы будете переведены на главную страницу.', Type.Info)
        /*chat.connection.stop()*/
        setTimeout(() => {
            window.location = '/Account/Signout'
        }, 5000)
    });
 
    chat.on('onConsultantResolve', function () {
        chatController.SuccessResolvedQuestion()
    });
    
    chat.on('onConsultantConnecting', function () {
        chatController.ConsultantConnectingToChat('Специалист подключился к чату, ожидайте ответа.', Type.Success)
    });

    chat.on('onClentDisconect', function () {
        chatController.ConsultanDisconected('Ваш собеседник покинул чат.', Type.Danger)
    });

    chat.on('onConsultanDisconect', function () {
        chatController.ClientDiconected(
            `<div>
                <p>Cвязь со специалистом потеряна. Вашими дальнейшими действиями могут быть:</p>
                <ul>
                    <li>Ожидать ответа специалиста(в данный момент ваш вопрос вернулся в очередь);</li>
                    <li>Если специалист ответил Вам на поставленный вопрос, можете завершить чат;</li>
                </ul>
            </div>
            `, Type.Danger)
    });

    chat.on('onHasEmptyQueue', function () {
        chatController.SetAlert('В данный момент очередь вопросов пуста. Ожидайте пользователей', Type.Warning)
    });
}
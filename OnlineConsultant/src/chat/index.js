/* global $ */
import {
    Type
} from './Utils/index';
import 'ms-signalr-client';
import InitConnection from './setup';
import InitClientEvents from './Events/clientChat';
import InitServerEvents from './Events/serverHub';
import ChatController from './Controllers/ChatController.js'

/*Подключаемся к хабу*/
let connection = $.hubConnection(),
    chat = connection.createHubProxy('ChatHub'),
    chatController = new ChatController();

/*Настройка подключения*/
InitConnection(connection);

/*Устанавливаем обработчики событий для клиента-чата*/
InitClientEvents(chat, chatController);



connection.start()
    .done(InitServerEvents(connection, chat, chatController))
    .fail(function () {
        chatController.SetAlert('Не удалось подключиться к системе.', Type.Danger);
    });

/*connection.disconnected(function () {
    //chatController.SetAlert('Вас отключили', Type.Info);
})*/

connection.error(function (error) {
    chatController.SetAlert(`Chat-${error}`, Type.Danger);
})
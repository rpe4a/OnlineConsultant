/* global $ */
export default function (connection, chatProxy, chatController) {
    return function () {
        console.log('Try connecting to server.');

        const client = connection.id;


        chatProxy.invoke('Connect', client)
            .done(function () {
                console.log('Client connected to server.');
            });

        chatController.GetNextUserEvent(() => {
            return chatProxy.invoke('ConnectionUserFromQueue')
        })

        chatController.ResolvedQuestion(() => {
            chatProxy.invoke('OnProcessed')
        })

        chatController.SendChatMessageEvent((userid, message) => {
            chatProxy.invoke('SendMessageToChat', userid, message)
        });

        $('#SignOutBtn').on('click', function () {
/*            connection.stop();*/
            chatProxy.invoke('Disconnected', true);
        })
    }
}
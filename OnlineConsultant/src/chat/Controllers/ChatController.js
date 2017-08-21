import ChatUserView from '../Views/ChatUsersView';
import ChatUserFromView from '../Views/ChatUserFromView';
import ChatMessageInputView from '../Views/ChatMessageInputView';
import ChatBodyView from '../Views/ChatBodyView';

export default class ChatController {
    constructor() {
        this.userView = new ChatUserView();
        this.userFromView = new ChatUserFromView();
        this.inputView = new ChatMessageInputView();
        this.bodyView = new ChatBodyView();
        /*this.userView.BindUserConteiner(this.UpdateUserFrom.bind(this))*/
    }

    ResolvedQuestion(handler) {
        this.inputView.Resolve(handler)
    }

    GetNextUserEvent(handler) {
        this.inputView.GetNextUser(handler)
    }

    SendChatMessageEvent(handler) {
        this.inputView.SendMessage(handler);
    }

    UpdateOnlineUsers(users) {
        this.userView.ClearUsersConteiner();

        users.forEach((user) => {
            this.userView.UpdateUsersConteiner(user);
        })
    }

    UpdateChatBody(MessagePackege) {
        this.bodyView.UpdateChatBody(MessagePackege);
    }

    UpdateUserFrom(user) {
        this.inputView.SetUserId(user);
        this.userFromView.PrepareChat(user);
    }

    ConsultanDisconected(message, type) {
        this.GetDefaultChatState()
        this.SetAlert(message, type)
    }

    ClientDiconected(message, type) {
        this.inputView.DisabledSendBtn();
        this.SetAlert(message, type)
    }

    ConsultantConnectingToChat(message, type) {
        this.inputView.EnabledSendBtn();
        this.SetAlert(message, type)
    }

    SetAlert(message, type) {
        this.bodyView.PushAlert(message, type);
    }

    GetDefaultChatState() {
        this.bodyView.DefaultState();
        this.inputView.DefaultState();
        this.userFromView.DefaultState();
    }

    SuccessResolvedQuestion() {
        this.GetDefaultChatState()
    }
}
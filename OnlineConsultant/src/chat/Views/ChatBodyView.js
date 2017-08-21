/* global $ */
export default class ChatBodyView {
    constructor() {
        this.preview = $('.chat-preview');
        this.ChatBody = $('.chat-body').not('.chat-preview');
    }

    DefaultState(){
        this.ShowPreview();
        this.ChatBody.find('.chat-message').remove();
        this.ChatBody.find('.chat-alert').remove();
    }

    HidePreview() {
        this.preview.addClass('hidden');
    }

    ShowPreview(){
        this.preview.removeClass('hidden');
    }

    PushAlert(message, type) {
        this.ChatBody.append(
            `<div class='alert alert-${type} chat-alert'>
                <button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                <h4><strong>Системное сообщение:</strong></h4>
                ${message}
            </div>
            `
        )

        this.ChatScrollBottom()
    }

    ChatScrollBottom(){
        this.ChatBody.scrollTop(this.ChatBody[0].scrollHeight);
    }

    UpdateChatBody({
        Message,
        IsSelf
    }) {
        this.HidePreview();

        if (IsSelf)
            this.ChatBody.append(`<div class="chat-message chat-message-in alert alert-default">
                                    ${Message}
                                </div>`)
        else
            this.ChatBody.append(`<div class="chat-message chat-message-out alert alert-info">
                                    ${Message}
                                </div>`)
        
        this.ChatScrollBottom()
    }
}
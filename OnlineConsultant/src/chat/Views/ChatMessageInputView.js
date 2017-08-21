/* global $ */
export default class ChatMessageInputView {
    constructor() {
        this.input = $('#ChatMessage');
        this.userFromId = $('#UserFromId');
        this.getNextBtn = $('#GetNextUserBtn');
        this.resolveBtn = $('#ResolverUserBtn');
        this.sendBtn = $('#SendMessageBtn');
    }

    DefaultState() {
        this.input.val('');
        this.userFromId.val('');
        this.getNextBtn.removeClass('hidden');
        this.resolveBtn.addClass('hidden');
        this.DisabledSendBtn()
    }

    SendMessage(handler) {
        this.sendBtn.on('click', () => {
            let message = this.input.val()

            if (message.length > 0) {
                handler(this.userFromId.val(), message.trim())
            }

            this.input.val('')
        })
    }

    DisabledSendBtn() {
        this.sendBtn.attr('disabled', 'disabled');
    }

    EnabledSendBtn() {
        this.sendBtn.removeAttr('disabled');
    }

    StartChatWithUser() {
        this.EnabledSendBtn();
        this.resolveBtn.removeClass('hidden');
        this.getNextBtn.addClass('hidden');
    }


    /*GetNextUser(handler, callback) {
        this.getNextBtn.on('click', () => {
            handler();
            callback().done(this.StartChatWithUser.bind(this));
        })
    }*/

    Resolve(handler) {
        this.resolveBtn.on('click', () => {
            handler()
        })
    }

    GetNextUser(handler) {
        this.getNextBtn.on('click', () => {
            handler().done((bool) => {
                if (bool) {
                    this.StartChatWithUser()
                }
            });
        })
    }

    SetUserId({
        Id
    }) {
        this.userFromId.val(Id)
    }
}
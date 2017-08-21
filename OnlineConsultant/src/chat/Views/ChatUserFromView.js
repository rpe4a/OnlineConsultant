/* global $ */
export default class ChatUserFromView {
    constructor() {
        this.UserFrom = $('#ChatUserFrom');
    }

    PrepareChat({
        Name
    }) {
        this.UserFrom.text(Name)
    }

    DefaultState(){
        this.UserFrom.text('Ожидание...');
        this.UserFrom.removeAttr('data-userid');
    }
}
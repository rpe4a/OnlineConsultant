/* global $ */
import {
    UserType
} from '../Utils/index';

export default class ChatUsersView {
    constructor() {
        this.UserConteiner = $('.chat-users');
    }

    ClearUsersConteiner() {
        this.UserConteiner.empty();
    }

    UpdateUsersConteiner({
        ConnectId,
        Name,
        Type,
        IsFree
    }) {

        let classUser = (Type === UserType.Consultant) ? (IsFree ? 'text-success' : 'text-warning') : 'text-success'

        this.UserConteiner.append(`<div class='chat-user-conteiner'>
                                            <i class='fa fa-circle ${classUser} padding-r-1'></i>
                                            <span class='chat-user' data-userid='${ConnectId}'>${Name}</span>
                                    </div>`)
    }



    /* BindUserConteiner(handler){
         this.UserConteiner.on('click', '.chat-user', handler) 
     }*/
}
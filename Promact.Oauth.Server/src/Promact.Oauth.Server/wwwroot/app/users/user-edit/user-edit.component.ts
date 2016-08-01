import {Component, Input} from "@angular/core";
import { UserService }   from '../user.service';
import {UserModel} from '../user.model';

@Component({
    template: '<h3>component</h3>'
})

export class UserEditComponent {
    users: Array<UserModel>;
    user: UserModel;

    constructor(private userService: UserService) {
        this.users = new Array<UserModel>();
        this.user = new UserModel();
    }


}


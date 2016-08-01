import {Component, Input} from "@angular/core";
import { UserService }   from '../user.service';
import {UserModel} from '../user.model';

@Component({
    templateUrl: './app/users/user-add/user-add.html'
    //template: '<h3>user add</h3>'
})

export class UserAddComponent {
    //users: Array<UserModel>;
    @Input()
    userModel: UserModel;

    constructor(private userService: UserService) {

        this.userModel = new UserModel();
    }

    addUser(userModel) {
        this.userService.registerUser(this.userModel).subscribe((users) => {
            //this.users = users
        }, err => {
        });
    }
}


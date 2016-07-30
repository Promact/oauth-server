import {Component} from "@angular/core";
import { UserService }   from '../user.service';
import {UserModel} from '../user.model';

@Component({
    templateUrl: "app/users/user-list/user-list.html"
})

export class UserListComponent {
    users: Array<UserModel>;
    user: UserModel;

    constructor(private userService: UserService) {
        this.users = new Array<UserModel>();
        this.user = new UserModel();
    }

    getUsers() {
        this.userService.getUsers().subscribe((users) => {
            this.users = users
        }, err => {
        });
    }

    ngOnInit() {
        this.getUsers();
    }
}
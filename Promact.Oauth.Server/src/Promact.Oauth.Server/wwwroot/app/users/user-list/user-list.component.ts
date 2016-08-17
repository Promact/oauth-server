import {Component} from "@angular/core";
import { Router, ROUTER_DIRECTIVES} from '@angular/router';
import { UserService }   from '../user.service';
import {UserModel} from '../user.model';

@Component({
    templateUrl: "app/users/user-list/user-list.html",
    directives: [ROUTER_DIRECTIVES]
})

export class UserListComponent {
    users: Array<UserModel>;
    user: UserModel;

    constructor(private userService: UserService, private router: Router) {
        this.users = new Array<UserModel>();
        this.user = new UserModel();
    }



    getUsers() {
        this.userService.getUsers().subscribe((users) => {
            this.users = users
        }, err => {
        });
    }

    userDetails(id){
        this.router.navigate(['/user/details', id]);
    }

    userEdit(id) {
        this.router.navigate(['/user/edit', id]);
    }

    ngOnInit() {
        this.getUsers();
    }
    
}
import { Component, OnInit } from "@angular/core";
import { Router } from '@angular/router';
import { UserService } from '../user.service';
import { UserModel } from '../user.model';
import { Md2Toast } from 'md2';
import { LoaderService } from '../../shared/loader.service';



@Component({
    moduleId: module.id,
    templateUrl: "user-list.html"
})

export class UserListComponent implements OnInit {
    users: Array<UserModel>;
    user: UserModel;
    constructor(private userService: UserService, private router: Router, private toast: Md2Toast, private loader: LoaderService) {
        this.users = new Array<UserModel>();
        this.user = new UserModel();
    }

    getUsers() {
        this.loader.loader = true;
        this.userService.getUsers().then((users) => {
            this.users = users;
            this.loader.loader = false;
        });
    }

    userDelete(userId) {
        this.loader.loader = true;
        this.userService.userDelete(userId).then((result) => {
            if (result === "") {
                this.toast.show('user succesfully deleted');
                this.getUsers();
                this.loader.loader = false;
            }
            else {
                this.toast.show('Remove the user from these project(s) ' + result +' and proceed to delete');
                this.loader.loader = false;
            }
            
        });
    }

    userDetails(id) {
        this.router.navigate(['/user/details', id]);
    }

    userEdit(id) {
        this.router.navigate(['/user/edit', id]);
    }

    ngOnInit() {
        this.getUsers();
    }

    reSendMail(user) {
        this.loader.loader = true;
        this.userService.reSendMail(user.Id).then((response) => {
            this.toast.show('Credentials re-send succesfully');
            this.loader.loader = false;
        }, err => {
        });
    }

}
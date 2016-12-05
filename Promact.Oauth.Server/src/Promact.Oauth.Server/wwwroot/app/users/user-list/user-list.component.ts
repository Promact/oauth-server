import { Component ,OnInit} from "@angular/core";
import { Router } from '@angular/router';
import { UserService } from '../user.service';
import { UserModel } from '../user.model';
import { Md2Toast } from 'md2';
import { LoaderService } from '../../shared/loader.service';
import { StringConstant } from '../../shared/stringconstant';


@Component({
    templateUrl: "app/users/user-list/user-list.html"
})

export class UserListComponent implements OnInit {
    users: Array<UserModel>;
    user: UserModel;
    constructor(private userService: UserService, private router: Router, private toast: Md2Toast, private loader: LoaderService, private stringConstant: StringConstant) {
        this.users = new Array<UserModel>();
        this.user = new UserModel();
    }
    getUsers() {
        this.loader.loader = true;
        this.userService.getUsers().subscribe((users) => {
            this.users = users;
            this.loader.loader = false;
        }, err => {
        });
    }

    userDetails(id) {
        this.router.navigate([this.stringConstant.userDetail, id]);
    }

    userEdit(id) {
        this.router.navigate([this.stringConstant.userEdit, id]);
    }

    ngOnInit() {
        this.getUsers();
    }

    reSendMail(user) {
        this.loader.loader = true;
        this.userService.reSendMail(user.Id).subscribe((response) => {
            if (response === true) {
                this.toast.show('Credentials re-send succesfully');
            }
            this.loader.loader = false;
        }, err => {
        });
    }

}
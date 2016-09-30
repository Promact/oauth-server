import {Component} from "@angular/core";
import {Router, ActivatedRoute } from "@angular/router";
import {Location} from "@angular/common";
import { LoginService } from '../../login.service';

import { UserService }   from '../user.service';
import {UserModel} from '../user.model';
import { Md2Toast } from 'md2/toast/toast';
import { LoaderService } from '../../shared/loader.service';

@Component({
    templateUrl: './app/users/user-edit/user-edit.html',
    providers: [Md2Toast]
})

export class UserEditComponent {
    user: UserModel;
    id: any;
    errorMessage: string;
    isSlackUserNameExist: boolean;
    listOfRoles: any;
    userRole: any;
    admin: any;

    constructor(private userService: UserService, private route: ActivatedRoute, private redirectionRoute: Router, private toast: Md2Toast,private loginService: LoginService,private loader : LoaderService) {
        this.user = new UserModel();
        this.listOfRoles = [];
    }

    ngOnInit() {
        this.getRole();
        this.getRoles();
        this.id = this.route.params.subscribe(params => {
            let id = this.route.snapshot.params['id'];
            this.userService.getUserById(id)
                .subscribe(
                user => this.user = user,
                error => this.errorMessage = <any>error)
        });
    }

    getRoles() {
        this.userService.getRoles().subscribe((result) => {
            if (result != null) {
                for (var i = 0; i < result.length; i++) {
                    this.listOfRoles.push(result[i]);
                }
            }
        }, err => {
        });
    }


    editUser(user: UserModel) {
        this.loader.loader = true;
        //if (this.isSlackUserNameExist == true) {
        this.userService.editUser(user).subscribe((result) => {
            if (result == true) {
                this.toast.show('User updated successfully.');
                this.redirectionRoute.navigate(['']);
            }
            else if (result == false) {
                this.toast.show('User Name or Slack User Name already exists.');
            }
            this.loader.loader = false;

        }, err => {
        });
        //}
        //else {
        //    this.toast.show('Slack User Name  already exists.');
        //}
    }

    //checkSlackUserName(slackUserName) {
    //    this.isSlackUserNameExist = false;
    //    this.userService.findUserBySlackUserName(slackUserName).subscribe((isSlackUserNameExist) => {
    //        if (isSlackUserNameExist) {
    //            this.isSlackUserNameExist = true;
    //        }
    //        else {
    //            this.isSlackUserNameExist = false;
    //        }
    //    }, err => {
    //    });
    //}


    goBack() {
        //this.location.back();
        this.redirectionRoute.navigate(['user/list']);
    }

    getRole() {
        this.loginService.getRoleAsync().subscribe((result) => {
            this.userRole = result;
            if (this.userRole.role === "Admin") {
                this.admin = true;
            }
            else {
                console.log(this.user);
                this.admin = false;
            }
        }, err => {
        });
    }
}


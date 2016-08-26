import {Component, Input} from "@angular/core";
import { UserService }   from '../user.service';
import {UserModel} from '../user.model';
import {Router, ROUTER_DIRECTIVES, ActivatedRoute} from '@angular/router';
import {Md2Toast} from 'md2/toast';

@Component({
    templateUrl: 'app/users/user-add/user-add.html',
    providers: [Md2Toast]
})

export class UserAddComponent {

    isEmailExist: boolean;
    isUserNameExist: boolean;
    isSlackUserNameExist: boolean;
    @Input()
    userModel: UserModel;

    constructor(private userService: UserService, private redirectionRoute: Router, private route: ActivatedRoute, private toast: Md2Toast) {
        this.userModel = new UserModel();
    }

    addUser(userModel) {
        userModel.JoiningDate = new Date(userModel.JoiningDate);
        if (this.isSlackUserNameExist == true) {
            if (this.isEmailExist == false) {
                this.userService.registerUser(this.userModel).subscribe((result) => {
                    if (result == true) {
                        this.toast.show('User added successfully.');
                        this.redirectionRoute.navigate(['/user/']);
                    }
                    else if (result == false) {
                        this.toast.show('User Name already exists.');
                    }
                }, err => {
                });
            }
            else {
                this.toast.show('Email Address already exists.');
            }
        }
        else {
            this.toast.show('Slack User Name  already exists.');
        }
    }

    checkEmail(email) {
        this.isEmailExist = false;

        this.userService.findUserByEmail(email + "@promactinfo.com").subscribe((isEmailExist) => {
            if (isEmailExist) {
                this.isEmailExist = true;
            }
            else {
                this.isEmailExist = false;

            }
        }, err => {
        });
    }

    checkSlackUserName(slackUserName)
    {
        this.isSlackUserNameExist = false;
        this.userService.findUserBySlackUserName(slackUserName).subscribe((isSlackUserNameExist) => {
            if (isSlackUserNameExist) {
                this.isSlackUserNameExist = true;
            }
            else {
                this.isSlackUserNameExist = false;
            }
        }, err => {
        });
    }

    goBack() {
        this.redirectionRoute.navigate(['/user/']);
    }
}


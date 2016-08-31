import {Component, Input} from '@angular/core';
import { UserService }   from '../user.service';
import {PasswordModel} from '../user-password.model';
import {FORM_DIRECTIVES, FormBuilder, Validators } from '@angular/forms';
import {Router, ROUTER_DIRECTIVES, ActivatedRoute} from '@angular/router';
import {Md2Toast} from 'md2/toast';

@Component({
    templateUrl: './app/users/user-change-password/user-change-password.html',
    directives: [FORM_DIRECTIVES],
    providers: [Md2Toast]
})

export class ChangePasswordComponent {
    isNotMatch: boolean;
    isSame: boolean;

    @Input()
    passwordModel: PasswordModel;

    constructor(private userService: UserService, private redirectionRoute: Router, private route: ActivatedRoute, private toast: Md2Toast) {
        this.passwordModel = new PasswordModel();
        this.isNotMatch = false;
    }

    changePassword(passwordModel) {
        this.userService.changePassword(this.passwordModel).subscribe((result) => {
            if (result == true) {
                this.toast.show('Password changed successfully');
                this.redirectionRoute.navigate(['admin/user']);
            }
            else if (result == false) {
                this.toast.show('Wrong password');
            }

        }, err => {

        });
    }

    matchPassword(confirmPassword, newPassword) {
        if (confirmPassword == undefined && newPassword == undefined) {
            this.isNotMatch = false;
        }
        else {
            if (confirmPassword == newPassword && (confirmPassword != undefined && newPassword != undefined)) {
                this.isNotMatch = false;
            }
            else {
                this.isNotMatch = true;
            }
        }
    }

    newPasswordIsSame(newPassword, oldPassword, confirmPassword) {
        // this.matchPassword(confirmPassword, newPassword);
        if (newPassword == oldPassword && (oldPassword != undefined && newPassword != undefined)) {
            this.isSame = true;
        }
        else {
            this.isSame = false;
        }
    }

    goBack() {
        this.redirectionRoute.navigate(['admin/user']);
    }
}
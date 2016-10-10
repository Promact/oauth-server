import {Component, Input} from '@angular/core';
import { UserService }   from '../users/user.service';
import {PasswordModel} from '../users/user-password.model';
import { FormBuilder, Validators } from '@angular/forms';
import { Router} from '@angular/router';
import { Md2Toast } from 'md2/toast/toast';
import { Location } from "@angular/common";
import { LoaderService } from '../shared/loader.service';

@Component({
    templateUrl: './app/change-password/change-password.html',
})

export class ChangePasswordComponent {
    isNotMatch: boolean;
    isSame: boolean;

    @Input()
    passwordModel: PasswordModel;

    constructor(private userService: UserService, private redirectionRoute: Router, private toast: Md2Toast, private loader: LoaderService) {
        this.passwordModel = new PasswordModel();
        this.isNotMatch = false;
    }

    changePassword(passwordModel) {
        this.loader.loader = true;
        this.userService.changePassword(this.passwordModel).subscribe((result) => {
            if (result == true) {
                this.toast.show('Password changed successfully');
                this.redirectionRoute.navigate(['']);
                this.loader.loader = false;
            }
            else if (result == false) {
                this.toast.show('Wrong password');
                this.loader.loader = false;
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
        //this.location.back();
        this.redirectionRoute.navigate(['user/list']);
        //this.redirectionRoute.navigate(['admin/user']);
    }
}
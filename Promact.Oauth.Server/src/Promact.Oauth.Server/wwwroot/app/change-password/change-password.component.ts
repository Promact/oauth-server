import { Component, Input } from '@angular/core';
import { UserService } from '../users/user.service';
import { PasswordModel } from '../users/user-password.model';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Md2Toast } from 'md2/toast/toast';
import { Location } from "@angular/common";
import { LoaderService } from '../shared/loader.service';

@Component({
    templateUrl: './app/change-password/change-password.html',
})

export class ChangePasswordComponent {
    isNotMatch: boolean;
    isSame: boolean;
    isInCorrect: boolean;

    @Input()
    passwordModel: PasswordModel;

    constructor(private userService: UserService, private redirectionRoute: Router, private toast: Md2Toast, private loader: LoaderService) {
        this.passwordModel = new PasswordModel();
        this.isNotMatch = false;
        this.isInCorrect = true;
    }

    checkOldPasswordIsValid() {
        if (this.passwordModel.OldPassword != undefined) {
            this.userService.checkOldPasswordIsValid(this.passwordModel.OldPassword).subscribe((result) => {
                this.isInCorrect = result;
            }, err => {
                console.log(err.statusText);
            });
        }
    }

    changePassword(passwordModel) {
        if (!this.isNotMatch) {
            this.loader.loader = true;
            this.userService.changePassword(this.passwordModel).subscribe((result) => {
                if (result.response == this.passwordModel.NewPassword)
                {
                    this.toast.show('Password changed successfully');
                    this.redirectionRoute.navigate(['']);
                }
                else {
                    this.toast.show(result.response);
                }
                this.loader.loader = false;
            }, err => {
                console.log(err.statusText);
                this.loader.loader = false;
            });
        }
    }

    matchPassword(confirmPassword, newPassword) {
        if (confirmPassword != undefined && newPassword != undefined) {
            if (confirmPassword == newPassword)
                this.isNotMatch = false;
            else
                this.isNotMatch = true;
        }
    }

    goBack() {
        //this.location.back();
        this.redirectionRoute.navigate(['user/list']);
        //this.redirectionRoute.navigate(['admin/user']);
    }
}
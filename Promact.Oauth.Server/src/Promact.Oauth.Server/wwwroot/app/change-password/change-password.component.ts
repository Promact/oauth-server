import { Component, Input } from '@angular/core';
import { UserService } from '../users/user.service';
import { PasswordModel } from '../users/user-password.model';
import { Router } from '@angular/router';
import { Md2Toast } from 'md2';
import { Location } from "@angular/common";
import { LoaderService } from '../shared/loader.service';


@Component({
    moduleId: module.id,
    templateUrl: 'change-password.html'
})

export class ChangePasswordComponent {
    isNotMatch: boolean;
    isInCorrect: boolean;
    showForm: boolean;

    @Input()
    passwordModel: PasswordModel;

    constructor(private userService: UserService, private redirectionRoute: Router, private toast: Md2Toast, private loader: LoaderService) {
        this.passwordModel = new PasswordModel();
        this.isNotMatch = false;
        this.isInCorrect = true;
        this.showForm = true;
    }

    checkOldPasswordIsValid() {
        this.isInCorrect = true;
        if (this.passwordModel.OldPassword !== "") {
            this.userService.checkOldPasswordIsValid(this.passwordModel.OldPassword).then((result) => {
                this.isInCorrect = result;
            }, err => {
                console.log(err.statusText);
            });
        }

    }

    changePassword(passwordModel: PasswordModel) {
        if (!this.isNotMatch) {
            this.loader.loader = true;
            this.userService.changePassword(passwordModel).then((result) => {
                if (result.errorMessage === null) {
                    this.toast.show('Password changed successfully');
                    this.redirectionRoute.navigate(['']);
                }
                else {
                    this.toast.show(result.errorMessage);
                }
                this.loader.loader = false;
            }, err => {
                console.log(err.statusText);
                this.loader.loader = false;
            });
        }
    }

    matchPassword(confirmPassword: string, newPassword: string) {
        if (confirmPassword !== "" && newPassword !== "") {
            if (confirmPassword === newPassword)
                this.isNotMatch = false;
            else
                this.isNotMatch = true;
        }
    }

    cancel() {
        this.loader.loader = true;
        this.showForm = false;
        setTimeout(() => {
            this.isInCorrect = true;
            this.isNotMatch = false;
            this.passwordModel = new PasswordModel();
            this.showForm = true;
            this.loader.loader = false;
        }, 0);

    }
}
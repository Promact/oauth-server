import { Component, Input } from '@angular/core';
import { UserService } from '../users/user.service';
import { PasswordModel } from '../users/user-password.model';
import { Router} from '@angular/router';
import { Md2Toast } from 'md2';
import { Location } from "@angular/common";
import { LoaderService } from '../shared/loader.service';
import { StringConstant } from '../shared/stringconstant';

@Component({
    templateUrl: './app/change-password/change-password.html',
})

export class ChangePasswordComponent {
    isNotMatch: boolean;
    isInCorrect: boolean;

    @Input()
    passwordModel: PasswordModel;

    constructor(private userService: UserService, private redirectionRoute: Router, private toast: Md2Toast, private loader: LoaderService, private stringConstant: StringConstant) {
        this.passwordModel = new PasswordModel();
        this.isNotMatch = false;
        this.isInCorrect = true;
    }

    checkOldPasswordIsValid() {
        this.isInCorrect = true;
        if (this.passwordModel.OldPassword !== "") {
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
            this.userService.changePassword(passwordModel).subscribe((result) => {
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

    matchPassword(confirmPassword, newPassword) {
        if (confirmPassword !== "" && newPassword !== "") {
            if (confirmPassword === newPassword)
                this.isNotMatch = false;
            else
                this.isNotMatch = true;
        }
    }

    goBack() {
        this.redirectionRoute.navigate([this.stringConstant.userList]);
    }
}
import {Component, Input} from '@angular/core';
import { UserService }   from '../user.service';
import {PasswordModel} from '../user-password.model';
import {Router, ROUTER_DIRECTIVES, ActivatedRoute} from '@angular/router';

@Component({
    templateUrl: './app/users/user-change-password/user-change-password.html'
})

export class ChangePasswordComponent {
    isNotMatch: boolean;
    isSame: boolean;
    
    @Input()
    passwordModel: PasswordModel;

    constructor(private userService: UserService, private redirectionRoute: Router, private route: ActivatedRoute) {
        this.passwordModel = new PasswordModel();
    }

    changePassword(passwordModel) {
        this.userService.changePassword(this.passwordModel).subscribe((password) => {
            this.redirectionRoute.navigate(['/']);
        }, err => {
            
        });
    }

    matchPassword(confirmPassword, newPassword) {
        if (confirmPassword == newPassword) {
            this.isNotMatch = false;
        }
        else {
            this.isNotMatch = true;
        }
    }

    newPasswordIsSame(newPassword, oldPassword, confirmPassword) {
        this.matchPassword(confirmPassword, newPassword);
        if (newPassword == oldPassword && (oldPassword != "" && newPassword != undefined)) {
            this.isSame = true;
        }
        else {
            this.isSame = false;
        }
    }

    goBack() {
        this.redirectionRoute.navigate(['/']);
    }
}
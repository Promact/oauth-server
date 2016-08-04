import {Component, Input} from '@angular/core';
import { UserService }   from '../user.service';
import {PasswordModel} from '../user-password.model';
import {Router} from '@angular/router';

@Component({
    templateUrl: './app/users/user-change-password/user-change-password.html'
})

export class ChangePasswordComponent {

    @Input()
    passwordModel: PasswordModel;

    constructor(private userService: UserService, private router: Router) {
        this.passwordModel = new PasswordModel();
    }

    changePassword(passwordModel) {
        this.userService.changePassword(this.passwordModel).subscribe((password) => {
        }, err => {
        });
    }

    goBack() {
        this.router.navigate(['/user']);
    }
}
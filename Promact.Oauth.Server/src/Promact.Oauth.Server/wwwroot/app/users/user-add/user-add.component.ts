import {Component, Input} from "@angular/core";
import { UserService }   from '../user.service';
import {UserModel} from '../user.model';
import {Router, ROUTER_DIRECTIVES, ActivatedRoute} from '@angular/router';


@Component({
    templateUrl: './app/users/user-add/user-add.html'
})

export class UserAddComponent {

    isEmailExist: boolean;
    isUserNameExist: boolean;

    @Input()
    userModel: UserModel;

    constructor(private userService: UserService, private redirectionRoute: Router, private route: ActivatedRoute) {
        this.userModel = new UserModel();
    }


    addUser(userModel) {
        this.userService.registerUser(this.userModel).subscribe((users) => {
            this.redirectionRoute.navigate(['/user']);
        }, err => {
        });
    }

    checkUserName(userName) {
        this.isUserNameExist = false;
        this.userService.findUserByUserName(userName).subscribe((isUserNameExist) => {
            if (isUserNameExist) {
                this.isUserNameExist = true;
            }
            else {
                this.isUserNameExist = false;
            }
        }, err => {
        });
    }

    checkEmail(email) {
        this.isEmailExist = false;
        this.userService.findUserByEmail(email).subscribe((isEmailExist) => {
            if (isEmailExist) {
                this.isEmailExist = true;
            }
            else {
                this.isEmailExist = false;
            }
        }, err => {
        });
    }

}


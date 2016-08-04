import {Component, Input} from "@angular/core";
import { UserService }   from '../user.service';
import {UserModel} from '../user.model';
import {Router, ROUTER_DIRECTIVES, ActivatedRoute} from '@angular/router';

@Component({
    templateUrl: './app/users/user-add/user-add.html'
})

export class UserAddComponent {

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
    
}


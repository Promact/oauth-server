import {Component} from "@angular/core";

import {UserModel} from '../user.model';
import {UserService} from '../user.service';
import { Router, ROUTER_DIRECTIVES, ActivatedRoute }from '@angular/router';


@Component({
    templateUrl: './app/users/user-details/user-details.html',
    directives: [ROUTER_DIRECTIVES],
   
})

export class UserDetailsComponent {
    user: UserModel;
    id: any;
    errorMessage: string;

    constructor(private userService: UserService, private route: ActivatedRoute, private redirectRoute: Router) {
        this.user = new UserModel();
    }


    ngOnInit() {

        this.id = this.route.params.subscribe(params => {
            let id = this.route.snapshot.params['id'];

            this.userService.getUserById(id)
                .subscribe(
                user => this.user = user,
                error => this.errorMessage = <any>error)
        });
    }

    goBack() {
        this.redirectRoute.navigate(['/']);
    }
}
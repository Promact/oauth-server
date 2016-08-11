import {Component, Input} from "@angular/core";
import {Router, ActivatedRoute, ROUTER_DIRECTIVES} from "@angular/router";
import {MD_INPUT_DIRECTIVES} from '@angular2-material/input/input';

import { UserService }   from '../user.service';
import {UserModel} from '../user.model';


@Component({
    templateUrl: './app/users/user-edit/user-edit.html',
    directives: [MD_INPUT_DIRECTIVES],
})

export class UserEditComponent {
    user: UserModel;
    id: any;
    errorMessage: string;

    constructor(private userService: UserService, private route: ActivatedRoute, private redirectionRoute: Router) {
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


    editUser(user: UserModel) {
        this.userService.editUser(user).subscribe((user) => {
            this.user = user;
            this.redirectionRoute.navigate(['/']);
        }, err => {
        });
    }


    goBack() {
        this.redirectionRoute.navigate(['/']);
    }

}


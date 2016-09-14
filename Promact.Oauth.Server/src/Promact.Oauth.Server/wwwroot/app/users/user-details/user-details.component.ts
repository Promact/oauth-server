﻿import {Component} from "@angular/core";
import { LoginService } from '../../login.service';
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
    admin: any;
    userRole: any;

    constructor(private userService: UserService, private route: ActivatedRoute, private redirectRoute: Router, private loginService: LoginService) {
        this.user = new UserModel();
    }


    ngOnInit() {
        this.getRole();
        this.id = this.route.params.subscribe(params => {
            let id = this.route.snapshot.params['id'];

            this.userService.getUserById(id)
                .subscribe(
                user => this.user = user,
                error => this.errorMessage = <any>error)
        });
    }

    goBack() {
        this.redirectRoute.navigate(['/user/list']);
    }

    edit(id: any) {
        this.redirectRoute.navigate(['/user/edit/' + id]);
    }

    getRole() {
        this.loginService.getRoleAsync().subscribe((result) => {
            this.userRole = result;
            if (this.userRole.role === "Admin") {
                this.admin = true;
            }
            else {
                console.log(this.user);
                this.admin = false;
            }
        }, err => {
        });
    }
}
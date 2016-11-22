﻿import { Component } from "@angular/core";
import { Router, ActivatedRoute } from "@angular/router";
import { Location } from "@angular/common";
import { LoginService } from '../../login.service';

import { UserService } from '../user.service';
import { UserModel } from '../user.model';
import { Md2Toast } from 'md2';
import { LoaderService } from '../../shared/loader.service';
import { UserRole } from "../../shared/userrole.model";

@Component({
    templateUrl: './app/users/user-edit/user-edit.html'

})

export class UserEditComponent {
    user: UserModel;
    id: any;
    errorMessage: string;
    isSlackUserNameExist: boolean;
    listOfRoles: any;
    admin: any;

    constructor(private userService: UserService, private route: ActivatedRoute, private redirectionRoute: Router, private toast: Md2Toast, private loginService: LoginService, private loader: LoaderService, private userRole: UserRole) {
        this.user = new UserModel();
        this.listOfRoles = [];
    }

    ngOnInit() {
        if (this.userRole.Role === "Admin") {
            this.admin = true;
        }
        else {
            this.admin = false;
        }
        this.getRoles();
        this.id = this.route.params.subscribe(params => {
            let id = this.route.snapshot.params['id'];
            this.userService.getUserById(id)
                .subscribe(
                user => this.user = user,
                error => this.errorMessage = <any>error)
        });
    }


    getRoles() {
        this.userService.getRoles().subscribe((result) => {
            if (result != null) {
                for (var i = 0; i < result.length; i++) {
                    this.listOfRoles.push(result[i]);
                }
            }
        }, err => {
            console.log(err.statusText);

        });
    }


    editUser(user: UserModel) {
        this.loader.loader = true;
        user.FirstName = user.FirstName.trim();
        this.userService.editUser(user).subscribe((result) => {
            this.toast.show('User updated successfully.');
            this.redirectionRoute.navigate(['']);
            this.loader.loader = false;

        }, err => {
            console.log(err.statusText);
            this.toast.show('User detail could not be edited successfully.');
            this.loader.loader = false;
        });

    }
    goBack() {
        this.redirectionRoute.navigate(['user/list']);
    }


}


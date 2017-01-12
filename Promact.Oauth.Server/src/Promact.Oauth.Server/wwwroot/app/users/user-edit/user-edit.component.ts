import { Component, OnInit } from "@angular/core";
import { Router, ActivatedRoute } from "@angular/router";
import { UserService } from '../user.service';
import { UserModel } from '../user.model';
import { UserRoleModel } from '../userrole.model';
import { Md2Toast } from 'md2';
import { LoaderService } from '../../shared/loader.service';
import { UserRole } from "../../shared/userrole.model";

@Component({
    templateUrl: './app/users/user-edit/user-edit.html'

})

export class UserEditComponent implements OnInit {
    user: UserModel;
    listOfRoles: Array<UserRoleModel>;
    admin: boolean;

    constructor(private userService: UserService, private route: ActivatedRoute, private redirectionRoute: Router, private toast: Md2Toast, private loader: LoaderService, private userRole: UserRole) {
        this.user = new UserModel();
        this.listOfRoles = new Array<UserRoleModel>();
    }

    ngOnInit() {
        if (this.userRole.Role === "Admin") {
            this.admin = true;
        }
        else {
            this.admin = false;
        }
        this.getRoles();
        this.route.params.subscribe(params => {
            let id = this.route.snapshot.params['id'];
            this.userService.getUserById(id)
                .subscribe(
                user => this.user = user,
                error => { console.log(error.statusText); });
        });
    }

    getRoles() {
        this.userService.getRoles().subscribe((result) => {
            if (result !== null) {
                for (let i = 0; i < result.length; i++) {
                    this.listOfRoles.push(result[i]);
                }
            }
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
            if (err.status === 404) {//Not Found 
                this.toast.show('User Name or Slack User Name already exists.');
            }
            this.loader.loader = false;
        });
    }

    goBack() {
        this.redirectionRoute.navigate(['']);
    }

}


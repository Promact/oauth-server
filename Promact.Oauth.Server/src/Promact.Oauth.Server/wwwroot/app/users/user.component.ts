import { Component, OnInit }   from '@angular/core';
import { Router }from '@angular/router';
import { LoginService } from '../login.service';
import { UserService } from './user.service';
import { UserRole } from "../shared/userrole.model";
import { StringConstant } from '../shared/stringconstant';

@Component({
    template: `
    <router-outlet></router-outlet>`,
    providers: [UserService, LoginService]
})

export class UserComponent implements OnInit {
    admin: boolean;
    constructor(private loginService: LoginService, private router: Router, private userRole: UserRole, private stringconstant: StringConstant) { }

   

    ngOnInit() {

        if (this.userRole.Role === this.stringconstant.admin) {
            this.router.navigate([this.stringconstant.userList]);
            this.admin = true;
        }
        else {
            this.router.navigate([this.stringconstant.userDetail + this.userRole.Id]);
            this.admin = false;
        }
    }
}
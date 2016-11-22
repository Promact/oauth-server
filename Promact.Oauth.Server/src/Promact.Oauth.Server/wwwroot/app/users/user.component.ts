import { Component, OnInit }   from '@angular/core';
import { Router }from '@angular/router';
import { LoginService } from '../login.service';
import { UserService } from './user.service';
import { UserRole } from "../shared/userrole.model";


@Component({
    template: `
    <router-outlet></router-outlet>`,
    providers: [UserService, LoginService]
})

export class UserComponent {
    user: any;
    admin: any;
    constructor(private loginService: LoginService, private router: Router, private userRole: UserRole) { }

   

    ngOnInit() {
   
        if (this.userRole.Role === "Admin") {
            this.router.navigate(['user/list']);
            this.admin = true;
        }
        else {
            this.router.navigate(['/user/details/' + this.userRole.Id]);
            this.admin = false;
        }
    }
}
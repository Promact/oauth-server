﻿import { Component, OnInit }   from '@angular/core';
import { Router }from '@angular/router';
import { UserService } from './user.service';
import { UserRole } from "../shared/userrole.model";
import { StringConstant } from '../shared/stringconstant';

@Component({
    template: `
    <router-outlet></router-outlet>`,
    providers: [UserService]
})

export class UserComponent implements OnInit {
    admin: boolean;
    constructor( private router: Router, private userRole: UserRole, private stringconstant: StringConstant) { }

   

    ngOnInit() {

        if (this.userRole.Role === this.stringconstant.admin) {
            this.router.navigate(['/user/list']);
            this.admin = true;
        }
        else {
            this.router.navigate(['/user/details/' + this.userRole.Id]);
            this.admin = false;
        }
    }
}
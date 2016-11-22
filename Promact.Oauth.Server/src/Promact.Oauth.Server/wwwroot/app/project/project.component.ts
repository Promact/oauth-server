import { Component, OnInit }   from '@angular/core';
import { Router }from '@angular/router';
import { ProjectService }   from './project.service';
import { LoginService } from '../login.service';
import { UserRole } from "../shared/userrole.model";

@Component({
    template: `
    <router-outlet></router-outlet>
    `,
    providers: [ProjectService]

})
export class ProjectComponent implements OnInit {
    admin: boolean;
    constructor(private loginService: LoginService, private router: Router, private userRole: UserRole) { }
    ngOnInit() {
        if (this.userRole.Role === "Admin") {
            this.admin = true;
        }
        else {
            this.admin = false;
        }
    }
}

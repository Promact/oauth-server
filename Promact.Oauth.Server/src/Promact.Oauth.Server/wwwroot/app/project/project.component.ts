import { Component, OnInit }   from '@angular/core';
import { Router }from '@angular/router';
import { ProjectService }   from './project.service';
import { UserRole } from "../shared/userrole.model";
import { StringConstant } from '../shared/stringconstant';

@Component({
    template: `
    <router-outlet></router-outlet>
    `,
    providers: [ProjectService]

})
export class ProjectComponent implements OnInit {
    admin: boolean;
    constructor(private router: Router, private userRole: UserRole, private stringconstant: StringConstant) { }
    ngOnInit() {
        if (this.userRole.Role === this.stringconstant.admin) {
            this.admin = true;
        }
        else {
            this.admin = false;
        }
    }
}

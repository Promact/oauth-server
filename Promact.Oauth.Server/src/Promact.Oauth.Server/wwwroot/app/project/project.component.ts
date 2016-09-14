import { Component, OnInit }   from '@angular/core';
import { Router, ROUTER_DIRECTIVES}from '@angular/router';
import { ProjectService }   from './project.service';
import { LoginService } from '../login.service';


@Component({
    template: `
    <router-outlet></router-outlet>
`,
    directives: [ROUTER_DIRECTIVES],
    providers: [ProjectService]

})
export class ProjectComponent {
    user: any;
    admin: any;
    constructor(private loginService: LoginService, private router: Router) { }

    getRole() {
        this.loginService.getRoleAsync().subscribe((result) => {
            this.user = result;
            if (this.user.role === "Admin") {
                this.router.navigate(['project/list']);
            }
            else {
                this.router.navigate(['project/list']);
            }
        }, err => {
        });
    }

    ngOnInit() {
        this.getRole();
    }
}

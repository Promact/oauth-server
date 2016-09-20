import { Component, OnInit }   from '@angular/core';
import { Router }from '@angular/router';
import { LoginService } from '../login.service';
import {UserService} from './user.service';

@Component({
    template: `
    <router-outlet></router-outlet>`,
    providers: [UserService]
})

export class UserComponent {
    user: any;
    admin: any;
    constructor(private loginService: LoginService, private router: Router) { }

    getRole() {
        this.loginService.getRoleAsync().subscribe((result) => {
            this.user = result;
            if (this.user.role === "Admin") {
                this.router.navigate(['user/list']);
                this.admin = true;
            }
            else {
                console.log(this.user);
                this.router.navigate(['/user/details/' + this.user.userId]);
                this.admin = false;
            }
        }, err => {
        });
    }

    ngOnInit() {
        this.getRole();
    }
}
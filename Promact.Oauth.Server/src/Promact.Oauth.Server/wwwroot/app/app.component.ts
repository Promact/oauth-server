import { Component } from '@angular/core';
import { LoginService } from './login.service';
import { Router } from '@angular/router';

@Component({
    selector: 'my-app',
    templateUrl: './app/index.html'
})
export class AppComponent {
    user: any;
    admin: any;
    constructor(private loginService: LoginService, private router: Router) { }

    getRole() {
        this.loginService.getRoleAsync().subscribe((result) => {
            this.user = result;
            if (this.user.role === "Admin") {
                this.admin = true;
            }
            else {
                this.admin = false;
            }
        }, err => {
        });
    }

    ngOnInit() {
        this.getRole();
    }
}



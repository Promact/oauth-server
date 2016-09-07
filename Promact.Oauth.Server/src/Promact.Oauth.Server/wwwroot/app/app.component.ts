import { Component } from '@angular/core';
import { LoginService } from './login.service';
import { Router, ROUTER_DIRECTIVES} from '@angular/router';


@Component({
    selector: 'my-app',
    template: '<router-outlet></router-outlet>',
    directives: [ROUTER_DIRECTIVES]
})
export class AppComponent {
    user: any;
    constructor(private loginService: LoginService, private router: Router) { }

    getRole() {
        this.loginService.getRoleAsync().subscribe((result) => {
            this.user = result;
            if (this.user.role === "Admin") {
                console.log("admin");
                this.router.navigate(['admin/user']);
            }
            else {
                console.log("User");
                this.router.navigate(['employee/' + this.user.userId]);
            }
        }, err => {
        });
    }

    ngOnInit() {
        this.getRole();
    }
}

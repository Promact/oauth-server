import { Component } from '@angular/core';
import { LoginService } from './login.service';
import { Router, ROUTER_DIRECTIVES} from '@angular/router';
import { LoginModel } from './LoginModel';


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
            if (this.user.role = "Admin") {
                this.router.navigate(['/']);
            }
            else {
                this.router.navigate(['user/employee/' + this.user.userId]);
            }
        }, err => {
        });
    }

    ngOnInit() {
        console.log("afkjsadkfjnasklnf");
        this.getRole();
     
    }
}

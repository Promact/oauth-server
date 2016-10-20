import { Component } from '@angular/core';
import { LoginService } from './login.service';
import { Router } from '@angular/router';
import { LoaderService } from './shared/loader.service'
import { UserRole } from "./shared/userrole.model";

@Component({
    selector: 'my-app',
    templateUrl: './app/index.html'
})
export class AppComponent {
    user: any;
    admin: any;
    constructor(private loginService: LoginService, private router: Router, private loader: LoaderService, private userRole: UserRole)
    { }
    ngOnInit() {
        if (this.userRole.Role === "Admin") {
            this.admin = true;
        }
        else {
            this.admin = false;
        }
    }
}



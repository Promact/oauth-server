import { Component } from '@angular/core';
import { LoginService } from './login.service';
import { Router } from '@angular/router';
import { LoaderService } from './shared/loader.service'
import { MyService } from "./shared/globalVariable";

@Component({
    selector: 'my-app',
    templateUrl: './app/index.html'
})
export class AppComponent {
    user: any;
    admin: any;
    constructor(private loginService: LoginService, private router: Router, private loader: LoaderService, private myService: MyService) {
        //debugger;
        this.getRole();
    }

    getRole() {
        this.loginService.getRoleAsync().subscribe((result) => {
            this.user = result;
            if (this.user.role === "Admin") {
                this.admin = true;
                this.myService.setValue(true);
            }
            else {
                this.admin = false;
                this.myService.setValue(false);
            }
        }, err => {
        });
    }

    ngOnInit() {
        this.getRole();
    }
}



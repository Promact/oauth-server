import { Component ,OnInit} from '@angular/core';
import { LoginService } from './login.service';
import { Router } from '@angular/router';
import { LoaderService } from './shared/loader.service';
import { UserRole } from "./shared/userrole.model";
import { StringConstant } from './shared/stringconstant';

@Component({
    selector: 'my-app',
    templateUrl: './app/index.html'
})
export class AppComponent implements OnInit {

    admin: boolean;
    userId: string;
    constructor(private loginService: LoginService, private router: Router, private loader: LoaderService, private userRole: UserRole, private stringconstant: StringConstant  ) { }
    ngOnInit() {
        if (this.userRole.Role === this.stringconstant.admin) {
            this.admin = true; }
        else {
            this.userId = this.userRole.Id;
            this.admin = false;
        }
    }
}



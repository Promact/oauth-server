import { Component ,OnInit} from '@angular/core';
import { Router } from '@angular/router';
import { LoaderService } from './shared/loader.service';
import { UserRole } from "./shared/userrole.model";
import { StringConstant } from './shared/stringconstant';

@Component({
    selector: 'my-app',
    moduleId: module.id,
    templateUrl: 'index.html'
})
export class AppComponent implements OnInit {

    admin: boolean;
    userId: string;
    constructor(private router: Router, public loader: LoaderService, private userRole: UserRole, private stringconstant: StringConstant) { }
    ngOnInit() {
         if (this.userRole.Role === this.stringconstant.admin) {
            this.admin = true; }
        else {
            this.userId = this.userRole.Id;
            this.admin = false;
        }
    }
}



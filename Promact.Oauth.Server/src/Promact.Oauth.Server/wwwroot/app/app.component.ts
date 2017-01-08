import { Component ,OnInit} from '@angular/core';
import { Router } from '@angular/router';
import { LoaderService } from './shared/loader.service';
import { UserRole } from "./shared/userrole.model";

@Component({
    selector: 'my-app',
    templateUrl: './app/index.html'
})
export class AppComponent implements OnInit {

    admin: boolean;
    userId: string;
    constructor(private router: Router, private loader: LoaderService, private userRole: UserRole) { }
    ngOnInit() {
        if (this.userRole.Role === "Admin") {
            this.admin = true; }
        else {
            this.userId = this.userRole.Id;
            this.admin = false;
        }
    }
}



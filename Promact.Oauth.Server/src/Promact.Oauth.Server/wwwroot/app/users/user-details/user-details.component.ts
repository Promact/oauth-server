import {Component} from "@angular/core";
import { LoginService } from '../../login.service';
import {UserModel} from '../user.model';
import {UserService} from '../user.service';
import { Router, ActivatedRoute }from '@angular/router';
import { UserRole } from "../../shared/userrole.model";

@Component({
    templateUrl: './app/users/user-details/user-details.html'   
})

export class UserDetailsComponent {
    user: UserModel;
    id: any;
    errorMessage: string;
    admin: any;
    

    constructor(private userService: UserService, private route: ActivatedRoute, private redirectRoute: Router, private loginService: LoginService, private userRole: UserRole) {
        this.user = new UserModel();
    }
    ngOnInit() {
        
        if (this.userRole.Role === "Admin") {
            this.admin = true;
        }
        else {
            this.admin = false;
        }
        this.id = this.route.params.subscribe(params => {
            let id = this.route.snapshot.params['id'];

            this.userService.getUserById(id)
                .subscribe(
                user => this.user = user,
                error => this.errorMessage = <any>error)
        });
    }

    goBack() {
        this.redirectRoute.navigate(['/user/list']);
    }

    edit(id: any) {
        this.redirectRoute.navigate(['/user/edit/' + id]);
    }

    
}
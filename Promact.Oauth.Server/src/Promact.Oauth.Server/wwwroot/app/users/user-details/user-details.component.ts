import {Component , OnInit} from "@angular/core";
import { LoginService } from '../../login.service';
import {UserModel} from '../user.model';
import {UserService} from '../user.service';
import { Router, ActivatedRoute }from '@angular/router';
import { UserRole } from "../../shared/userrole.model";
import { LoaderService } from '../../shared/loader.service';

@Component({
    //templateUrl: './app/users/user-details/user-details.html'   
    templateUrl: 'user-details.html'
})

export class UserDetailsComponent implements OnInit {
    user: UserModel;
    errorMessage: string;
    admin: boolean;
    

    constructor(private userService: UserService, private route: ActivatedRoute, private redirectRoute: Router, private loginService: LoginService,
        private loader: LoaderService, private userRole: UserRole) {
        this.user = new UserModel();
        this.admin = true;
    }
    ngOnInit() {
        
        if (this.userRole.Role === "Admin") {
            this.admin = true;
        }
        else {
            this.admin = false;
        }
        this.loader.loader = true;

        this.route.params.subscribe(params => {
            let id = this.route.snapshot.params['id'];
            this.userService.getUserById(id)
                .subscribe((user) => {
                    this.user = user,
                        this.loader.loader = false;
                }, err => {
                    console.log(err.statusText);
                    this.loader.loader = false;
                });


        });
    }

    goBack() {
        this.redirectRoute.navigate(['/user/list']);
    }

    edit(id: number) {
        this.redirectRoute.navigate(['/user/edit/' + id]);
    }

    
}
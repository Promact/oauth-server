import {Component , OnInit} from "@angular/core";
import {UserModel} from '../user.model';
import {UserService} from '../user.service';
import { Router, ActivatedRoute }from '@angular/router';
import { UserRole } from "../../shared/userrole.model";
import { LoaderService } from '../../shared/loader.service';
import { StringConstant } from '../../shared/stringconstant';

@Component({
    templateUrl: './app/users/user-details/user-details.html'   
})

export class UserDetailsComponent implements OnInit {
    user: UserModel;
    errorMessage: string;
    admin: boolean;
    

    constructor(private userService: UserService, private route: ActivatedRoute, private redirectRoute: Router,
        private loader: LoaderService, private userRole: UserRole, private stringConstant: StringConstant) {
        this.user = new UserModel();
        this.admin = true;
    }
    ngOnInit() {
        
        if (this.userRole.Role === this.stringConstant.admin) {
            this.admin = true;
        }
        else {
            this.admin = false;
        }
        this.loader.loader = true;

        this.route.params.subscribe(params => {
            let id = this.route.snapshot.params[this.stringConstant.paramsId];
            this.userService.getUserById(id)
                .then((user) => {
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
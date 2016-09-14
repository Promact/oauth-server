import {Component} from "@angular/core";
import {Router, ActivatedRoute, ROUTER_DIRECTIVES} from "@angular/router";
import {UserModel} from '../users/user.model';
import { UserService }   from '../users/user.service';
import {Location} from "@angular/common";

@Component({
    templateUrl: './app/Employee/Employee-edit.html',

})

export class EmployeeEditComponent {
    user: UserModel;
    id: any;
    errorMessage: string;


    constructor(private userService: UserService, private route: ActivatedRoute, private redirectionRoute: Router, private location: Location) {
        this.user = new UserModel();
    }

    ngOnInit() {
        this.id = this.route.params.subscribe(params => {
            this.id = this.route.snapshot.params['id'];

            this.userService.getUserById(this.id)
                .subscribe(
                user => this.user = user,
                error => this.errorMessage = <any>error)
        });
    }


    editUser(user: UserModel) {
        this.userService.editUser(user).subscribe((result) => {
            if (result == true) {
                //this.toast.show('User updated successfully.');
                this.redirectionRoute.navigate(['employee/' + this.id]);
            }
            else if (result == false) {
                //this.toast.show('User Name already exists.');
            }

        }, err => {
        });
    }


    goBack() {
        this.location.back();
    };
}

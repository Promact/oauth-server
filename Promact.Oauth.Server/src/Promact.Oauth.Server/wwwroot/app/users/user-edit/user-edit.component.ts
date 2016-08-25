import {Component} from "@angular/core";
import {Router, ActivatedRoute, ROUTER_DIRECTIVES} from "@angular/router";



import { UserService }   from '../user.service';
import {UserModel} from '../user.model';
import {Md2Toast} from 'md2/toast';

@Component({
    templateUrl: './app/users/user-edit/user-edit.html',

    providers: [Md2Toast]
})

export class UserEditComponent {
    user: UserModel;
    id: any;
    errorMessage: string;


    constructor(private userService: UserService, private route: ActivatedRoute, private redirectionRoute: Router, private toast: Md2Toast) {
        this.user = new UserModel();
    }

    ngOnInit() {
        this.id = this.route.params.subscribe(params => {
            let id = this.route.snapshot.params['id'];

            this.userService.getUserById(id)
                .subscribe(
                user => this.user = user,
                error => this.errorMessage = <any>error)
        });
    }


    editUser(user: UserModel) {
        this.userService.editUser(user).subscribe((result) => {
            if (result == true) {
                this.toast.show('User updated successfully.');
                this.redirectionRoute.navigate(['admin/user']);
            }
            else if (result == false) {
                this.toast.show('User Name already exists.');
            }
            
        }, err => {
        });
    }


    goBack() {
        this.redirectionRoute.navigate(['admin/user']);
    }
}


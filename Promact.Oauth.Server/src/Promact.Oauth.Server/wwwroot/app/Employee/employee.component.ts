import { Component, OnInit }   from '@angular/core';
import {UserModel} from '../users/user.model';
import {UserService} from '../users/user.service';
import { Router, ROUTER_DIRECTIVES, ActivatedRoute }from '@angular/router';

@Component({
    templateUrl: "app/Employee/UserIndex.html",
    directives: [ROUTER_DIRECTIVES]
})

export class EmployeeComponent {
    user: UserModel;
    id: any;
    errorMessage: string;

    constructor(private userService: UserService, private route: ActivatedRoute, private redirectRoute: Router) {
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
    userEdit(id) {
        this.redirectRoute.navigate(['employee/edit', id]);
    }
}
import { Component, OnInit }   from '@angular/core';
import { Router, ROUTER_DIRECTIVES}from '@angular/router';

import {UserService} from './user.service';

@Component({
    templateUrl: './app/users/user.html',
    //template: `<p>User List</p>
    //<router-outlet></router-outlet>`,
    directives: [ROUTER_DIRECTIVES],
    providers: [UserService]
})

export class UserComponent {
}
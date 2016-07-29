import {Component} from '@angular/core';
import {RouteConfig, ROUTER_DIRECTIVES, Router, RouteParams, ROUTER_PROVIDERS, RouterOutlet} from "@angular/router-deprecated";

import {UserAddComponent} from './user-add/user-add.component';
import {UserEditComponent} from './user-edit/user-edit.component';
import {UserListComponent} from './user-list/user-list.component';


@Component({
    selector: 'User',
    templateUrl: 'users/user.html',
    //template: '<p>ajhvjahvjahv</p>',
    directives: [RouterOutlet]
})


    @RouteConfig([
        {
            path: '/',
            name: 'List',
            component: UserListComponent,
            useAsDefault: true
        },
        {
            path: '/add',
            name: 'Add',
            component: UserAddComponent
        },
        {
            path: '/edit',
            name: 'Edit',
            component: UserEditComponent
        }
    ])

export class UserComponent {
    
}


import {Component, OnInit} from '@angular/core';
import {RouterConfig} from '@angular/router';
import {RouteConfig, ROUTER_DIRECTIVES, Router, RouteParams} from "@angular/router-deprecated";

import {UserService} from '../user-service';

@Component({
    selector: '',
    //templateUrl: './user-list/user-list.html'
    template: '<h3>user-list-component</h3>'
})


export class UserListComponent {

    constructor(private userService: UserService) { }

    ngOnInit()
    {
        this.userService.getUsers();
    }
}
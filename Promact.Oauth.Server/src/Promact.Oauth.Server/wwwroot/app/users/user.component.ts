﻿import { Component, OnInit }   from '@angular/core';
import { Router, ROUTER_DIRECTIVES}from '@angular/router';

import {UserService} from './user.service';

@Component({
    templateUrl: '',
    directives: [ROUTER_DIRECTIVES],
    providers: [UserService]
})

export class UserComponent {
}
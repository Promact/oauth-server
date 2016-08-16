import { Component } from '@angular/core';
import { ROUTER_DIRECTIVES } from '@angular/router';
import { HttpService}   from './http.service';
import {MdToolbar} from '@angular2-material/toolbar/toolbar';
import {MdButton, MdAnchor} from '@angular2-material/button/button';
import {MD_SIDENAV_DIRECTIVES} from '@angular2-material/sidenav/sidenav'
import {UserService} from './users/user.service';


@Component({
    selector: 'my-app',
    templateUrl: './app/index.html', 

    directives: [ROUTER_DIRECTIVES, MdToolbar, MdButton, MdAnchor, MD_SIDENAV_DIRECTIVES],

    providers: [HttpService, UserService]
})
export class AppComponent {  }

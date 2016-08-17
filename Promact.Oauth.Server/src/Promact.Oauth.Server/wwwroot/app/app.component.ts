import { Component } from '@angular/core';
import { ROUTER_DIRECTIVES } from '@angular/router';
import { HttpService}   from './http.service';
import {UserService} from './users/user.service';

@Component({
    selector: 'my-app',
    templateUrl: 'app/index.html',
    directives: [ROUTER_DIRECTIVES],
    providers: [HttpService, UserService]
})
export class AppComponent {   }

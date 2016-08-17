//import { Component } from '@angular/core';
//import { ROUTER_DIRECTIVES } from '@angular/router';
//import { HttpService}   from './http.service';
//import {UserService} from './users/user.service';

//@Component({
//    selector: 'my-app',
//    templateUrl: 'app/index.html',
//    directives: [ROUTER_DIRECTIVES],
//    providers: [HttpService, UserService]
//})
//export class AppComponent { }



import { Component } from '@angular/core';
import { ROUTER_DIRECTIVES } from '@angular/router';
import { HttpService}   from './http.service';
import {UserService} from './users/user.service';


@Component({
    selector: 'my-app',
    template: `
  <h1>{{title}}</h1>
  <nav>
    <a routerLink="/consumerapp" routerLinkActive="active">Consumer</a>
    <a routerLink="/project" routerLinkActive="active">Project</a>
    <a routerLink="/user" routerLinkActive="active">User</a>
    <a routerLink="/user/changePassword" routerLinkActive="active">Chnage Password</a>
  </nav>
  <router-outlet></router-outlet>
`,
})

export class AppComponent {

}

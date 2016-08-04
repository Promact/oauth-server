import { Component } from '@angular/core';
import { ROUTER_DIRECTIVES } from '@angular/router';
import { HttpService}   from './http.service';

@Component({
    selector: 'my-app',
    template: `
    <h1>Component Router</h1>
    <nav>
        <a routerLink="/user" routerLinkActive="active">Users</a>
        <a routerLink="/project" routerLinkActive="active">Projects</a>
        <a routerLink="/consumerapp" routerLinkActive="active">Consumer App</a>
    </nav>
    <router-outlet></router-outlet>
  `,
    directives: [ROUTER_DIRECTIVES],
    providers: [HttpService]
})
export class AppComponent { }

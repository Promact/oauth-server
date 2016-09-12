import { Component, OnInit }   from '@angular/core';
import { Router, ROUTER_DIRECTIVES}from '@angular/router';
import { UserService }   from '../users/user.service';

@Component({
    template: `
    <router-outlet></router-outlet>
`,
    directives: [ROUTER_DIRECTIVES],
    providers: [UserService]

})
export class ChangePassword { }

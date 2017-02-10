import { Component, OnInit }   from '@angular/core';
import { Router}from '@angular/router';
import { UserService }   from '../users/user.service';

@Component({
    moduleId : module.id,
    template: `
    <router-outlet></router-outlet>
`,
    providers: [UserService]

})
export class ChangePasswordMainComponent { }

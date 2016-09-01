import { Component, OnInit }   from '@angular/core';
import { Router, ROUTER_DIRECTIVES}from '@angular/router';
import { ProjectService }   from './project.service';

@Component({
    template: `
    <router-outlet></router-outlet>
`,
    directives: [ROUTER_DIRECTIVES],
    providers: [ProjectService]

})
export class ProjectComponent { }

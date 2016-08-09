import { Component, OnInit }   from '@angular/core';
import { Router, ROUTER_DIRECTIVES}from '@angular/router';
import { ConsumerAppService }   from './consumerapp.service';

@Component({
    template: `
    <p>Consumer App List</p>
    <router-outlet></router-outlet>
`,
    directives: [ROUTER_DIRECTIVES],
    providers: [ConsumerAppService]

})
export class ConsumerAppComponent {



}
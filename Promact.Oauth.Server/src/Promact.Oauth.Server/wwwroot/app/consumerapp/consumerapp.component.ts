import { Component, OnInit }   from '@angular/core';
import { Router, ROUTER_DIRECTIVES}from '@angular/router';
import { ConsumerAppService }   from './consumerapp.service';

@Component({
    template: `
    <router-outlet></router-outlet>`,
    directives: [ROUTER_DIRECTIVES],
    providers: [ConsumerAppService]

})
export class ConsumerAppComponent {


}
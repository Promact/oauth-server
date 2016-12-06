import { Component, OnInit }   from '@angular/core';
import { Router }from '@angular/router';
import { ConsumerAppService }   from './consumerapp.service';

@Component({
    template: `
    <router-outlet></router-outlet>`,
    providers: [ConsumerAppService]

})
export class ConsumerAppComponent {


}
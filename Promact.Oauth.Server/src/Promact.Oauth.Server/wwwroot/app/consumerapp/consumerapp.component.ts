import { Component }   from '@angular/core';
import { ConsumerAppService }   from './consumerapp.service';

@Component({
    template: `
    <router-outlet></router-outlet>`,
    providers: [ConsumerAppService]

})
export class ConsumerAppComponent {


}
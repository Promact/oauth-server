import { Component }   from '@angular/core';
import { ConsumerAppService }   from './consumerapp.service';

@Component({
    moduleId: module.id,
    template: `
    <router-outlet></router-outlet>`,
    providers: [ConsumerAppService]

})
export class ConsumerAppComponent {


}
import {provideRouter, RouterConfig} from '@angular/router';
import {ConsumerappListComponent} from "./consumerapp-list/consumerapp-list.component";
//import {ConsumerappAddComponent} from "./consumerapp-add/consumerapp-add.component.ts";

export const consumerRoute: RouterConfig = [{
    path: "consumerapp",
    component: ConsumerappListComponent,
    //children: [
    //    { path: '', component: ConsumerappListComponent }
    //]
}];
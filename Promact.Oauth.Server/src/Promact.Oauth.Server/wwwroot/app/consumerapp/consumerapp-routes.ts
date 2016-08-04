import {provideRouter, RouterConfig} from '@angular/router';
import {ConsumerAppComponent } from "./consumerapp.component";
import {ConsumerappListComponent} from "./consumerapp-list/consumerapp-list.component";
import {ConsumerappAddComponent} from "./consumerapp-add/consumerapp-add.component";

export const consumerRoute: RouterConfig = [{
    path: "consumerapp",
    component: ConsumerAppComponent,
    children: [{ path: '', component: ConsumerappListComponent },
        { path: 'add', component: ConsumerappAddComponent }]
}]; 
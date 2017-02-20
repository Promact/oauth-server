import { ModuleWithProviders } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";
import {ConsumerAppComponent } from "./consumerapp.component";
import {ConsumerappListComponent} from "./consumerapp-list/consumerapp-list.component";
import {ConsumerappAddComponent} from "./consumerapp-add/consumerapp-add.component";
import { ConsumerappEditComponent } from "./consumerapp-edit/consumer-edit.component";
import { authenticationService } from "../authentication.service";

const consumerRoutes: Routes = [{
    path: "consumerapp", canActivate: [authenticationService],
    component: ConsumerAppComponent,
    children: [{ path: '', component: ConsumerappListComponent},
        { path: 'add', component: ConsumerappAddComponent },
        { path: 'edit/:id', component: ConsumerappEditComponent },
    ]
}]; 
export const consumerRoute: ModuleWithProviders = RouterModule.forChild(consumerRoutes);


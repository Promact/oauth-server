import {provideRouter, RouterConfig} from '@angular/router';
import { AdminComponent } from '../Admin/admin.component';
import {ConsumerAppComponent} from '../consumerapp/consumerapp.component';
import {ProjectComponent} from "../project/project.component";
import {UserComponent} from '../users/user.component';

export const AdminRoute: RouterConfig = [{
    path: "admin",
    component: AdminComponent,
    children: [
        { path: 'consumerapp', component: ConsumerAppComponent },
        { path: 'project', component: ProjectComponent },
        { path: 'user', component: UserComponent },
    ]
}]; 
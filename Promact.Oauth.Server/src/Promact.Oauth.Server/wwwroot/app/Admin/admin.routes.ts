
import {consumerRoute} from '../consumerapp/consumerapp-routes';
import {userRoutes} from '../users/user.routes';
import {projectRoutes} from '../project/project.routes';
import {provideRouter, RouterConfig} from '@angular/router';
import { AdminComponent } from '../Admin/admin.component';
import {ConsumerAppComponent} from '../consumerapp/consumerapp.component';
import {ProjectComponent} from "../project/project.component";
import {UserComponent} from '../users/user.component';

export const AdminRoute: RouterConfig = [{
    path: "admin",
    component: AdminComponent,
    children: [
        ...consumerRoute,
        { path: 'consumerapp', component: ConsumerAppComponent },
        ...projectRoutes,
        { path: 'project', component: ProjectComponent },
        ...userRoutes,
        { path: 'user', component: UserComponent },
    ]
}]; 
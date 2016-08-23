
import {consumerRoute} from './consumerapp/consumerapp-routes';
import {userRoutes} from './users/user.routes';
import {projectRoutes} from './project/project.routes';
import {Routes, RouterModule } from '@angular/router';
import {ConsumerAppComponent} from './consumerapp/consumerapp.component';
import {ProjectComponent} from "./project/project.component";
import {UserComponent} from './users/user.component';
import { LoginComponent } from './login.component';
import { AdminComponent } from './Admin/admin.component';
import { AdminRoute } from './Admin/admin.routes';

const appRoutes: Routes = [

    { path: '', component: AdminComponent },
    //...AdminRoute,
    { path: 'admin', component: AdminComponent },
    ...consumerRoute,
    //{ path: 'consumerapp', component: ConsumerAppComponent },
    //...projectRoutes,
    //{ path: 'project', component: ProjectComponent },
    //...userRoutes,
    //{ path: 'user', component: UserComponent },
]

export const routing = RouterModule.forRoot(appRoutes);
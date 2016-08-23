
import {consumerRoute} from './consumerapp/consumerapp-routes';
import {userRoutes} from './users/user.routes';
import {projectRoutes} from './project/project.routes';
import {Routes, RouterModule } from '@angular/router';
import {ConsumerAppComponent} from './consumerapp/consumerapp.component';
import {ProjectComponent} from "./project/project.component";
import {UserComponent} from './users/user.component';
import {UserListComponent} from './users/user-list/user-list.component';

const appRoutes: Routes = [

    ...consumerRoute,
    { path: '', component: ConsumerAppComponent },
    ...projectRoutes,
    { path: 'project', component: ProjectComponent },
    ...userRoutes,
    { path: 'user', component: UserComponent },
]

export const routing = RouterModule.forRoot(appRoutes);
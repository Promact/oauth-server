import { provideRouter, RouterConfig } from '@angular/router';
import { ProjectComponent }  from './project/project.component';
import {projectRoutes} from './project/project.routes';
import {consumerRoute} from './consumerapp/consumerapp-routes';
import {userRoutes} from './users/user.routes';
import {UserComponent} from './users/user.component';
import {UserListComponent} from './users/user-list/user-list.component';


const routes: RouterConfig = [
    ...userRoutes,
    { path: '', component: UserListComponent },
    //{ path: '', component: UserComponent},

    ...projectRoutes,
    //{ path: '', component: ProjectComponent },
    
    
];

export const appRouterProviders = [
    provideRouter(routes)
];

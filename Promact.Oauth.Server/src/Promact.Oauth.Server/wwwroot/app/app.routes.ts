import { provideRouter, RouterConfig } from '@angular/router';
import { ProjectComponent }  from './project/project.component';
import {projectRoutes} from './project/project.routes';
import {consumerRoute} from './consumerapp/consumerapp-routes';
import {userRoutes} from './users/user.routes';
import {UserComponent} from './users/user.component';


const routes: RouterConfig = [
    ...projectRoutes,
    //{ path: '', component: ProjectComponent },
    ...userRoutes,
    { path: '', component: UserComponent },
    ...consumerRoute,
    
];

export const appRouterProviders = [
    provideRouter(routes)
];

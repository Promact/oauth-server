import { provideRouter, RouterConfig } from '@angular/router';
import { ProjectComponent }  from './project/project.component';
import {projectRoutes} from './project/project.routes';

import {userRoutes} from './users/user.routes';
import {UserComponent} from './users/user.component';

const routes: RouterConfig = [
    ...projectRoutes,
    //{ path: '', component: ProjectComponent },
    ...userRoutes,
    { path: '', component: UserComponent }
];

export const appRouterProviders = [
    provideRouter(routes)
];

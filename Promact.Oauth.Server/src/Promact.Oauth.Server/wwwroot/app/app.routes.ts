import { provideRouter, RouterConfig } from '@angular/router';
import { ProjectComponent }  from './project/project.component';
import {projectRoutes} from './project/project.routes';

const routes: RouterConfig = [
    ...projectRoutes,
    { path: '', component: ProjectComponent }
];
export const appRouterProviders = [
    provideRouter(routes)
];

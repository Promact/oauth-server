import { provideRouter, RouterConfig } from '@angular/router';
import {ProjectComponent} from "./project.component";
import {ProjectListComponent} from "./project-list/project-list.component";
export const projectRoutes: RouterConfig = [{
    path: "project",
    component: ProjectComponent,
    children: [
        { path: '', component: ProjectListComponent }
    ]
}];
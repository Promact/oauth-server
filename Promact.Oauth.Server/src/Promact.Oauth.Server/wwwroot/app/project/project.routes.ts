import { provideRouter, RouterConfig } from '@angular/router';
import {ProjectComponent} from "./project.component";
import {ProjectListComponent} from "./project-list/project-list.component";
import {ProjectAddComponent} from "./project-add/project-add.component";
import {ProjectEditComponent} from "./project-edit/project-edit.component";
export const projectRoutes: RouterConfig = [{
    path: "project",
    component: ProjectComponent,
    children: [
        { path: '', component: ProjectListComponent },
        { path: 'add', component: ProjectAddComponent },
        { path: 'edit/:id', component: ProjectEditComponent },
    ]
}];
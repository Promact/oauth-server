﻿import { ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ProjectComponent } from "./project.component";
import { ProjectListComponent } from "./project-list/project-list.component";
import { ProjectAddComponent } from "./project-add/project-add.component";
import { ProjectEditComponent } from "./project-edit/project-edit.component";
import { ProjectViewComponent } from "./project-view/project-view.component";
const projectRoutes: Routes =
    [{
        path: "project",
        component: ProjectComponent,
        children: [
            { path: '', component: ProjectListComponent },
            { path: 'list', component: ProjectListComponent },
            { path: 'add', component: ProjectAddComponent },
            { path: 'edit/:id', component: ProjectEditComponent },
            { path: 'view/:id', component: ProjectViewComponent }
        ]}
];
export const projectRoute: ModuleWithProviders = RouterModule.forChild(projectRoutes);

import { NgModule } from "@angular/core";
import { projectRoute } from "./project.routes";
import { ProjectComponent } from "./project.component";
import { ProjectListComponent } from "./project-list/project-list.component";
import { ProjectAddComponent } from "./project-add/project-add.component";
import { ProjectEditComponent } from "./project-edit/project-edit.component";
import { ProjectViewComponent } from "./project-view/project-view.component";
import { ProjectService } from "./project.service";

import { SharedModule } from '../shared/shared.module';

@NgModule({
    imports: [
        projectRoute,
        SharedModule
    ],
    declarations: [
        ProjectComponent,
        ProjectListComponent,
        ProjectEditComponent,
        ProjectAddComponent,
        ProjectViewComponent
    ],
    providers: [
        ProjectService
    ]
})
export class ProjectModule { }
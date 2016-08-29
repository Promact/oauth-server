import {Component} from "@angular/core";
import { ProjectService }   from '../project.service';
import {projectModel} from '../project.model'
import { ROUTER_DIRECTIVES, Router } from '@angular/router';
import {Md2Toast} from 'md2/toast';

@Component({
    templateUrl: "app/project/project-list/project-list.html",
    directives: [ROUTER_DIRECTIVES],
    providers: [Md2Toast] 
})
export class ProjectListComponent{
    projects: Array<projectModel>;
    project: projectModel;
    constructor(private router: Router, private proService: ProjectService, private toast: Md2Toast) {
        this.projects = new Array<projectModel>();
        this.project = new projectModel();
    }
    getPros() {
        this.proService.getPros().subscribe((projects) => {
            this.projects = projects
        }, err => {

        });
    }
    ngOnInit() {
        this.getPros();
    }
    editProject(Id) {
        this.router.navigate(['admin/project/edit', Id]);
    }
    viewProject(Id) {
        this.router.navigate(['admin/project/view', Id]);
    }
    
}
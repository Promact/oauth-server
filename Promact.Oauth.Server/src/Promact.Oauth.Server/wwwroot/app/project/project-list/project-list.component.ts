import {Component} from "@angular/core";
import { ProjectService }   from '../project.service';
import {projectModel} from '../project.model'
import { ROUTER_DIRECTIVES, Router } from '@angular/router';
import {Md2Toast} from 'md2/toast';
import { LoginService } from '../../login.service';


@Component({
    templateUrl: "app/project/project-list/project-list.html",
    directives: [ROUTER_DIRECTIVES],
    providers: [Md2Toast],
    
})
export class ProjectListComponent{
    projects: Array<projectModel>;
    project: projectModel;
    user: any;
    admin: any;
    constructor(private router: Router, private projectService: ProjectService, private toast: Md2Toast, private loginService: LoginService) {
        this.projects = new Array<projectModel>();
        this.project = new projectModel();
    }
    getProjects() {
        this.projectService.getProjects().subscribe((projects) => {
            this.projects = projects
        }, err => {

        });
    }
    ngOnInit() {
        this.getProjects();
        this.getRole();
    }
    editProject(Id) {
        this.router.navigate(['/project/edit', Id]);
    }
    viewProject(Id) {
        this.router.navigate(['/project/view', Id]);
    }

    getRole() {
        this.loginService.getRoleAsync().subscribe((result) => {
            this.user = result;
            if (this.user.role === "Admin") {
                this.router.navigate(['project/list']);
                this.admin = true;
            }
            else {
                this.router.navigate(['project/list']);
                this.admin = false;
            }
        }, err => {
        });
    }
}
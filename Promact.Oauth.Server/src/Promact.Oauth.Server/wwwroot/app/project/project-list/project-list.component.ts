import { Component } from "@angular/core";
import { ProjectService } from '../project.service';
import { projectModel } from '../project.model'
import { Router } from '@angular/router';
import { Md2Toast } from 'md2/toast/toast';
import { LoginService } from '../../login.service';
import { LoaderService } from '../../shared/loader.service';

@Component({
    templateUrl: 'app/project/project-list/project-list.html',
    

})
export class ProjectListComponent {
    projects: Array<projectModel>;
    project: projectModel;
    user: any;
    admin: any;
    constructor(private router: Router, private projectService: ProjectService, private toast: Md2Toast, private loginService: LoginService,
        private loader: LoaderService) {
        this.projects = new Array<projectModel>();
        this.project = new projectModel();
    }
    getProjects() {
        this.loader.loader = true;
        this.projectService.getProjects().subscribe((projects) => {
            this.projects = projects;
            this.loader.loader = false;
        }, err => {

        });
    }
    ngOnInit() {
        //this.loader.loader = true;
        this.getProjects();
        this.getRole();
        //this.loader.loader = false;
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
                //this.router.navigate(['project/list']);
                this.admin = true;
            }
            else {
                //this.router.navigate(['project/list']);
                this.admin = false;
            }
        }, err => {
        });
    }
}
import { Component } from "@angular/core";
import { ProjectService } from '../project.service';
import { projectModel } from '../project.model'
import { Router } from '@angular/router';
import { Md2Toast } from 'md2/toast/toast';
import { LoginService } from '../../login.service';
import { LoaderService } from '../../shared/loader.service';
import { UserRole } from "../../shared/userrole.model";


@Component({
    templateUrl: 'app/project/project-list/project-list.html',
    

})
export class ProjectListComponent {
    projects: Array<projectModel>;
    project: projectModel;
    user: any;
    admin: any;
    constructor(private router: Router, private projectService: ProjectService, private toast: Md2Toast, private loginService: LoginService,
        private loader: LoaderService, private userRole: UserRole) {
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
        if (this.userRole.Role === "Admin") {
            this.admin = true;}
        else {this.admin = false;}
        this.getProjects();
       
        
    }
    editProject(Id) {
        this.router.navigate(['/project/edit', Id]);
    }
    viewProject(Id) {
        this.router.navigate(['/project/view', Id]);
    }

    
}
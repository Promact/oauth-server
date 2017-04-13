import { Component, OnInit } from "@angular/core";
import { ProjectService } from '../project.service';
import { ProjectModel } from '../project.model';
import { Router } from '@angular/router';
import { Md2Toast } from 'md2';
import { LoaderService } from '../../shared/loader.service';
import { UserRole } from "../../shared/userrole.model";
import { DatePipe } from '@angular/common';
import { StringConstant } from '../../shared/stringconstant';

@Component({
    templateUrl: 'app/project/project-list/project-list.html',
})
export class ProjectListComponent implements OnInit {
    projects: Array<ProjectModel>;
    project: ProjectModel;
    admin: boolean;
    constructor(private router: Router, private projectService: ProjectService, private toast: Md2Toast,
        private loader: LoaderService, private userRole: UserRole, private stringConstant: StringConstant) {
        this.projects = new Array<ProjectModel>();
        this.project = new ProjectModel();
    }
    getProjects() {
        this.loader.loader = true;
        this.projectService.getProjects().then((projects) => {
            this.projects = projects;
            let datePipe = new DatePipe(this.stringConstant.medium);
            this.projects.forEach(project => {
                project.CreatedOns = datePipe.transform(project.CreatedDate, this.stringConstant.dateFormat);
                project.UpdatedOns = datePipe.transform(project.UpdatedDate, this.stringConstant.dateFormat);
            });
            this.loader.loader = false;
        });
    }
    ngOnInit() {
        if (this.userRole.Role === this.stringConstant.admin) {
            this.admin = true;}
        else {this.admin = false;}
        this.getProjects();
       
        
    }
    editProject(Id: number) {
        this.router.navigate(['/project/edit', Id]);
    }
    viewProject(Id: number) {
        this.router.navigate(['/project/view', Id]);
    }

    
}
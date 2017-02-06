import { Component, OnInit, EventEmitter, Output } from "@angular/core";
import { Location } from "@angular/common";
import { ProjectService } from '../project.service';
import { ProjectModel } from '../project.model';
import { UserModel } from '../../users/user.model';
import { Router, ActivatedRoute } from '@angular/router';
import { Md2Toast } from 'md2';
import { LoaderService } from '../../shared/loader.service';
import { StringConstant } from '../../shared/stringconstant';

@Component({
    moduleId: module.id,
    templateUrl: "project-edit.html",
    
})
export class ProjectEditComponent implements OnInit {
    project: ProjectModel;
    Userlist: Array<UserModel>;
    @Output() close = new EventEmitter();

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private toast: Md2Toast,
        private service: ProjectService,
        private location: Location,
        private loader: LoaderService,
        private stringConstant: StringConstant) { }
    /**
     * Get Project details and user details 
     */
    ngOnInit() {
        this.project = new ProjectModel();
        this.project.listUsers = new Array<UserModel>();
        this.project.applicationUsers = new Array<UserModel>();
        this.route.params.subscribe(params => {
            let id = +params[this.stringConstant.paramsId]; // (+) converts string 'id' to a number
            this.service.getProject(id).then(project => {
                this.project = project;
                this.service.getUsers().then(listUsers => {
                    this.project.listUsers = listUsers;
                });
            }, err => {
                this.toast.show("Project dose not exists.");
                this.loader.loader = false;
            });
        });
    }



    /**
     * navigation to projects page
     */
    gotoProjects() {
        this.location.back();
    }//this.router.navigate(['/project']); }

    /**
     * edit project and nevigate back to project page after update
     * @param project project that need update.
     */
    editProject(project: ProjectModel) {
        let bool = 0;
        for (let i = 0; i < project.applicationUsers.length; i++) {
            if (project.teamLeaderId === project.applicationUsers[i].Id) {
                this.toast.show("Teamleader is selected as team member,Please select another team leader");
                bool = 1;
            }
        }
        if (project.name === null) {
            this.toast.show("Project Name can not be blank");
        }


        else {
            if (bool === 0) {
                this.loader.loader = true;
                this.service.editProject(project).then((project) => {
                    if (project.name === null && project.slackChannelName === null) {
                        this.toast.show("Project Name and slackChannelName already exists");
                    }
                    else if (project.name !== null && project.slackChannelName === null) {
                        this.toast.show("slack Channel Name already exists");
                    }
                    else if (project.name === null && project.slackChannelName !== null) {
                        this.toast.show("Project Name already exists");
                    }
                    else {
                        this.toast.show("Project Successfully Updated.");
                        this.router.navigate(['/project/list']);
                    }

                    this.loader.loader = false;
                }, err => {
                    this.toast.show("Project could not be Successfully Updated.");
                    this.loader.loader = false;
                });
            }
        }


    }

}
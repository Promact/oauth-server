import { Component, OnInit } from "@angular/core";
import { ProjectService } from '../project.service';
import { ProjectModel } from '../project.model';
import { UserModel } from '../../users/user.model';
import { Router, ActivatedRoute } from '@angular/router';
import { Md2Toast } from 'md2';
import { LoaderService } from '../../shared/loader.service';


@Component({
    moduleId: module.id,
    templateUrl: "project-add.html",
})
export class ProjectAddComponent implements OnInit {

    private disabled: boolean = false;
    projects: Array<ProjectModel>;
    item: Array<string> = [];
    project: ProjectModel;
    Userlist: Array<UserModel>;
    constructor(private route: ActivatedRoute,private router: Router,private toast: Md2Toast,private proService: ProjectService,
        private loader: LoaderService) {
        this.projects = new Array<ProjectModel>();
        this.project = new ProjectModel();

    }
    /**
     * Project Added in database
     * @param project project table information pass
     */
    addProject(project: ProjectModel) {
        let bool = 0;
        for (let i = 0; i < project.ApplicationUsers.length; i++) {
            if (project.TeamLeaderId === project.ApplicationUsers[i].Id) {
                this.toast.show("Teamleader is selected as team member,Please select another team leader");
                bool = 1;
            }
        }

        if (project.Name === null && project.SlackChannelName === null) {
            this.toast.show("Project Name and Slack Channel Name can not be blank");
        }
        else if (project.Name === null && project.SlackChannelName !== null) {
            this.toast.show("Project Name can not be blank ");
        }
        else if (project.Name !== null && project.SlackChannelName === null) {
            this.toast.show("Slack Channel Name can not be blank");
        }
        else {
            if (bool === 0) {
                this.loader.loader = true;
                this.proService.addProject(project).then((project) => {
                    this.project = project;
                    if (project.Name === null && project.SlackChannelName === null) {
                        this.toast.show("Project and slackChannelName already exists");
                        this.proService.getUsers().then(listUsers => {
                            this.project.ListUsers = listUsers;
                            this.project.ApplicationUsers = new Array<UserModel>();

                        });
                    }
                    else if (project.Name !== null && project.SlackChannelName === null) {
                        this.toast.show("slackChannelName already exists");
                        this.proService.getUsers().then(listUsers => {
                            this.project.ListUsers = listUsers;
                            this.project.ApplicationUsers = new Array<UserModel>();

                        });

                    }
                    else if (project.Name === null && project.SlackChannelName !== null) {
                        this.toast.show("Project already exists");
                        this.proService.getUsers().then(listUsers => {
                            this.project.ListUsers = listUsers;
                            this.project.ApplicationUsers = new Array<UserModel>();

                        });
                    }
                    else {
                        this.toast.show("Project Successfully Added.");
                        this.router.navigate(['/project/list']);
                    }
                    this.loader.loader = false;
                }, err => {
                    this.toast.show("Project could not be Successfully Added.");
                    this.loader.loader = false;
                });
            }
        }
    }
    /**
     * getUser Method get User Information
     */
    ngOnInit() {
        this.project = new ProjectModel();
        this.route.params.subscribe(params => {
            this.proService.getUsers().then(listUsers => {
                this.project.ListUsers = listUsers;
                this.project.ApplicationUsers = new Array<UserModel>();

            });
        });

    }

    gotoProjects() {
        this.router.navigate(['project/list']);
    }

}


import { Component, OnInit } from "@angular/core";
import { ProjectService } from '../project.service';
import { ProjectModel } from '../project.model';
import { UserModel } from '../../users/user.model';
import { Router, ActivatedRoute } from '@angular/router';
import { Md2Toast } from 'md2';
import { LoaderService } from '../../shared/loader.service';
import { StringConstant } from '../../shared/stringconstant';

@Component({
    templateUrl: "app/project/project-add/project-add.html",
})
export class ProjectAddComponent implements OnInit {

    private disabled: boolean = false;
    projects: Array<ProjectModel>;
    item: Array<string> = [];
    project: ProjectModel;
    Userlist: Array<UserModel>;
    constructor(private route: ActivatedRoute,private router: Router,private toast: Md2Toast,private proService: ProjectService,
        private loader: LoaderService,private stringConstant: StringConstant) {
        this.projects = new Array<ProjectModel>();
        this.project = new ProjectModel();

    }
    /**
     * Project Added in database
     * @param project project table information pass
     */
    addProject(project: ProjectModel) {
        let bool = 0;
        for (let i = 0; i < project.applicationUsers.length; i++) {
            if (project.teamLeaderId === project.applicationUsers[i].Id) {
                this.toast.show("Teamleader is selected as team member,Please select another team leader");
                bool = 1;
            }
        }

        if (project.name === null && project.slackChannelName === null) {
            this.toast.show("Project Name and Slack Channel Name can not be blank");
        }
        else if (project.name === null && project.slackChannelName !== null) {
            this.toast.show("Project Name can not be blank ");
        }
        else if (project.name !== null && project.slackChannelName === null) {
            this.toast.show("Slack Channel Name can not be blank");
        }
        else {
            if (bool === 0) {
                this.loader.loader = true;
                this.proService.addProject(project).subscribe((project) => {
                    this.project = project;
                    if (project.name === null && project.slackChannelName === null) {
                        this.toast.show("Project and slackChannelName already exists");
                        this.proService.getUsers().subscribe(listUsers => {
                            this.project.listUsers = listUsers;
                            this.project.applicationUsers = new Array<UserModel>();

                        });
                    }
                    else if (project.name !== null && project.slackChannelName === null) {
                        this.toast.show("slackChannelName already exists");
                        this.proService.getUsers().subscribe(listUsers => {
                            this.project.listUsers = listUsers;
                            this.project.applicationUsers = new Array<UserModel>();

                        });

                    }
                    else if (project.name === null && project.slackChannelName !== null) {
                        this.toast.show("Project already exists");
                        this.proService.getUsers().subscribe(listUsers => {
                            this.project.listUsers = listUsers;
                            this.project.applicationUsers = new Array<UserModel>();

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
            this.proService.getUsers().subscribe(listUsers => {
                this.project.listUsers = listUsers;
                this.project.applicationUsers = new Array<UserModel>();

            });
        });

    }

    gotoProjects() {
        this.router.navigate(['/project/list']);
    }

}


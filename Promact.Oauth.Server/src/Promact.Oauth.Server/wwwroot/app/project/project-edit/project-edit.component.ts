import { Component, OnInit, EventEmitter, Output } from "@angular/core";
import { Location } from "@angular/common";
import { ProjectService } from '../project.service';
import { projectModel } from '../project.model'
import { UserModel } from '../../users/user.model';
import { Router, ActivatedRoute } from '@angular/router';
import { Md2Toast } from 'md2/toast';

@Component({
    templateUrl: "app/project/project-edit/project-edit.html",
    providers: [Md2Toast]
})
export class ProjectEditComponent implements OnInit {
    project: projectModel;
    private sub: any;
    Userlist: Array<UserModel>;
    @Output() close = new EventEmitter();

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private toast: Md2Toast,
        private service: ProjectService,
        private location: Location) { }
    /**
     * Get Project details and user details 
     */
    ngOnInit() {
        this.project = new projectModel();
        this.project.listUsers = new Array<UserModel>();
        this.project.applicationUsers = new Array<UserModel>();
        this.route.params.subscribe(params => {
            let id = +params['id']; // (+) converts string 'id' to a number
            this.service.getProject(id).subscribe(project => {
                this.project = project;
                //this.project.applicationUsers = project.applicationUsers;
                this.service.getUsers().subscribe(listUsers => {
                    this.project.listUsers = listUsers;
                    if (!this.project.applicationUsers)
                        this.project.applicationUsers = new Array<UserModel>();
                    for (let i = 0; i < this.project.listUsers.length; i++) {
                        for (let j = 0; j < this.project.applicationUsers.length; j++) {
                            if (this.project.listUsers[i].Id == this.project.applicationUsers[j].Id) {
                                this.project.applicationUsers[j].Email = this.project.listUsers[i].Email;
                                this.project.applicationUsers[j].IsActive = this.project.listUsers[i].IsActive;
                                this.project.applicationUsers[j].LastName = this.project.listUsers[i].LastName;
                                this.project.applicationUsers[j].UserName = this.project.listUsers[i].UserName;
                                this.project.applicationUsers[j].UniqueName = this.project.listUsers[i].UniqueName;
                                this.project.applicationUsers[j].NumberOfCasualLeave = this.project.listUsers[i].NumberOfCasualLeave;
                                this.project.applicationUsers[j].NumberOfSickLeave = this.project.listUsers[i].NumberOfSickLeave;
                                this.project.applicationUsers[j].JoiningDate = this.project.listUsers[i].JoiningDate;
                                this.project.applicationUsers[j].SlackUserName = this.project.listUsers[i].SlackUserName;
                            }
                        }
                    }
                })
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
    editProject(project: projectModel) {
        this.service.editProject(project).subscribe((project) => {
            if (project.name == null && project.slackChannelName == null) {
                this.toast.show("Project and slackChannelName already exists");
            }
            else if (project.name != null && project.slackChannelName == null) {
                this.toast.show("slackChannelName already exists");
            }
            else if (project.name == null && project.slackChannelName != null) {
                this.toast.show("Project already exists");
            }
            else {
                this.toast.show("Project Successfully Updated.");
                this.router.navigate(['/project/list'])
            }


        }, err => {

        });
    }


}


import {Component, OnInit, EventEmitter, Output} from "@angular/core";
import {Location} from "@angular/common";
import { ProjectService }   from '../project.service';
import {projectModel} from '../project.model'
import {UserModel} from '../../users/user.model';
import { ROUTER_DIRECTIVES, Router, ActivatedRoute } from '@angular/router';
import {Md2Toast} from 'md2/toast';
import {Md2Multiselect } from 'md2/multiselect';

@Component({
    templateUrl: "app/project/project-edit/project-edit.html",
    directives: [Md2Multiselect],
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
        this.project.ListUsers = new Array<UserModel>();
        this.project.ApplicationUsers = new Array<UserModel>();
        this.route.params.subscribe(params => {
            let id = +params['id']; // (+) converts string 'id' to a number
            this.service.getProject(id).subscribe(project => {
                this.project = project;
                this.service.getUsers().subscribe(listUsers => {
                    this.project.ListUsers = listUsers;
                    if (!this.project.ApplicationUsers)
                        this.project.ApplicationUsers = new Array<UserModel>();
                    for (let i = 0; i < this.project.ListUsers.length; i++) {
                        for (let j = 0; j < this.project.ApplicationUsers.length; j++)
                            {
                            if (this.project.ListUsers[i].Id == this.project.ApplicationUsers[j].Id) {
                                this.project.ApplicationUsers[j].Email = this.project.ListUsers[i].Email;
                                this.project.ApplicationUsers[j].IsActive = this.project.ListUsers[i].IsActive;
                                this.project.ApplicationUsers[j].LastName = this.project.ListUsers[i].LastName;
                                this.project.ApplicationUsers[j].UserName = this.project.ListUsers[i].UserName;
                                this.project.ApplicationUsers[j].UniqueName = this.project.ListUsers[i].UniqueName;
                                this.project.ApplicationUsers[j].NumberOfCasualLeave = this.project.ListUsers[i].NumberOfCasualLeave;
                                this.project.ApplicationUsers[j].NumberOfSickLeave = this.project.ListUsers[i].NumberOfSickLeave;
                                this.project.ApplicationUsers[j].JoiningDate = this.project.ListUsers[i].JoiningDate;
                                this.project.ApplicationUsers[j].SlackUserName = this.project.ListUsers[i].SlackUserName;
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
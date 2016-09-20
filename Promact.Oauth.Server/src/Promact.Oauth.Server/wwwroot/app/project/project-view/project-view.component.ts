import {Component, OnInit} from "@angular/core";
import { ProjectService }   from '../project.service';
import {projectModel} from '../project.model';
import {UserModel} from '../../users/user.model';
import {  Router, ActivatedRoute } from '@angular/router';
import {Location} from "@angular/common";
@Component({
    templateUrl: "app/project/project-view/project-view.html",
})
export class ProjectViewComponent implements OnInit {
    project: projectModel;
    private sub: any;
    Userlist: Array<UserModel>;
    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private service: ProjectService,
        private location: Location) { }
    /**
     * getporject feching the list of projects and getUser feching list of Users
     */
    ngOnInit() {
        this.route.params.subscribe(params => {
            let id = +params['id']; // (+) converts string 'id' to a number
            this.service.getProject(id).subscribe(project => {
                this.project = project;
                this.service.getUsers().subscribe(ListUsers => {
                    this.project.ListUsers = ListUsers;
                    if (!this.project.ApplicationUsers)
                        this.project.ApplicationUsers = new Array<UserModel>();
                    for (let i = 0; i < this.project.ListUsers.length; i++) {
                        for (let j = 0; j < this.project.ApplicationUsers.length; j++) {
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

    gotoProjects() {
        this.location.back();
    }
}
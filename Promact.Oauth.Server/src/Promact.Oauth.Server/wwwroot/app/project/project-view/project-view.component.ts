import { Component, OnInit } from "@angular/core";
import { ProjectService } from '../project.service';
import { projectModel } from '../project.model';
import { UserModel } from '../../users/user.model';
import { Router, ActivatedRoute } from '@angular/router';
import { Location } from "@angular/common";
@Component({
    templateUrl: "app/project/project-view/project-view.html",
})
export class ProjectViewComponent implements OnInit {
    project: projectModel;
    private sub: any;
    Userlist: Array<UserModel>;
    teamLeaderFirstName: string;
    teamLeaderEmail: string;
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

                if (this.project.teamLeaderId == null) {
                    this.teamLeaderFirstName = "";
                    this.teamLeaderEmail = "";
                }
                else {
                    this.teamLeaderFirstName = this.project.teamLeader.FirstName;
                    this.teamLeaderEmail = this.project.teamLeader.Email;

                }
                this.service.getUsers().subscribe(ListUsers => {
                    this.project.listUsers = ListUsers;
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
                    if (this.project.applicationUsers.length == 0)
                    {
                        var user = new UserModel();
                        user.UniqueName = "-";
                        this.project.applicationUsers.push(user);
                    }
                })
            });
        });
    }

    gotoProjects() {
        this.location.back();
    }
}

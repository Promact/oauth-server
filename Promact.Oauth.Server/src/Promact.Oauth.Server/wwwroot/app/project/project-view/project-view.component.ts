import { Component, OnInit } from "@angular/core";
import { ProjectService } from '../project.service';
import { ProjectModel } from '../project.model';
import { UserModel } from '../../users/user.model';
import { Router, ActivatedRoute } from '@angular/router';
import { Location } from "@angular/common";
import { Md2Toast } from 'md2';
import { StringConstant } from '../../shared/stringconstant';

@Component({
    templateUrl: "app/project/project-view/project-view.html",
})
export class ProjectViewComponent implements OnInit {
    project: ProjectModel;
    Userlist: Array<UserModel>;
    teamLeaderFirstName: string;
    teamLeaderEmail: string;
    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private service: ProjectService,
        private location: Location,
        private toast: Md2Toast,
        private stringConstant: StringConstant) { }
    /**
     * getporject feching the list of projects and getUser feching list of Users
     */
    ngOnInit() {
        this.route.params.subscribe(params => {
            let id = +params[this.stringConstant.paramsId]; // (+) converts string 'id' to a number
            this.service.getProject(id).subscribe(project => {
                this.project = project;

                if (this.project.teamLeaderId === null) {
                    this.teamLeaderFirstName = "";
                    this.teamLeaderEmail = "";
                }
                else {
                    this.teamLeaderFirstName = this.project.teamLeader.FirstName;
                    this.teamLeaderEmail = this.project.teamLeader.Email;

                }
                this.service.getUsers().subscribe(ListUsers => {
                    this.project.listUsers = ListUsers;
                    if (this.project.applicationUsers.length === 0) {
                        let user = new UserModel();
                        user.UniqueName = "-";
                        this.project.applicationUsers.push(user);
                    }
                });
            }, err => {
                this.toast.show("Project dose not exists.");
            });

        });
    }

    gotoProjects() {
        this.location.back();
    }
}

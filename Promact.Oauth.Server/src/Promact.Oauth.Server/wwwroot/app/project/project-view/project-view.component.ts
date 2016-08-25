import {Component, OnInit} from "@angular/core";
import { ProjectService }   from '../project.service';
import {projectModel} from '../project.model';
import {UserModel} from '../../users/user.model';
import { ROUTER_DIRECTIVES, Router, ActivatedRoute } from '@angular/router';
import {Location} from "@angular/common";
@Component({
    templateUrl: "app/project/project-view/project-view.html",
    directives: []
})
export class ProjectViewComponent implements OnInit {
    pro: projectModel;
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
            this.service.getProject(id).subscribe(pro => {
                this.pro = pro;
                this.service.getUsers().subscribe(listUsers => {
                    this.pro.listUsers = listUsers;
                    if (!this.pro.applicationUsers)
                        this.pro.applicationUsers = new Array<UserModel>();
                    for (let i = 0; i < this.pro.listUsers.length; i++) {
                        for (let j = 0; j < this.pro.applicationUsers.length; j++) {
                            if (this.pro.listUsers[i].Id == this.pro.applicationUsers[j].Id) {
                                this.pro.applicationUsers[j].Email = this.pro.listUsers[i].Email;
                                this.pro.applicationUsers[j].IsActive = this.pro.listUsers[i].IsActive;
                                this.pro.applicationUsers[j].LastName = this.pro.listUsers[i].LastName;
                                this.pro.applicationUsers[j].UserName = this.pro.listUsers[i].UserName;
                                this.pro.applicationUsers[j].UniqueName = this.pro.listUsers[i].UniqueName;
                                this.pro.applicationUsers[j].NumberOfCasualLeave = this.pro.listUsers[i].NumberOfCasualLeave;
                                this.pro.applicationUsers[j].NumberOfSickLeave = this.pro.listUsers[i].NumberOfSickLeave;
                                this.pro.applicationUsers[j].JoiningDate = this.pro.listUsers[i].JoiningDate;
                                this.pro.applicationUsers[j].SlackUserName = this.pro.listUsers[i].SlackUserName;
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
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
                })
            });
        });
    }

    gotoProjects() {
        this.location.back();//this.router.navigate(['/project/']); }
    }
}
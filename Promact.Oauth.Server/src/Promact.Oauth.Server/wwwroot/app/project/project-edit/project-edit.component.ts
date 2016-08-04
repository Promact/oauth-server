import {Component, OnInit, OnDestroy, EventEmitter, Output} from "@angular/core";
import { ProjectService }   from '../project.service';
import {projectModel} from '../project.model'
import {UserModel} from '../../users/user.model';
import { ROUTER_DIRECTIVES, Router, ActivatedRoute } from '@angular/router';

@Component({
    templateUrl: "app/project/project-edit/project-edit.html",
    directives: []
})
export class ProjectEditComponent implements OnInit, OnDestroy {
    pro: projectModel;
    private sub: any;
    navigated = false;
    Userlist: Array<UserModel>;
    @Output() close = new EventEmitter();

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private service: ProjectService) { }

    ngOnInit() {
        this.route.params.subscribe(params => {
            let id = +params['id']; // (+) converts string 'id' to a number
            this.service.getProject(id).subscribe(pro => {
                this.pro = pro;
                this.service.getUsers().subscribe(listUsers => {
                    this.pro.listUsers = listUsers;
                    if (!this.pro.applicatioUsers)
                        this.pro.applicatioUsers = new Array<UserModel>();
                })
            });
        });
    }

    ngOnDestroy() {
        this.sub.unsubscribe();
    }

    /**
     * navigation to projects page
     */
    gotoProjects() { this.router.navigate(['/project/']); }

    /**
     * edit project and nevigate back to project page after update
     * @param pro project that need update.
     */
    editProject(pro: projectModel) {
        this.service.editProject(pro).subscribe((pro) => {
            this.pro = pro
            this.router.navigate(['/project/'])
        }, err => {

        });
    }
    goBack(savedProject: projectModel = null) {
        this.close.emit(savedProject);
        if (this.navigated) { window.history.back(); }
    }

    addUser(newObj: string) {
        var obj = this.pro.listUsers.find(x => x.firstName == newObj);
        this.pro.applicatioUsers.push(obj);
    }
}
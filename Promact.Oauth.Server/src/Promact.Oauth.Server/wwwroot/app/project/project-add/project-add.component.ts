import {Component} from "@angular/core";
import { ProjectService }   from '../project.service';
import {projectModel} from '../project.model'
import {UserModel} from '../../users/user.model';
import { ROUTER_DIRECTIVES, Router, ActivatedRoute } from '@angular/router';
import {Md2Toast} from 'md2/toast';
import {Md2Multiselect } from 'md2/multiselect';
@Component({
    selector: 'md2-select',
    templateUrl: "app/project/project-add/project-add.html",
    directives: [Md2Multiselect],
    providers: [Md2Toast]
})
export class ProjectAddComponent {

    private disabled: boolean = false;

    projects: Array<projectModel>;
    item: Array<string> = [];
    project: projectModel;
    private sub: any
    Userlist: Array<UserModel>;
    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private toast: Md2Toast,
        private proService: ProjectService) {
        this.projects = new Array<projectModel>();
        this.project = new projectModel();

    }
    /**
     * Project Added in database
     * @param pro project table information pass
     */
    addProject(project: projectModel) {
        this.proService.addProject(project).subscribe((project) => {
            this.project = project;
            if (project.name == null && project.slackChannelName == null) {
                this.toast.show("Project and slackChannelName already exists");
                this.proService.getUsers().subscribe(listUsers => {
                    this.project.ListUsers = listUsers;
                    this.project.ApplicationUsers = new Array<UserModel>();

                });
            }
            else if (project.name != null && project.slackChannelName == null) {
                this.toast.show("slackChannelName already exists");
                this.proService.getUsers().subscribe(listUsers => {
                    this.project.ListUsers = listUsers;
                    this.project.ApplicationUsers = new Array<UserModel>();

                });

            }
            else if (project.name == null && project.slackChannelName != null) {
                this.toast.show("Project already exists");
                this.proService.getUsers().subscribe(listUsers => {
                    this.project.ListUsers = listUsers;
                    this.project.ApplicationUsers = new Array<UserModel>();

                });
            }
            else {
                this.toast.show("Project Successfully Added.");
                this.router.navigate(['admin/project/'])
            }

        }, err => {

        });
    }
    /**
     * getUser Method get User Information
     */
    ngOnInit() {
        this.project = new projectModel();
        this.sub = this.route.params.subscribe(params => {
            this.proService.getUsers().subscribe(listUsers => {
                this.project.ListUsers = listUsers;
                this.project.ApplicationUsers = new Array<UserModel>();

            });
        });

    }

    gotoProjects() {
        this.router.navigate(['admin/project/']);
    }

}
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
    pro: projectModel;
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
        this.pro = new projectModel();
        this.pro.listUsers = new Array<UserModel>();
        this.pro.applicationUsers = new Array<UserModel>();
        this.route.params.subscribe(params => {
            let id = +params['id']; // (+) converts string 'id' to a number
            this.service.getProject(id).subscribe(pro => {
                this.pro = pro;
                this.service.getUsers().subscribe(listUsers => {
                    this.pro.listUsers = listUsers;
                    if (!this.pro.applicationUsers)
                        this.pro.applicationUsers = new Array<UserModel>();
                    for (let i = 0; i<this.pro.listUsers.length; i++) {
                        for (let j = 0; j < this.pro.applicationUsers.length; j++)
                            {
                            if (this.pro.listUsers[i].Id == this.pro.applicationUsers[j].Id) {
                                this.pro.applicationUsers[j].Email = this.pro.listUsers[i].Email;
                                this.pro.applicationUsers[j].IsActive = this.pro.listUsers[i].IsActive;
                                this.pro.applicationUsers[j].LastName = this.pro.listUsers[i].LastName;
                                this.pro.applicationUsers[j].UserName = this.pro.listUsers[i].UserName
                            }//break; 
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
     * @param pro project that need update.
     */
    editProject(pro: projectModel) {
        this.service.editProject(pro).subscribe((pro) => {
            //this.pro = pro
            //this.router.navigate(['/project'])
            this.toast.show("Project is edited successfully")  
            this.router.navigate(['/project/'])
            //this.location.back();
        }, err => {

        });
    }
    
    deleteSelectedUser(newObj: string) {
        
    }
}
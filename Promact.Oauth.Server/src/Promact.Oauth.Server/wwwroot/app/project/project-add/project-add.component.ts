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

    pros: Array<projectModel>;
    item: Array<string> = [];
    pro: projectModel;
    private sub: any
    Userlist: Array<UserModel>;
    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private toast: Md2Toast,
        private proService: ProjectService) {
        this.pros = new Array<projectModel>();
        this.pro = new projectModel();
        
    }
    /**
     * Project Added in database
     * @param pro project table information pass
     */
    addProject(pro: projectModel) {
        this.proService.addProject(pro).subscribe((pro) => {
            this.pro = pro;
            this.toast.show("Project is added successfully")    
            this.router.navigate(['/project/'])
        }, err => {

        });
    } 
    /**
     * getUser Method get User Information
     */
    ngOnInit() {
        this.pro = new projectModel();
        this.sub = this.route.params.subscribe(params => {
            this.proService.getUsers().subscribe(listUsers => {
                this.pro.listUsers = listUsers;
                this.pro.applicationUsers = new Array<UserModel>();

            });
        });
       
    }
   
}
import {Component} from "@angular/core";
import { ProjectService }   from '../project.service';
import {projectModel} from '../project.model'
import {UserModel} from '../../users/user.model';
import { ROUTER_DIRECTIVES, Router, ActivatedRoute } from '@angular/router';
@Component({
    templateUrl: "app/project/project-add/project-add.html",
    directives: [],
   
})
export class ProjectAddComponent {
    pros: Array<projectModel>;

    pro: projectModel;
    private sub: any
    Userlist: Array<UserModel>;
    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private proService: ProjectService) {
        this.pros = new Array<projectModel>();
        this.pro = new projectModel();
        
    }
    addProject(pro: projectModel) {
        this.proService.addProject(pro).subscribe((pro) => {
            this.pro = pro
            this.router.navigate(['/project/'])
        }, err => {

        });
    } 
    ngOnInit() {
        this.pro = new projectModel();
        this.sub = this.route.params.subscribe(params => {
            this.proService.getUsers().subscribe(listUsers => {
                this.pro.listUsers = listUsers;
                this.pro.applicatioUsers = new Array<UserModel>();

            });
        });
        //this.addProject(this.pro);
    }

    //onChangeObj(newObj: UserModel) {
    //    //console.log(newObj);
    //    this.Userlist.push(newObj);
    //    // ... do other stuff here ...
    //}

    addUser(newObj: string) {
        //this.Userlist.push(newObj);
        var obj = this.pro.listUsers.find(x => x.firstName == newObj);
        //this.Userlist.push(obj);
        this.pro.applicatioUsers.push(obj);
    }
}
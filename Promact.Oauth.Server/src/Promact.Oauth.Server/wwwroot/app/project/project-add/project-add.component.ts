import {Component} from "@angular/core";
import { ProjectService }   from '../project.service';
import {projectModel} from '../project.model'
import { ROUTER_DIRECTIVES } from '@angular/router';
@Component({
    templateUrl: "app/project/project-add/project-add.html",
    directives: []
})
export class ProjectAddComponent {
    pros: Array<projectModel>;
    pro: projectModel;
    constructor(private proService: ProjectService) {
        this.pros = new Array<projectModel>();
        this.pro = new projectModel();
    }
    addProject(pro: projectModel) {
        this.proService.addProject(pro).subscribe((pro) => {
            this.pro = pro
        }, err => {

        });
    } 
    ngOnInit() {
        this.pro = new projectModel();
        //this.addProject(this.pro);
    }
}
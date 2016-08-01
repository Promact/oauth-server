import {Component} from "@angular/core";
import { ProjectService }   from '../project.service';
import {projectModel} from '../project.model'
import { ROUTER_DIRECTIVES, Router } from '@angular/router';
@Component({
    templateUrl: "app/project/project-list/project-list.html",
    directives: [ROUTER_DIRECTIVES]
})
export class ProjectListComponent{
    pros: Array<projectModel>;
    pro: projectModel;
    constructor(private router: Router,private proService: ProjectService) {
        this.pros = new Array<projectModel>();
        this.pro = new projectModel();
    }
    getPros() {
        this.proService.getPros().subscribe((pros) => {
            this.pros = pros
        }, err => {

        });
    }
    ngOnInit() {
        this.getPros();
    }
    editProject(pro: projectModel) {
        this.router.navigate(['/edit', pro.Id]);
    }
}
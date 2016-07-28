import {Component} from "@angular/core";
import { ProjectService }   from '../project.service';
import {projectModel} from '../project.model'
@Component({
    templateUrl:"app/project/project-list/project-list.html"
})
export class ProjectListComponent{
    pros: Array<projectModel>;
    pro: projectModel;
    constructor(private proService: ProjectService) {
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
}
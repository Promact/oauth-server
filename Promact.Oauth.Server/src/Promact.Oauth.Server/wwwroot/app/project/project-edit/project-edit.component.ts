import {Component, OnInit} from "@angular/core";
import { ProjectService }   from '../project.service';
import {projectModel} from '../project.model'
import { ROUTER_DIRECTIVES, Router, ActivatedRoute } from '@angular/router';

@Component({
    templateUrl: "app/project/project-edit/project-edit.html",
    directives: []
})
export class ProjectEditComponent implements OnInit {
    pro: projectModel;
    private sub: any;
    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private service: ProjectService) { }

    ngOnInit() { }
}
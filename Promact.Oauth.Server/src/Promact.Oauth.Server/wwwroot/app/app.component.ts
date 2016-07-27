import { Component, OnInit  } from '@angular/core';
import { Pro } from './Pro';
import { ProDetailComponent } from './project-detail.component';
import { ProService } from './project.service';
import {HttpService} from "./http.service";

@Component({
    selector: 'my-app',
    templateUrl: 'app/project.component.html',
    styleUrls: ['app/project.component.css'],
    directives: [ProDetailComponent],
    providers: [ProService, HttpService]
})


export class AppComponent implements OnInit {
    title = 'Tour of Projects';
    pros: Pro[];
    selectedPro: Pro;
    error: any;
    constructor(private proService: ProService) { }
    getPros() {
        this.proService.getPros().subscribe((pros) => {
            this.pros = pros
        }, err => {

        });
    }
    ngOnInit() {
        this.getPros();
    }
    onSelect(pro: Pro) {
        this.selectedPro = pro;
    }
    addProject(projectName: string) {
        let project = new Pro();
        project.name = projectName;
        this.proService.addProject(project).subscribe(() => {
            console.log("Test");
        });
    }
    deleteProject(project: Pro) {
    
    }
}

//import { Component } from "@angular/core";
//@Component({
//    selector: "my-app",
//    template: "<h1>My First Angular 2 App</h1>"
//})
//export class AppComponent { }


//import { Component, OnInit  } from '@angular/core';
//import { Pro } from './Pro';
//import { ProDetailComponent } from './project-detail.component';
//import { ProService } from './project.service';

//@Component({
//    selector: 'my-app',
//    template: `
//    <h1>{{title}}</h1>
//    <h2>My Project</h2>
//    <ul class="pros">
//      <li *ngFor="let pro of pros"
//        [class.selected]="pro === selectedPro"
//        (click)="onSelect(pro)">
//        <span class="badge">{{pro.id}}</span> {{pro.name}}{{pro.description}}
//      </li>
//    </ul>
//    <my-pro-detail [pro]="selectedPro"></my-pro-detail>
//  `,
//    styles: [`
//    .selected {
//      background-color: #CFD8DC !important;
//      color: white;
//    }
//    .pros {
//      margin: 0 0 2em 0;
//      list-style-type: none;
//      padding: 0;
//      width: 15em;
//    }
//    .pros li {
//      cursor: pointer;
//      position: relative;
//      left: 0;
//      background-color: #EEE;
//      margin: .5em;
//      padding: .3em 0;
//      height: 1.6em;
//      border-radius: 4px;
//    }
//    .pros li.selected:hover {
//      background-color: #BBD8DC !important;
//      color: white;
//    }
//    .pros li:hover {
//      color: #607D8B;
//      background-color: #DDD;
//      left: .1em;
//    }
//    .pros .text {
//      position: relative;
//      top: -3px;
//    }
//    .pros .badge {
//      display: inline-block;
//      font-size: small;
//      color: white;
//      padding: 0.8em 0.7em 0 0.7em;
//      background-color: #607D8B;
//      line-height: 1em;
//      position: relative;
//      left: -1px;
//      top: -4px;
//      height: 1.8em;
//      margin-right: .8em;
//      border-radius: 4px 0 0 4px;
//    }
//  `],
//    directives: [ProDetailComponent],
//    providers: [ProService]
//})
////export class AppComponent {
////    title = 'Tour of Heroes';
////    pros = PROS;
////    selectedPro: Pro;
////    onSelect(pro: Pro) { this.selectedPro = pro; }

//export class AppComponent implements OnInit {
//    title = 'Tour of Projects';
//    pros: Pro[];
//    selectedPro: Pro;
//    constructor(private proService: ProService) { }
//    getPros() {
//        this.proService.getPros().then((pros) => {
//            this.pros = pros
//        }, err => {
        
//        });
//    }
//    ngOnInit() {
//        this.getPros();
//    }
//    onSelect(pro: Pro) {
//        this.selectedPro = pro;
//    }
//}

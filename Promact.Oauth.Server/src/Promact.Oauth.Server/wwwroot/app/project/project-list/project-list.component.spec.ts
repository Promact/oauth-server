//import {async,inject,TestBed,ComponentFixture} from '@angular/core/testing';
//import { By } from "@angular/platform-browser";
//import { Provider, DebugElement } from "@angular/core";
//import { ProjectService }   from '../project.service';
//import {projectModel} from '../project.model';
//import { Router } from '@angular/router';
//import {ProjectListComponent} from './project-list.component';
//import {TestConnection} from "../../shared/mocks/test.connection";
//import {MockProjectService} from "../../shared/mocks/project/mock.project.service";
//import {MockBaseService} from '../../shared/mocks/mock.base';
//import { LoginService } from '../../login.service';
//import { ProjectModule } from '../project.module';

//declare var describe, it, beforeEach, expect;
//let comp: ProjectListComponent;
//let fixture: ComponentFixture<ProjectListComponent>;
//let el: DebugElement;


//describe("Project List Test", () => {
//    let projectListComponent: ProjectListComponent;
//    class MockRouter { }
//    class McokLogin { }
//    beforeEach(async(() => {
//        TestBed.configureTestingModule({
//            imports: [ProjectModule],
//            providers: [
//                { provide: Router, useClass: MockRouter },
//                { provide: ProjectService, useClass: MockProjectService },
//                { provide: LoginService, useClass: McokLogin }
//            ]

//        }).compileComponents(); //compile template and css 
//        fixture = TestBed.createComponent(ProjectListComponent);
//        comp = fixture.componentInstance;

//    }));

//    it("should get default Project for company", done => {

//        expect(projectListComponent).toBeDefined();
//    });
//});


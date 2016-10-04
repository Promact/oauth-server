//import {async,inject,TestBed,ComponentFixture} from '@angular/core/testing';
//import { By } from "@angular/platform-browser";
//import { Provider, DebugElement } from "@angular/core";
//import { ProjectService }   from '../project.service';
//import {projectModel} from '../project.model';
//import { Router, RouterModule, Routes} from '@angular/router';
//import {ProjectListComponent} from './project-list.component';
//import {TestConnection} from "../../shared/mocks/test.connection";
//import {MockProjectService} from "../../shared/mocks/project/mock.project.service";
//import {MockBaseService} from '../../shared/mocks/mock.base';
//import { LoginService } from '../../login.service';
//import { ProjectModule } from '../project.module';
//import { Md2Toast } from 'md2/toast/toast';
//import { LoaderService } from '../../shared/loader.service';
//import { RouterLinkStubDirective } from '../../shared/mocks/mock.routerLink';

//declare var describe, it, beforeEach, expect;
//let comp: ProjectListComponent;
//let fixture: ComponentFixture<ProjectListComponent>;
//let el: DebugElement;
//let promise: TestBed;

//describe("Project List Test", () => {
//    let projectListComponent: ProjectListComponent;
//    class MockRouter { }
//    class McokLogin { }
//    class Md2Toast { }
//    class MockLoaderService { }
//    const routes: Routes = [];
//    beforeEach(async(() => {
//        this.promise = TestBed.configureTestingModule({
//            declarations: [RouterLinkStubDirective], //Declaration of mock routerLink used on page.
//            imports: [ProjectModule
//                , RouterModule.forRoot(routes, { useHash: true }) //Set LocationStrategy for component. 
//            ],
//            providers: [
//                { provide: Router, useClass: MockRouter },
//                { provide: ProjectService, useClass: MockProjectService },
//                { provide: Md2Toast, useClass: Md2Toast },
//                { provide: LoginService, useClass: McokLogin },
//                { provide: LoaderService, useClass: MockLoaderService }
//            ]
//        }).compileComponents(); 
//        this.promise.then(() => {
//            this.fixture = TestBed.createComponent(ProjectListComponent); //Create instance of component            
//            this.projectListComponent = this.fixture.componentInstance;
//        });
          
//    }));

 
//    it("should get default Project for company", done => {
//        this.promise.then(() => {
//            //fixture = TestBed.createComponent(ProjectListComponent);
//            //projectListComponent = fixture.componentInstance;
//            expect(projectListComponent).toBeDefined();
//            done();
//            //fixture.detectChanges();
//            //let listOfProject = projectListComponent.getProjects();
//            //expect(projectListComponent.projects.length).toEqual(1);
//        });
//    });
//});


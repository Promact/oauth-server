//import {addProviders, async, inject, TestBed, ComponentFixture, TestComponentBuilder} from '@angular/core/testing';
//import {provide} from "@angular/core";
//import {Location} from "@angular/common";
//import {LocationStrategy} from "@angular/common";
//import {projectModel} from "../project.model";
//import {ProjectViewComponent} from "../project-view/project-view.component";
//import {ProjectService} from "../project.service";
//import {UserModel} from '../../users/user.model';
//import {ROUTER_DIRECTIVES, Router, ActivatedRoute } from '@angular/router';
//import {Md2Toast} from 'md2/toast';
//import {MockToast} from "../../shared/mocks/mock.toast";
//import {Md2Multiselect } from 'md2/multiselect';
//import {TestConnection} from "../../shared/mocks/test.connection";
//import {MockProjectService} from "../../shared/mocks/project/mock.project.service";
//import {MockBaseService} from '../../shared/mocks/mock.base';
//import {MockRouter} from '../../shared/mocks/mock.router';
//import {Observable} from 'rxjs/Observable';
//declare var describe, it, beforeEach, expect;

//describe("Project View Test", () => {
//    let projectViewComponent: ProjectViewComponent;
//    let projeptService: ProjectService;
//    class MockLocation { }
//    class MockActivatedRoute extends ActivatedRoute {
//        constructor() {
//            super();
//            this.params = Observable.of({ id: "1"});
//        }
//    }
    
//    beforeEach(() => {
//        TestBed.configureTestingModule({
//            providers: [
//                provide(ActivatedRoute, { useClass: MockActivatedRoute }),
//                provide(Router, { useClass: MockRouter }),
//                provide(TestConnection, { useClass: TestConnection }),
//                provide(ProjectService, { useClass: MockProjectService }),
//                provide(Md2Toast, { useClass: MockToast }),
//                provide(MockBaseService, { useClass: MockBaseService }),
//                provide(UserModel, { useClass: UserModel }),
//                provide(projectModel, { useClass: projectModel }),
//                provide(Location, { useClass: MockLocation })
//            ]
//        });
//    });
    
//    beforeEach(inject([ActivatedRoute, Router, ProjectService, Location], (route: ActivatedRoute, router: Router, projectService: ProjectService, location: Location) => {
//        projectViewComponent = new ProjectViewComponent(route, router,projectService, location);
//    }));
//    it("should be defined", () => {
//        expect(projectViewComponent).toBeDefined();
//    });
//    it("should get default page for Project", () => {
//        projectViewComponent.ngOnInit();
//        expect(projectViewComponent.Userlist).not.toBeNull();
//        expect(projectViewComponent.project).not.toBeNull();
//    });
    

//});
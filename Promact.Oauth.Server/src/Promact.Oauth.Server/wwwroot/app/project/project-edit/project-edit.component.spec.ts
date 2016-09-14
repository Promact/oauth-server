//declare var describe, it, beforeEach, expect;
//import {addProviders, async, inject, TestBed, ComponentFixture, TestComponentBuilder} from '@angular/core/testing';
//import {provide} from "@angular/core";
//import {Location} from "@angular/common";
//import {LocationStrategy} from "@angular/common";
//import {projectModel} from "../project.model";
//import {ProjectEditComponent} from "../project-edit/project-edit.component";
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

//describe("Project Edit Test", () => {
//    let projectEditComponent: ProjectEditComponent;
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
//                    provide(ActivatedRoute, { useClass: MockActivatedRoute }),
//                    provide(Router, { useClass: MockRouter }),
//                    provide(TestConnection, { useClass: TestConnection }),
//                    provide(ProjectService, { useClass: MockProjectService }),
//                    provide(Md2Toast, { useClass: MockToast }),
//                    provide(MockBaseService, { useClass: MockBaseService }),
//                    provide(UserModel, { useClass: UserModel }),
//                    provide(projectModel, { useClass: projectModel }),
//                    provide(Location, { useClass: MockLocation })
//            ]
//        });
//    });
//    beforeEach(inject([ActivatedRoute, Router, Md2Toast, ProjectService, Location], (route: ActivatedRoute, router: Router, toast: Md2Toast, projectService: ProjectService, location: Location) => {
//        projectEditComponent = new ProjectEditComponent(route, router, toast, projectService, location);
//    }));
//    it("should be defined", () => {
//        expect(projectEditComponent).toBeDefined();
//    });
//    it("should get default page for Project", () => {
//        projectEditComponent.ngOnInit();
//        expect(projectEditComponent.Userlist).not.toBeNull();
//        expect(projectEditComponent.project).not.toBeNull();
//    });
   
//    it("should check Project name and Slack Channel Name before update", inject([projectModel], (projectModel: projectModel) => {
//        let expectedProjecteName = "Test Page2";
//        projectModel.Name = expectedProjecteName;
//        let expectedSlackChannelName = "Test Slack Name";
//        projectModel.SlackChannelName = expectedSlackChannelName;
//        projectEditComponent.editProject(projectModel);
//        expect(projectModel.Name).toBe(expectedProjecteName);
//        expect(projectModel.SlackChannelName).toBe(expectedSlackChannelName);
//    }));

//});
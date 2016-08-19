//import {TestBed, inject} from '@angular/core/testing';
////import {it, describe, expect} from "@angular/core/testing";
//describe('Test', () => {
//    it('Should test matchers', () => {
//        let _undefined, _defined = true;
//        expect("a" + "b").toBe("ab");
//        expect(_undefined).toBeUndefined();
//        expect(_defined).toBeDefined();
//        expect(!_defined).toBeFalsy();
//        expect(_defined).toBeTruthy();
//        expect(null).toBeNull();
//        expect(1 + 1).toEqual(2);
//        expect(5).toBeGreaterThan(4);
//        expect(5).toBeLessThan(6);
//        expect("abcdbca").toContain("bcd");
//        expect([4, 5, 6]).toContain(5);
//        expect("abcdefgh").toMatch(/efg/);
//        expect("abcdbca").not.toContain("xyz");
//        expect("abcdefgh").not.toMatch(/123/);
//    });
//});

//import {addProviders, inject} from '@angular/core/testing';
//import {addProviders, async, inject, TestBed, ComponentFixture} from '@angular/core/testing';
//import {it, describe, expect, beforeEach} from "@angular/core/testing";
//import {provide} from "@angular/core";
//import {projectModel} from "../project.model";
//import {ProjectAddComponent} from "../project-add/project-add.component";
//import { DeprecatedFormsModule } from '@angular/common';
//import {ProjectService} from "../project.service";
//import {UserModel} from '../../users/user.model';
//import { ROUTER_DIRECTIVES, Router, ActivatedRoute } from '@angular/router';
//import {Md2Toast} from 'md2/toast';
//import {MockToast} from "../../shared/mocks/mock.toast";
//import {Md2Multiselect } from 'md2/multiselect';
//import {TestConnection} from "../../shared/mocks/test.connection";
//import {MockProjectService} from "../../shared/mocks/project/mock.project.service";
//import {MockBaseService} from '../../shared/mocks/mock.base';
//import {MockRouter} from '../../shared/mocks/mock.router';


//describe('Project Add Test', () => {
//    let projectAddComponent: ProjectAddComponent;
//    class MockRouter { }
//    class MockActivatedRoute { }

//    beforeEach(() => {
//        TestBed.configureTestingModule({
//            //declarations: [ProjectAddComponent],
//            providers: [
//                { provide: ActivatedRoute, useClass: MockActivatedRoute },
//                {provide:Router,  useClass: MockRouter },
//                {provide:TestConnection,useClass: TestConnection },
//                {provide:ProjectService,useClass: MockProjectService },
//                {provide:Md2Toast, useClass: MockToast },
//                {provide:MockBaseService, useClass: MockBaseService },
//                {provide:UserModel,  useClass: UserModel },
//                { provide: projectModel, useClass: projectModel }

//            ]
//        });

//    });

//    //beforeEach(inject([ActivatedRoute, Router, Md2Toast, ProjectService], (route: ActivatedRoute, router: Router, toast: Md2Toast, projectService: ProjectService) => {
//    //projectAddComponent = new ProjectAddComponent(route, router, toast, projectService);
//    //}));
//    //it('should get customer details', inject([CustomerService], (customerService) => {
//    //    let customerDetails = customerService.printCustomerDetails(1);
//    //    expect(customerDetails).toBe('Customer purchased: Hamburger,Fries');
//    //}));
//    it("should check project name before add", inject([projectModel], (pro: projectModel) => {
//        //projectAddComponent.ngOnInit();
//        let expectedProjectName = "Test Project";
//        pro.Name = expectedProjectName;
//        let expectedSlackChannelName = "Test Slack Name";
//        pro.SlackChannelName = expectedSlackChannelName;
//        projectAddComponent.addProject(pro);
//        expect(pro.Name).toBe(expectedProjectName);
//    }));
//});

//describe("Project Add Test", () => {
//    let projectAddComponent: ProjectAddComponent;
//    class MockRouter { }
//    class MockActivatedRoute { }
//    beforeEach(() => {
//        addProviders([
//            provide(ActivatedRoute, { useClass: MockActivatedRoute }),
//            provide(Router, { useClass: MockRouter }),
//            provide(TestConnection, { useClass: TestConnection }),
//            provide(ProjectService, { useClass: MockProjectService }),
//            provide(Md2Toast, { useClass: MockToast }),
//            provide(MockBaseService, { useClass: MockBaseService }),
//            provide(UserModel, { useClass: UserModel }),
//            provide(projectModel, { useClass: projectModel }),
//        ]);

//    });
//    //beforeEach(inject([ActivatedRoute, Router, Md2Toast, ProjectService], (route: ActivatedRoute, router: Router, toast: Md2Toast, projectService: ProjectService) => {
//    //    projectAddComponent = new ProjectAddComponent(route, router, toast, projectService);
//    //}));
//    it("should check project name before add", inject([projectModel], (projectModel: projectModel) => {
//        let expectedProjectName = "Test Project";
//        projectModel.Name = expectedProjectName;
//        let expectedSlackChannelName = "Test Slack Name";
//        projectModel.SlackChannelName = expectedSlackChannelName;
//        projectAddComponent.addProject(projectModel);
//        expect(projectModel.Name).toBe(expectedProjectName);
//    }));

//});


//describe("Project Add Test", () => {
//    let projectAddComponent: ProjectAddComponent;
//    let projeptService: ProjectService;
//    class MockActivatedRoute { }
//    class MockRouter { }
//    beforeEachProviders(() =>
//        [
//            provide(ActivatedRoute, { useClass: MockActivatedRoute }),
//            provide(Router, { useClass: MockRouter }),
//            provide(TestConnection, { useClass: TestConnection }),
//            provide(ProjectService, { useClass: MockProjectService }),
//            provide(Md2Toast, { useClass: MockToast }),
//            provide(MockBaseService, { useClass: MockBaseService }),
//            provide(UserModel, { useClass: UserModel }),
//            provide(projectModel, { useClass: projectModel }),
//        ]
//    );

    //beforeEach(inject([ActivatedRoute, Router, Md2Toast, ProjectService], (route: ActivatedRoute, router: Router, toast: Md2Toast, projectService: ProjectService) => {
    //    projectAddComponent = new ProjectAddComponent(route, router, toast, projectService);
    //}));

//    //it("should get default project", () => {
//    //    projectAddComponent.ngOnInit();
//    //});
//    it("should check project name before add", inject([projectModel], (projectModel: projectModel) => {
//        let expectedProjectName = "Test Project";
//        projectModel.Name = expectedProjectName;
//        let expectedSlackChannelName = "Test Slack Name";
//        projectModel.SlackChannelName = expectedSlackChannelName;
//        projectAddComponent.addProject(projectModel);
//        expect(projectModel.Name).toBe(expectedProjectName);
//    }));

//});


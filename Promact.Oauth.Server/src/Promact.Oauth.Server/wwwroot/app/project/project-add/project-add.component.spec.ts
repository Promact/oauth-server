import {async, inject, TestBed, ComponentFixture} from '@angular/core/testing';
import {provide} from "@angular/core";
import {projectModel} from "../project.model";
import {ProjectAddComponent} from "../project-add/project-add.component";
import { DeprecatedFormsModule } from '@angular/common';
import {ProjectService} from "../project.service";
import {UserModel} from '../../users/user.model';
import { ROUTER_DIRECTIVES, Router, ActivatedRoute } from '@angular/router';
import {Md2Toast} from 'md2/toast';
import {MockToast} from "../../shared/mocks/mock.toast";
import {Md2Multiselect } from 'md2/multiselect';
import {TestConnection} from "../../shared/mocks/test.connection";
import {MockProjectService} from "../../shared/mocks/project/mock.project.service";
import {MockBaseService} from '../../shared/mocks/mock.base';
import {MockRouter} from '../../shared/mocks/mock.router';

describe("Project Add Test", () => {
    let projectAddComponent: ProjectAddComponent;
    class MockRouter { }
    class MockActivatedRoute { }
    beforeEach(() => {
        TestBed.configureTestingModule({
            declarations: [ProjectAddComponent],
            providers: [
                provide(ActivatedRoute, { useClass: MockActivatedRoute }),
                provide(Router, { useClass: MockRouter }),
                provide(TestConnection, { useClass: TestConnection }),
                provide(ProjectService, { useClass: MockProjectService }),
                provide(Md2Toast, { useClass: MockToast }),
                provide(MockBaseService, { useClass: MockBaseService }),
                provide(UserModel, { useClass: UserModel }),
                provide(projectModel, { useClass: projectModel }),
            ],
            imports: [DeprecatedFormsModule]
        });
    });
        describe('Add Project ', () => {
            beforeEach(async(() => {
                TestBed.compileComponents();
            }));
            it("should check project name before add", inject([projectModel], (projectModel: projectModel) => {
                let expectedProjectName = "Test Project";
                projectModel.Name = expectedProjectName;
                let expectedSlackChannelName = "Test Slack Name";
                projectModel.SlackChannelName = expectedSlackChannelName;
                projectAddComponent.addProject(projectModel);
                expect(projectModel.Name).toBe(expectedProjectName);
            }));

        });

});
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

//    beforeEach(inject([ActivatedRoute, Router, Md2Toast, ProjectService], (route: ActivatedRoute, router: Router, toast: Md2Toast, projectService: ProjectService) => {
//        projectAddComponent = new ProjectAddComponent(route, router, toast, projectService);
//    }));

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


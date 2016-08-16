import {describe, beforeEach, beforeEachProviders, expect, inject, it} from "@angular/core/testing";
import {provide} from "@angular/core";
import {Location} from "@angular/common";
import {LocationStrategy} from "@angular/common";
import {projectModel} from "../project.model";
import {ProjectEditComponent} from "../project-edit/project-edit.component";
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

//import {MockActivatedRoute} from '../../shared/mocks/mock.activatedroute';

describe("Project Add Test", () => {
    let projectEditComponent: ProjectEditComponent;
    let projeptService: ProjectService;

    //class MockActivatedRoute extends ActivatedRoute {
    //    constructor() {
    //        super(null, null, null, null, null);
    //        this.params = Observable.of({ id: "5" });
    //    }
    //}
    class MockActivatedRoute { }
    class MockLocation { }
    beforeEachProviders(() =>
        [
            provide(ActivatedRoute, { useClass: MockActivatedRoute }),
            provide(Router, { useClass: MockRouter }),
            provide(TestConnection, { useClass: TestConnection }),
            provide(ProjectService, { useClass: MockProjectService }),
            provide(Md2Toast, { useClass: MockToast }),
            provide(MockBaseService, { useClass: MockBaseService }),
            provide(UserModel, { useClass: UserModel }),
            provide(projectModel, { useClass: projectModel }),
            provide(Location, { useClass: MockLocation })
            //provide(LocationStrategy, { useClass: LocationStrategy })
        ]
    );

    beforeEach(inject([ActivatedRoute, Router, Md2Toast, ProjectService, Location], (route: ActivatedRoute, router: Router, toast: Md2Toast, projectService: ProjectService, location: Location) => {
        projectEditComponent = new ProjectEditComponent(route, router, toast, projectService, location);
    }));
    //it("should get Project with given projectid to edit", () => {
    //    projectEditComponent.ngOnInit();
        
        
    //});
    //it("should check project name before add", inject([projectModel], (projectModel: projectModel) => {
    //    let expectedProjectName = "Test Project";
    //    projectModel.Name = expectedProjectName;
    //    let expectedSlackChannelName = "Test Slack Name";
    //    projectModel.SlackChannelName = expectedSlackChannelName;
    //    projectEditComponent.addProject(projectModel);
    //    expect(projectModel.Name).toBe(expectedProjectName);
    //}));
    it("should check Project name and Slack Channel Name before update", inject([projectModel], (projectModel: projectModel) => {
        let expectedProjecteName = "Test Page2";
        projectModel.Name = expectedProjecteName;
        let expectedSlackChannelName = "Test Slack Name";
        projectModel.SlackChannelName = expectedSlackChannelName;
        projectEditComponent.editProject(projectModel);
        expect(projectModel.Name).toBe(expectedProjecteName);
        expect(projectModel.SlackChannelName).toBe(expectedSlackChannelName);
    }));

});
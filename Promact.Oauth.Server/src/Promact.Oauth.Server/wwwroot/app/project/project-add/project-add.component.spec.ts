import {async, inject, TestBed, ComponentFixture, TestComponentBuilder, addProviders} from '@angular/core/testing';
import {provide} from "@angular/core";
import {projectModel} from "../project.model";
import {ProjectAddComponent} from "../project-add/project-add.component";
import { DeprecatedFormsModule } from '@angular/common';
import {ProjectService} from "../project.service";
import {UserModel} from '../../users/user.model';
import { ROUTER_DIRECTIVES, Router} from '@angular/router';
import { ActivatedRoute} from '@angular/router';
import {Md2Toast} from 'md2/toast';
import {MockToast} from "../../shared/mocks/mock.toast";
import {Md2Multiselect } from 'md2/multiselect';
import {TestConnection} from "../../shared/mocks/test.connection";
import {MockProjectService} from "../../shared/mocks/project/mock.project.service";
import {MockBaseService} from '../../shared/mocks/mock.base';
import {MockRouter} from '../../shared/mocks/mock.router';
import {Observable} from 'rxjs/Observable';
//import {MockActivatedRoute} from '../../shared/mocks/mock.activatedroute';
declare var describe, it, beforeEach, expect;

describe('Project Add Test', () => {
    let projectAddComponent: ProjectAddComponent;
    class MockRouter { }
    //class MockActivatedRoute { }
    class MockActivatedRoute extends ActivatedRoute {
        constructor() {
            super();
            this.params = Observable.of({ });
        }
    }
    
    beforeEach(() => {
        TestBed.configureTestingModule({
            providers: [
                { provide: ActivatedRoute, useClass: MockActivatedRoute },
                { provide: Router, useClass: MockRouter },
                { provide: TestConnection, useClass: TestConnection },
                { provide: ProjectService, useClass: MockProjectService },
                { provide: Md2Toast, useClass: MockToast },
                { provide: MockBaseService, useClass: MockBaseService },
                { provide: UserModel, useClass: UserModel },
                { provide: projectModel, useClass: projectModel }

            ]
        });

    });
    beforeEach(inject([ActivatedRoute, Router, Md2Toast, ProjectService], (route: ActivatedRoute, router: Router, toast: Md2Toast, projectService: ProjectService) => {
        projectAddComponent = new ProjectAddComponent(route, router, toast, projectService);
    }));
    
    it("should get default page for Project", () => {
        projectAddComponent.ngOnInit();
        expect(projectAddComponent.Userlist).not.toBeNull();
    });

    
    it("should check project name before add", inject([projectModel], (projectModel: projectModel) => {
        let expectedProjectName = "Tests Projects";
        projectModel.Name = expectedProjectName;
        let expectedSlackChannelName = "Test Slack Name";
        projectModel.SlackChannelName = expectedSlackChannelName;
        projectAddComponent.addProject(projectModel);
        expect(projectModel.Name).toBe(expectedProjectName);
    }));

   
});    







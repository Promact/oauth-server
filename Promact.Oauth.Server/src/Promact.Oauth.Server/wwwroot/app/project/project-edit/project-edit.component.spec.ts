declare var describe, it, beforeEach, expect, spyOn;
import { async, TestBed, fakeAsync, tick} from '@angular/core/testing';
import { ProjectModel } from "../project.model";
import { ProjectEditComponent } from "../project-edit/project-edit.component";
import { ProjectService } from "../project.service";
import { UserModel } from '../../users/user.model';
import { ActivatedRoute, RouterModule, Routes, Router} from '@angular/router';
import { Md2Toast } from 'md2';
import { MockToast } from "../../shared/mocks/mock.toast";
import { MockProjectService } from "../../shared/mocks/project/mock.project.service";
import { ProjectModule } from '../project.module';
import { LoaderService } from '../../shared/loader.service';
import { StringConstant } from '../../shared/stringconstant';
import { ActivatedRouteStub } from "../../shared/mocks/mock.activatedroute";
import { MockRouter } from '../../shared/mocks/mock.router';
import { Location } from "@angular/common";
import { MockLocation } from '../../shared/mocks/mock.location';
let stringConstant = new StringConstant();

let mockUser = new UserModel();
mockUser.FirstName = stringConstant.firstName;
mockUser.LastName = stringConstant.lastName;
mockUser.Email = stringConstant.email;
mockUser.IsActive = true;
mockUser.Id = stringConstant.id;
let mockList = new Array<UserModel>();
mockList.push(mockUser);

describe('Project Edit Test', () => {

    const routes: Routes = [];
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            imports: [ProjectModule, RouterModule.forRoot(routes, { useHash: true }) //Set LocationStrategy for component. 
            ],
            providers: [
                { provide: ActivatedRoute, useClass: ActivatedRouteStub },
                { provide: ProjectService, useClass: MockProjectService },
                { provide: Md2Toast, useClass: MockToast },
                { provide: UserModel, useClass: UserModel },
                { provide: ProjectModel, useClass: ProjectModel },
                { provide: LoaderService, useClass: LoaderService },
                { provide: StringConstant, useClass: StringConstant },
                { provide: Router, useClass: MockRouter },
                { provide: Location, useClass: MockLocation },
            ]
        }).compileComponents();
    }));

    it("should be defined ProjectEditComponent", () => {
        let fixture = TestBed.createComponent(ProjectEditComponent);
        let projectEditComponent = fixture.componentInstance;
        expect(projectEditComponent).toBeDefined();
    });

    
    it("should get selected Project", fakeAsync(() => {
        let fixture = TestBed.createComponent(ProjectEditComponent); //Create instance of component            
        let activatedRoute = fixture.debugElement.injector.get(ActivatedRoute);
        activatedRoute.testParams = { id: stringConstant.id };
        let projectEditComponent = fixture.componentInstance;
        projectEditComponent.ngOnInit();
        tick();
        expect(projectEditComponent.Userlist).not.toBeNull();
    }));

    it("should get erro on selected Project Id", fakeAsync(() => {
        let fixture = TestBed.createComponent(ProjectEditComponent); //Create instance of component            
        let projectEditComponent = fixture.componentInstance;
        let activatedRoute = fixture.debugElement.injector.get(ActivatedRoute);
        activatedRoute.testParams = { id: stringConstant.id };
        let projectService = fixture.debugElement.injector.get(ProjectService);
        spyOn(projectService, "getProject").and.returnValue(Promise.reject(""));
        projectEditComponent.ngOnInit();
        tick();
        expect(projectEditComponent.Userlist).not.toBeNull();
    }));

    it("should get error on selected Project", fakeAsync(() => {
        let fixture = TestBed.createComponent(ProjectEditComponent); //Create instance of component            
        let projectEditComponent = fixture.componentInstance;
        let activatedRoute = fixture.debugElement.injector.get(ActivatedRoute);
        activatedRoute.testParams = { id: stringConstant.id };
        let projectModel = new ProjectModel();
        projectModel.Name = "test";
        projectModel.ApplicationUsers = mockList;
        let projectService = fixture.debugElement.injector.get(ProjectService);
        spyOn(projectService, "editProject").and.returnValue(Promise.reject(""));
        projectEditComponent.editProject(projectModel);
        tick();
        expect(projectModel).not.toBeNull();
    }));

    it("should get selected Project", fakeAsync(() => {
        let fixture = TestBed.createComponent(ProjectEditComponent); //Create instance of component            
        let projectEditComponent = fixture.componentInstance;
        let activatedRoute = fixture.debugElement.injector.get(ActivatedRoute);
        activatedRoute.testParams = { id: stringConstant.id };
        let projectService = fixture.debugElement.injector.get(ProjectService);
        spyOn(projectService, "getProject").and.returnValue(Promise.reject(""));
        projectEditComponent.ngOnInit();
        tick();
        expect(projectEditComponent.Userlist).not.toBeNull();
    }));


    it("should check Project name and Slack Channel Name before update", fakeAsync(() => {
        let fixture = TestBed.createComponent(ProjectEditComponent); //Create instance of component            
        let projectEditComponent = fixture.componentInstance;
        let projectModels = new ProjectModel();
        let expectedProjecteName = stringConstant.projectName;
        projectModels.Name = expectedProjecteName;
        let expectedSlackChannelName = stringConstant.slackChannelName;
        projectModels.SlackChannelName = expectedSlackChannelName;
        projectModels.ApplicationUsers = mockList;
        projectModels.TeamLeaderId = stringConstant.teamLeaderId;
        projectEditComponent.editProject(projectModels);
        tick();
        expect(projectModels.Name).toBe(expectedProjecteName);
    }));

    it("should check Project name before update", fakeAsync(() => {
        let fixture = TestBed.createComponent(ProjectEditComponent); //Create instance of component            
        let projectEditComponent = fixture.componentInstance;
        let projectModels = new ProjectModel();
        let expectedProjecteName = null;
        projectModels.Name = expectedProjecteName;
        let expectedSlackChannelName = stringConstant.slackChannelName;
        projectModels.SlackChannelName = expectedSlackChannelName;
        projectModels.ApplicationUsers = mockList;
        projectModels.TeamLeaderId = stringConstant.teamLeaderId;
        projectEditComponent.editProject(projectModels);
        tick();
        expect(projectModels.Name).toBe(null);
    }));

    it("should check Teamleader is selected as team member ", fakeAsync(() => {
        let fixture = TestBed.createComponent(ProjectEditComponent); //Create instance of component            
        let projectEditComponent = fixture.componentInstance;
        let projectModels = new ProjectModel();
        let expectedProjecteName = null;
        projectModels.Name = stringConstant.projectName;
        let expectedSlackChannelName = stringConstant.slackChannelName;
        projectModels.SlackChannelName = expectedSlackChannelName;
        projectModels.ApplicationUsers = mockList;
        projectModels.TeamLeaderId = stringConstant.id;
        projectEditComponent.editProject(projectModels);
        tick();
        expect(projectModels.Name).not.toBeNull();
    }));

        it("should be check Slcak Channel Name is not duplicate ", fakeAsync(() => {
        let fixture = TestBed.createComponent(ProjectEditComponent); //Create instance of component            
        let projectEditComponent = fixture.componentInstance;
        let toast = fixture.debugElement.injector.get(Md2Toast);
        let expectedProjectName = stringConstant.projectName;
        let projectModels = new ProjectModel();
        projectModels.Name = expectedProjectName;
        projectModels.SlackChannelName = stringConstant.slackChannelName;
        projectModels.ApplicationUsers = mockList;
        projectModels.TeamLeaderId = stringConstant.teamLeaderId;
        let projectModel = new ProjectModel();
        projectModel.Name = null;
        projectModel.SlackChannelName = stringConstant.slackChannelName;
        let projectService = fixture.debugElement.injector.get(ProjectService);
        spyOn(projectService, "editProject").and.returnValue((Promise.resolve(projectModel)));
        projectEditComponent.editProject(projectModels);
        tick();
        expect(projectModels.SlackChannelName).not.toBeNull();
    }));


        it("should be check Project Name is not duplicate ", fakeAsync(() => {
            let fixture = TestBed.createComponent(ProjectEditComponent); //Create instance of component            
            let projectEditComponent = fixture.componentInstance;
            let toast = fixture.debugElement.injector.get(Md2Toast);
            let expectedProjectName = stringConstant.projectName;
            let projectModels = new ProjectModel();
            projectModels.Name = expectedProjectName;
            projectModels.SlackChannelName = stringConstant.slackChannelName;
            projectModels.ApplicationUsers = mockList;
            projectModels.TeamLeaderId = stringConstant.teamLeaderId;
            let projectModel = new ProjectModel();
            projectModel.Name = expectedProjectName;
            projectModel.SlackChannelName = null;
            let projectService = fixture.debugElement.injector.get(ProjectService);
            spyOn(projectService, "editProject").and.returnValue((Promise.resolve(projectModel)));
            projectEditComponent.editProject(projectModels);
            tick();
            expect(projectModels.SlackChannelName).not.toBeNull();
        }));

        it("should be check Project Name and slack channel is duplicate ", fakeAsync(() => {
            let fixture = TestBed.createComponent(ProjectEditComponent); //Create instance of component            
            let projectEditComponent = fixture.componentInstance;
            let toast = fixture.debugElement.injector.get(Md2Toast);
            let expectedProjectName = stringConstant.projectName;
            let projectModels = new ProjectModel();
            projectModels.Name = expectedProjectName;
            projectModels.SlackChannelName = stringConstant.slackChannelName;
            projectModels.ApplicationUsers = mockList;
            projectModels.TeamLeaderId = stringConstant.teamLeaderId;
            let projectModel = new ProjectModel();
            projectModel.Name = null;
            projectModel.SlackChannelName = null;
            let projectService = fixture.debugElement.injector.get(ProjectService);
            spyOn(projectService, "editProject").and.returnValue((Promise.resolve(projectModel)));
            projectEditComponent.editProject(projectModels);
            tick();
            expect(projectModels.SlackChannelName).not.toBeNull();
        }));

        it("should be check Project Name and slack channel is not duplicate ", fakeAsync(() => {
            let fixture = TestBed.createComponent(ProjectEditComponent); //Create instance of component            
            let projectEditComponent = fixture.componentInstance;
            let toast = fixture.debugElement.injector.get(Md2Toast);
            let expectedProjectName = stringConstant.projectName;
            let projectModels = new ProjectModel();
            projectModels.Name = expectedProjectName;
            projectModels.SlackChannelName = stringConstant.slackChannelName;
            projectModels.ApplicationUsers = mockList;
            projectModels.TeamLeaderId = stringConstant.teamLeaderId;
            let projectModel = new ProjectModel();
            projectModel.Name = expectedProjectName;
            projectModel.SlackChannelName = stringConstant.slackChannelName;
            let projectService = fixture.debugElement.injector.get(ProjectService);
            let router = fixture.debugElement.injector.get(Router);
            spyOn(projectService, "editProject").and.returnValue((Promise.resolve(projectModel)));
            spyOn(router, "navigate");
            projectEditComponent.editProject(projectModels);
            tick();
            expect(projectModels.SlackChannelName).not.toBeNull();
        }));

        it('should be rediration to project list', fakeAsync(() => {
            let fixture = TestBed.createComponent(ProjectEditComponent);
            let projectEditComponent = fixture.componentInstance;
            let location = fixture.debugElement.injector.get(Location);
            spyOn(location, "back");
            projectEditComponent.gotoProjects();
            tick();
            expect(location.back).toHaveBeenCalled();
        }));


});




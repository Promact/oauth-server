declare let describe, it, beforeEach, expect, spyOn;
import { async, TestBed , fakeAsync,tick } from '@angular/core/testing';
import { ProjectModel } from "../project.model";
import { ProjectAddComponent } from "../project-add/project-add.component";
import { ProjectService } from "../project.service";
import { UserModel } from '../../users/user.model';
import { ActivatedRoute, RouterModule, Routes, Router} from '@angular/router';
import { Md2Toast } from 'md2';
import { MockToast } from "../../shared/mocks/mock.toast";
import { MockProjectService } from "../../shared/mocks/project/mock.project.service";
import { ProjectModule } from '../project.module';
import { LoaderService } from '../../shared/loader.service';
import { ActivatedRouteStub } from "../../shared/mocks/mock.activatedroute";
import { StringConstant } from '../../shared/stringconstant';
import { MockRouter } from '../../shared/mocks/mock.router';

let stringConstant = new StringConstant();

let mockUser = new UserModel();
mockUser.FirstName = stringConstant.firstName;
mockUser.LastName = stringConstant.lastName;
mockUser.Email = stringConstant.email;
mockUser.IsActive = true;
mockUser.Id = stringConstant.id;
let mockList = new Array<UserModel>();
mockList.push(mockUser);

describe('Project Add Test', () => {
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
                { provide: Router, useClass: MockRouter },
            ]
        }).compileComponents();

    }));

    it("should be defined ProjectAddComponent", () => {
        let fixture = TestBed.createComponent(ProjectAddComponent);
        let projectAddComponent = fixture.componentInstance;
        expect(projectAddComponent).toBeDefined();
    });

    it("should get user list for project", fakeAsync(() => {
        let fixture = TestBed.createComponent(ProjectAddComponent); //Create instance of component            
        let projectAddComponent = fixture.componentInstance;
        let toast = fixture.debugElement.injector.get(Md2Toast);
        projectAddComponent.ngOnInit();
        tick();
        expect(projectAddComponent.Userlist).not.toBeNull();
    }));

    it("should be teammembers and teamleader are not same", fakeAsync(() => {
        let fixture = TestBed.createComponent(ProjectAddComponent); //Create instance of component            
        let projectAddComponent = fixture.componentInstance;
        let toast = fixture.debugElement.injector.get(Md2Toast);
        let expectedProjectName = stringConstant.projectName;
        let projectModels = new ProjectModel();
        projectModels.Name = expectedProjectName;
        let expectedSlackChannelName = stringConstant.slackChannelName;
        projectModels.SlackChannelName = expectedSlackChannelName;
        projectModels.ApplicationUsers = mockList;
        projectModels.TeamLeaderId = stringConstant.id;
        projectAddComponent.addProject(projectModels);
        tick();
        expect(projectModels.Name).toBe(expectedProjectName);
    }));

    it("should be check project name before Added", fakeAsync(() => {
        let fixture = TestBed.createComponent(ProjectAddComponent); //Create instance of component            
        let projectAddComponent = fixture.componentInstance;
        let toast = fixture.debugElement.injector.get(Md2Toast);
        let expectedProjectName = null;
        let projectModels = new ProjectModel();
        projectModels.Name = expectedProjectName;
        let expectedSlackChannelName = stringConstant.slackChannelName;
        projectModels.SlackChannelName = expectedSlackChannelName;
        projectModels.ApplicationUsers = mockList;
        projectModels.TeamLeaderId = stringConstant.teamLeaderId;
        projectAddComponent.addProject(projectModels);
        tick();
        expect(projectModels.Name).toBe(null);
    }));

    it("should be check Slack Channel Name before Added", fakeAsync(() => {
        let fixture = TestBed.createComponent(ProjectAddComponent); //Create instance of component            
        let projectAddComponent = fixture.componentInstance;
        let toast = fixture.debugElement.injector.get(Md2Toast);
        let expectedProjectName = stringConstant.projectName;
        let projectModels = new ProjectModel();
        projectModels.Name = expectedProjectName;
        let expectedSlackChannelName = null;
        projectModels.SlackChannelName = expectedSlackChannelName;
        projectModels.ApplicationUsers = mockList;
        projectModels.TeamLeaderId = stringConstant.teamLeaderId;
        projectAddComponent.addProject(projectModels);
        tick();
        expect(projectModels.SlackChannelName).toBe(null);
    }));

    it("should be check Slack Channel Name before Added", fakeAsync(() => {
        let fixture = TestBed.createComponent(ProjectAddComponent); //Create instance of component            
        let projectAddComponent = fixture.componentInstance;
        let toast = fixture.debugElement.injector.get(Md2Toast);
        let expectedProjectName = stringConstant.projectName;
        let projectModels = new ProjectModel();
        projectModels.Name = expectedProjectName;
        let expectedSlackChannelName = null;
        projectModels.SlackChannelName = expectedSlackChannelName;
        projectModels.ApplicationUsers = mockList;
        projectModels.TeamLeaderId = stringConstant.teamLeaderId;
        projectAddComponent.addProject(projectModels);
        tick();
        expect(projectModels.SlackChannelName).toBe(null);
    }));


    it("should be check Slack Channel Name and Project Name before Added", fakeAsync(() => {
        let fixture = TestBed.createComponent(ProjectAddComponent); //Create instance of component            
        let projectAddComponent = fixture.componentInstance;
        let toast = fixture.debugElement.injector.get(Md2Toast);
        let expectedProjectName = stringConstant.projectName;
        let projectModels = new ProjectModel();
        projectModels.Name = null;
        let expectedSlackChannelName = null;
        projectModels.SlackChannelName = expectedSlackChannelName;
        projectModels.ApplicationUsers = mockList;
        projectModels.TeamLeaderId = stringConstant.teamLeaderId;
        projectAddComponent.addProject(projectModels);
        tick();
        expect(projectModels.SlackChannelName).toBe(null);
    }));

    it("should be check Slack Channel Name and Project Name is duplicate ", fakeAsync(() => {
        let fixture = TestBed.createComponent(ProjectAddComponent); //Create instance of component            
        let projectAddComponent = fixture.componentInstance;
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
        spyOn(projectService, "addProject").and.returnValue((Promise.resolve(projectModel)));
        projectAddComponent.addProject(projectModels);
        tick();
        expect(projectAddComponent.Userlist).not.toBeNull();
    }));

    it("should be check Project Name is duplicate ", fakeAsync(() => {
        let fixture = TestBed.createComponent(ProjectAddComponent); //Create instance of component            
        let projectAddComponent = fixture.componentInstance;
        let toast = fixture.debugElement.injector.get(Md2Toast);
        let expectedProjectName = stringConstant.projectName;
        let projectModels = new ProjectModel();
        projectModels.Name = expectedProjectName;
        let expectedSlackChannelName = null;
        projectModels.SlackChannelName = stringConstant.slackChannelName;
        projectModels.ApplicationUsers = mockList;
        projectModels.TeamLeaderId = stringConstant.teamLeaderId;
        let projectModel = new ProjectModel();
        projectModel.Name = null;
        projectModel.SlackChannelName = stringConstant.slackChannelName;
        let projectService = fixture.debugElement.injector.get(ProjectService);
        spyOn(projectService, "addProject").and.returnValue((Promise.resolve(projectModel)));
        projectAddComponent.addProject(projectModels);
        tick();
        expect(projectAddComponent.Userlist).not.toBeNull();
    }));

    it("should be check Slcak Channel Name is duplicate ", fakeAsync(() => {
        let fixture = TestBed.createComponent(ProjectAddComponent); //Create instance of component            
        let projectAddComponent = fixture.componentInstance;
        let toast = fixture.debugElement.injector.get(Md2Toast);
        let expectedProjectName = stringConstant.projectName;
        let projectModels = new ProjectModel();
        projectModels.Name = expectedProjectName;
        let expectedSlackChannelName = null;
        projectModels.SlackChannelName = stringConstant.slackChannelName;
        projectModels.ApplicationUsers = mockList;
        projectModels.TeamLeaderId = stringConstant.teamLeaderId;
        let projectModel = new ProjectModel();
        projectModel.Name = stringConstant.projectName;
        projectModel.SlackChannelName = null;
        let projectService = fixture.debugElement.injector.get(ProjectService);
        spyOn(projectService, "addProject").and.returnValue((Promise.resolve(projectModel)));
        projectAddComponent.addProject(projectModels);
        tick();
        expect(projectAddComponent.Userlist).not.toBeNull();
    }));

    it("should be check Slcak Channel Name is not duplicate ", fakeAsync(() => {
        let fixture = TestBed.createComponent(ProjectAddComponent); //Create instance of component            
        let projectAddComponent = fixture.componentInstance;
        let toast = fixture.debugElement.injector.get(Md2Toast);
        let expectedProjectName = stringConstant.projectName;
        let projectModels = new ProjectModel();
        projectModels.Name = expectedProjectName;
        let expectedSlackChannelName = null;
        projectModels.SlackChannelName = stringConstant.slackChannelName;
        projectModels.ApplicationUsers = mockList;
        projectModels.TeamLeaderId = stringConstant.teamLeaderId;
        let projectModel = new ProjectModel();
        projectModel.Name = stringConstant.projectName;
        projectModel.SlackChannelName = stringConstant.slackChannelName;
        let projectService = fixture.debugElement.injector.get(ProjectService);
        spyOn(projectService, "addProject").and.returnValue((Promise.resolve(projectModel)));
        projectAddComponent.addProject(projectModels);
        tick();
        expect(projectModels.Name).not.toBeNull();
    }));

    it("should be check Slcak Channel Name is not duplicate ", fakeAsync(() => {
        let fixture = TestBed.createComponent(ProjectAddComponent); //Create instance of component            
        let projectAddComponent = fixture.componentInstance;
        let toast = fixture.debugElement.injector.get(Md2Toast);
        let expectedProjectName = stringConstant.projectName;
        let projectModels = new ProjectModel();
        projectModels.Name = expectedProjectName;
        let expectedSlackChannelName = null;
        projectModels.SlackChannelName = stringConstant.slackChannelName;
        projectModels.ApplicationUsers = mockList;
        projectModels.TeamLeaderId = stringConstant.teamLeaderId;
        let projectModel = new ProjectModel();
        projectModel.Name = stringConstant.projectName;
        projectModel.SlackChannelName = stringConstant.slackChannelName;
        let projectService = fixture.debugElement.injector.get(ProjectService);
        spyOn(projectService, "addProject").and.returnValue((Promise.resolve(stringConstant.testString)));
        projectAddComponent.addProject(projectModels);
        tick();
        expect(projectModels.Name).not.toBeNull();
    }));


    it('should be rediration to project list', fakeAsync(() => {
        let fixture = TestBed.createComponent(ProjectAddComponent);
        let projectAddComponent = fixture.componentInstance;
        let router = fixture.debugElement.injector.get(Router);
        spyOn(router, "navigate");
        projectAddComponent.gotoProjects();
        tick();
        expect(router.navigate).toHaveBeenCalled();
    }));

    it("should get error on add Project", fakeAsync(() => {
        let fixture = TestBed.createComponent(ProjectAddComponent); //Create instance of component            
        let projectAddComponent = fixture.componentInstance;
        let activatedRoute = fixture.debugElement.injector.get(ActivatedRoute);
        activatedRoute.testParams = { id: stringConstant.id };
        let projectModel = new ProjectModel();
        projectModel.Name = "test";
        projectModel.ApplicationUsers = mockList;
        let projectService = fixture.debugElement.injector.get(ProjectService);
        spyOn(projectService, "addProject").and.returnValue(Promise.reject(""));
        projectAddComponent.addProject(projectModel);
        tick();
        expect(projectModel).not.toBeNull();
    }));
}); 







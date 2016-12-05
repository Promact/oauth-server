declare let describe, it, beforeEach, expect;
import { async, inject, TestBed, ComponentFixture } from '@angular/core/testing';
import { Provider } from "@angular/core";
import { ProjectModel } from "../project.model";
import { ProjectAddComponent } from "../project-add/project-add.component";
import { ProjectService } from "../project.service";
import { UserModel } from '../../users/user.model';
import { Router, ActivatedRoute, RouterModule, Routes } from '@angular/router';
import { Md2Toast } from 'md2';
import { MockToast } from "../../shared/mocks/mock.toast";
import { Md2Multiselect } from 'md2/multiselect';
import { MockProjectService } from "../../shared/mocks/project/mock.project.service";
import { MockRouter } from '../../shared/mocks/mock.router';
import { Observable } from 'rxjs/Observable';
import { ProjectModule } from '../project.module';
import { LoaderService } from '../../shared/loader.service';
import { StringConstant } from '../../shared/stringconstant';
import { ActivatedRouteStub } from "../../shared/mocks/mock.activatedroute";


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
    class MockActivatedRoute extends ActivatedRoute {
        constructor() {
            super();
            this.params = Observable.of({});
        }
    }

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            imports: [ProjectModule, RouterModule.forRoot(routes, { useHash: true }) //Set LocationStrategy for component. 
            ],
            providers: [
                { provide: ActivatedRoute, useClass: ActivatedRouteStub },
                { provide: Router, useClass: MockRouter },
                { provide: ProjectService, useClass: MockProjectService },
                { provide: Md2Toast, useClass: MockToast },
                { provide: UserModel, useClass: UserModel },
                { provide: ProjectModel, useClass: ProjectModel },
                { provide: LoaderService, useClass: LoaderService },
                { provide: StringConstant, useClass: StringConstant }
            ]
        }).compileComponents();

    }));

    it("should get user list for project", () => {
        let fixture = TestBed.createComponent(ProjectAddComponent); //Create instance of component            
        let projectAddComponent = fixture.componentInstance;
        let toast = fixture.debugElement.injector.get(Md2Toast);
        projectAddComponent.ngOnInit();
        expect(projectAddComponent.Userlist).not.toBeNull();
    });

    it("should be add new project", () => {
        let fixture = TestBed.createComponent(ProjectAddComponent); //Create instance of component            
        let projectAddComponent = fixture.componentInstance;
        let toast = fixture.debugElement.injector.get(Md2Toast);
        let expectedProjectName = "Tests Projects";
        let projectModels = new ProjectModel();
        projectModels.name = expectedProjectName;
        let expectedSlackChannelName = "Test Slack Name";
        projectModels.slackChannelName = expectedSlackChannelName;
        let mockUser = new UserModel();
        mockUser.FirstName = "Ronak";
        mockUser.LastName = "Shah";
        mockUser.Email = "rshah@Promactinfo.com";
        mockUser.IsActive = true;
        mockUser.Id = "1";
        let mockList = new Array<UserModel>();
        mockList.push(mockUser);
        projectModels.applicationUsers = mockList;
        projectModels.teamLeaderId = "2";
        projectAddComponent.addProject(projectModels);
        expect(projectModels.name).toBe(expectedProjectName);
    });


    it("should be check project name before Added", () => {
        let fixture = TestBed.createComponent(ProjectAddComponent); //Create instance of component            
        let projectAddComponent = fixture.componentInstance;
        let toast = fixture.debugElement.injector.get(Md2Toast);
        let expectedProjectName = null;
        let projectModels = new ProjectModel();
        projectModels.name = expectedProjectName;
        let expectedSlackChannelName = "Test Slack Name";
        projectModels.slackChannelName = expectedSlackChannelName;
        let mockUser = new UserModel();
        mockUser.FirstName = "Ronak";
        mockUser.LastName = "Shah";
        mockUser.Email = "rshah@Promactinfo.com";
        mockUser.IsActive = true;
        mockUser.Id = "1";
        let mockList = new Array<UserModel>();
        mockList.push(mockUser);
        projectModels.applicationUsers = mockList;
        projectModels.teamLeaderId = "2";
        projectAddComponent.addProject(projectModels);
        expect(projectModels.name).toBe(null);
    });

    it("should be check Slack Channel Name before Added", () => {
        let fixture = TestBed.createComponent(ProjectAddComponent); //Create instance of component            
        let projectAddComponent = fixture.componentInstance;
        let toast = fixture.debugElement.injector.get(Md2Toast);
        let expectedProjectName = "test project";
        let projectModels = new ProjectModel();
        projectModels.name = expectedProjectName;
        let expectedSlackChannelName = null;
        projectModels.slackChannelName = expectedSlackChannelName;
        let mockUser = new UserModel();
        mockUser.FirstName = "Ronak";
        mockUser.LastName = "Shah";
        mockUser.Email = "rshah@Promactinfo.com";
        mockUser.IsActive = true;
        mockUser.Id = "1";
        let mockList = new Array<UserModel>();
        mockList.push(mockUser);
        projectModels.applicationUsers = mockList;
        projectModels.teamLeaderId = "2";
        projectAddComponent.addProject(projectModels);
        expect(projectModels.slackChannelName).toBe(null);
    });
});







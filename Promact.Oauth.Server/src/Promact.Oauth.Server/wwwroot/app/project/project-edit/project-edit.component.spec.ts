declare var describe, it, beforeEach, expect;
import { async, TestBed } from '@angular/core/testing';
import { ProjectModel } from "../project.model";
import { ProjectEditComponent } from "../project-edit/project-edit.component";
import { ProjectService } from "../project.service";
import { UserModel } from '../../users/user.model';
import { ActivatedRoute, RouterModule, Routes } from '@angular/router';
import { Md2Toast } from 'md2';
import { MockToast } from "../../shared/mocks/mock.toast";
import { MockProjectService } from "../../shared/mocks/project/mock.project.service";
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
                { provide: StringConstant, useClass: StringConstant }
            ]
        }).compileComponents();
    }));

    it("should get selected Project", () => {
        let fixture = TestBed.createComponent(ProjectEditComponent); //Create instance of component            
        let activatedRoute = fixture.debugElement.injector.get(ActivatedRoute);
        activatedRoute.testParams = { id: stringConstant.id };
        let projectEditComponent = fixture.componentInstance;
        projectEditComponent.ngOnInit();
        expect(projectEditComponent.Userlist).not.toBeNull();
    });

    it("should check Project name and Slack Channel Name before update", () => {
            let fixture = TestBed.createComponent(ProjectEditComponent); //Create instance of component            
            let projectEditComponent = fixture.componentInstance;
            let projectModels = new ProjectModel();
            let expectedProjecteName = stringConstant.projectName;
            projectModels.name = expectedProjecteName;
            let expectedSlackChannelName = stringConstant.slackChannelName;
            projectModels.slackChannelName = expectedSlackChannelName;
            projectModels.applicationUsers = mockList;
            projectModels.teamLeaderId = stringConstant.teamLeaderId;
            projectEditComponent.editProject(projectModels);
            expect(projectModels.name).toBe(expectedProjecteName);
    });
    
    it("should check Project name before update", () => {
            let fixture = TestBed.createComponent(ProjectEditComponent); //Create instance of component            
            let projectEditComponent = fixture.componentInstance;
            let projectModels = new ProjectModel();
            let expectedProjecteName = null;
            projectModels.name = expectedProjecteName;
            let expectedSlackChannelName = stringConstant.slackChannelName;
            projectModels.slackChannelName = expectedSlackChannelName;
            projectModels.applicationUsers = mockList;
            projectModels.teamLeaderId = stringConstant.teamLeaderId;
            projectEditComponent.editProject(projectModels);
            expect(projectModels.name).toBe(null);
    });
    
});    




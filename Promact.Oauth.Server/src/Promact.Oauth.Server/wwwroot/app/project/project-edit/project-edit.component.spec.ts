declare var describe, it, beforeEach, expect;
import { async, inject, TestBed, ComponentFixture } from '@angular/core/testing';
import { Provider } from "@angular/core";
import { ProjectModel } from "../project.model";
import { ProjectEditComponent } from "../project-edit/project-edit.component";
import { ProjectService } from "../project.service";
import { UserModel } from '../../users/user.model';
import { Router, ActivatedRoute, RouterModule, Routes } from '@angular/router';
import { Md2Toast } from 'md2';
import { MockToast } from "../../shared/mocks/mock.toast";
import { Md2Multiselect } from 'md2/multiselect';
import { MockProjectService } from "../../shared/mocks/project/mock.project.service";
import { MockRouter } from '../../shared/mocks/mock.router';
import { Observable } from 'rxjs/Observable';
import { RouterLinkStubDirective } from '../../shared/mocks/mock.routerLink';
import { ProjectModule } from '../project.module';
import { LoaderService } from '../../shared/loader.service';
import { StringConstant } from '../../shared/stringconstant';
import { ActivatedRouteStub } from "../../shared/mocks/mock.activatedroute";

let promise: TestBed;
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
        this.promise = TestBed.configureTestingModule({
            declarations: [RouterLinkStubDirective], //Declaration of mock routerLink used on page.
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

    it("should be defined ProjectEditComponent", () => {
        let fixture = TestBed.createComponent(ProjectEditComponent);
        let projectEditComponent = fixture.componentInstance;
        expect(projectEditComponent).toBeDefined();
    });

    it("should get selected Project", done => {
        this.promise.then(() => {
            let fixture = TestBed.createComponent(ProjectEditComponent); //Create instance of component            
            let activatedRoute = fixture.debugElement.injector.get(ActivatedRoute);
            activatedRoute.testParams = { id: stringConstant.id };
            let projectEditComponent = fixture.componentInstance;
            projectEditComponent.ngOnInit();
            expect(projectEditComponent.Userlist).not.toBeNull();
            done();
        });
    });

    it("should check Project name and Slack Channel Name before update", done => {
        this.promise.then(() => {
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
            done();
        });
    });


    it("should check Project name before update", done => {
        this.promise.then(() => {
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
            done();
        });
    });

   
});    




declare let describe, it, beforeEach, expect;
import { async, inject, TestBed, ComponentFixture } from '@angular/core/testing';
import {Provider} from "@angular/core";
import {ProjectModel} from "../project.model";
import {ProjectAddComponent} from "../project-add/project-add.component";
import {ProjectService} from "../project.service";
import {UserModel} from '../../users/user.model';
import { Router, ActivatedRoute, RouterModule, Routes} from '@angular/router';
import { Md2Toast } from 'md2';
import {MockToast} from "../../shared/mocks/mock.toast";
import {Md2Multiselect } from 'md2/multiselect';
import {MockProjectService} from "../../shared/mocks/project/mock.project.service";
import {MockRouter} from '../../shared/mocks/mock.router';
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

describe('Project Add Test', () => {
    
    const routes: Routes = [];
    
    beforeEach(async(() => {
        this.promise = TestBed.configureTestingModule({
            declarations: [RouterLinkStubDirective], //Declaration of mock routerLink used on page.
            imports: [ProjectModule,RouterModule.forRoot(routes, { useHash: true }) //Set LocationStrategy for component. 
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

    it("should be defined projectAddComponent", () => {
        let fixture = TestBed.createComponent(ProjectAddComponent);
        let projectAddComponent = fixture.componentInstance;
        expect(projectAddComponent).toBeDefined();
    });

    it("should get user list for project", done => {
        this.promise.then(() => {
            let fixture = TestBed.createComponent(ProjectAddComponent); //Create instance of component            
            let projectAddComponent = fixture.componentInstance;
            let toast = fixture.debugElement.injector.get(Md2Toast);
            projectAddComponent.ngOnInit();
            expect(projectAddComponent.Userlist).not.toBeNull();
            done();
        });
    });
    it("should be add new project", done => {
        this.promise.then(() => {
            let fixture = TestBed.createComponent(ProjectAddComponent); //Create instance of component            
            let projectAddComponent = fixture.componentInstance;
            let toast = fixture.debugElement.injector.get(Md2Toast);
            let projectModels = new ProjectModel();
            projectModels.name = stringConstant.projectName;
            projectModels.slackChannelName = stringConstant.slackChannelName;
            projectModels.applicationUsers = mockList;
            projectModels.teamLeaderId = stringConstant.teamLeaderId;
            projectAddComponent.addProject(projectModels);
            expect(projectModels.name).toBe(stringConstant.projectName);
            done();
        });
    });

    it("should be check project name before Added", done => {
        this.promise.then(() => {
            let fixture = TestBed.createComponent(ProjectAddComponent); //Create instance of component            
            let projectAddComponent = fixture.componentInstance;
            let toast = fixture.debugElement.injector.get(Md2Toast);
            let projectModels = new ProjectModel();
            projectModels.name = null;
            projectModels.slackChannelName = stringConstant.slackChannelName;
            projectModels.applicationUsers = mockList;
            projectModels.teamLeaderId = stringConstant.teamLeaderId;
            projectAddComponent.addProject(projectModels);
            expect(projectModels.name).toBe(null);
            done();
        });
    });

    it("should be check Slack Channel Name before Added", done => {
        this.promise.then(() => {
            let fixture = TestBed.createComponent(ProjectAddComponent); //Create instance of component            
            let projectAddComponent = fixture.componentInstance;
            let toast = fixture.debugElement.injector.get(Md2Toast);
            let projectModels = new ProjectModel();
            projectModels.name = stringConstant.projectName;
            projectModels.slackChannelName = null;
            projectModels.applicationUsers = mockList;
            projectModels.teamLeaderId = stringConstant.teamLeaderId;
            projectAddComponent.addProject(projectModels);
            expect(projectModels.slackChannelName).toBe(null);
            done();
        });
    });
});    







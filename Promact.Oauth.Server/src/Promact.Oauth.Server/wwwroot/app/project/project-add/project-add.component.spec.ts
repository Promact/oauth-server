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

let promise: TestBed;


describe('Project Add Test', () => {
    class MockLoaderService { }
    const routes: Routes = [];
    class MockActivatedRoute extends ActivatedRoute {
        constructor() {
            super();
            this.params = Observable.of({ });
        }
    }
    beforeEach(async(() => {
        this.promise = TestBed.configureTestingModule({
            declarations: [RouterLinkStubDirective], //Declaration of mock routerLink used on page.
            imports: [ProjectModule,RouterModule.forRoot(routes, { useHash: true }) //Set LocationStrategy for component. 
            ],
            providers: [
                { provide: ActivatedRoute, useClass: MockActivatedRoute },
                { provide: Router, useClass: MockRouter },
                { provide: ProjectService, useClass: MockProjectService },
                { provide: Md2Toast, useClass: MockToast },
                { provide: UserModel, useClass: UserModel },
                { provide: ProjectModel, useClass: ProjectModel },
                { provide: LoaderService, useClass: MockLoaderService }
            ]
        }).compileComponents();
            
    }));

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
            done();
        });
    });
});    







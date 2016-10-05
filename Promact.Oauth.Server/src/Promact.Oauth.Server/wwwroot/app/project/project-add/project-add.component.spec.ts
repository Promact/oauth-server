declare var describe, it, beforeEach, expect;
import { async, inject, TestBed, ComponentFixture } from '@angular/core/testing';
import {Provider} from "@angular/core";
import {projectModel} from "../project.model";
import {ProjectAddComponent} from "../project-add/project-add.component";
import {ProjectService} from "../project.service";
import {UserModel} from '../../users/user.model';
import { Router, ActivatedRoute, RouterModule, Routes} from '@angular/router';
import { Md2Toast } from 'md2/toast/toast';
import {MockToast} from "../../shared/mocks/mock.toast";
import {Md2Multiselect } from 'md2/multiselect';
import {MockProjectService} from "../../shared/mocks/project/mock.project.service";
import {MockRouter} from '../../shared/mocks/mock.router';
import { Observable } from 'rxjs/Observable';
import { RouterLinkStubDirective } from '../../shared/mocks/mock.routerLink';
import { ProjectModule } from '../project.module';
let promise: TestBed;


describe('Project Add Test', () => {
    let projectAddComponent: ProjectAddComponent;
    class MockRouter { }
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
                { provide: ProjectService, useClass: ProjectService },
                { provide: UserModel, useClass: UserModel },
                { provide: projectModel, useClass: projectModel },
            ]
        }).compileComponents();
            this.promise.then(() => {
                this.fixture = TestBed.createComponent(ProjectAddComponent); //Create instance of component            
            this.projectListComponent = this.fixture.componentInstance;
        });
    }));

    it("should get default Project for company", done => {
        this.promise.then(() => {
            expect(projectAddComponent).toBeDefined();
            done();
        })
    });
   

    //it("should get default page for Project", () => {
    //    projectAddComponent.ngOnInit();
    //    expect(projectAddComponent.Userlist).not.toBeNull();
    //});

    
    //it("should check project name before add", inject([projectModel], (projectModel: projectModel) => {
    //    let expectedProjectName = "Tests Projects";
    //    projectModel.Name = expectedProjectName;
    //    let expectedSlackChannelName = "Test Slack Name";
    //    projectModel.SlackChannelName = expectedSlackChannelName;
    //    projectAddComponent.addProject(projectModel);
    //    expect(projectModel.Name).toBe(expectedProjectName);
    //}));
});    







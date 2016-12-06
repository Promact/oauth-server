declare var describe, it, beforeEach, expect;
import { async, inject, TestBed, ComponentFixture } from '@angular/core/testing';
import { Provider } from "@angular/core";
import { ProjectModel } from "../project.model";
import { ProjectViewComponent } from "../project-view/project-view.component";
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
import { ActivatedRouteStub } from "../../shared/mocks/mock.activatedroute";

let promise: TestBed;


describe('Project View Test', () => {
    class MockLocation { }
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
                { provide: UserModel, useClass: UserModel },
                { provide: ProjectModel, useClass: ProjectModel },
                { provide: Location, useClass: MockLocation },
                { provide: Md2Toast, useClass: MockToast }
            ]
        }).compileComponents();

    }));

    it("should get default Project for company", done => {
        this.promise.then(() => {
            let fixture = TestBed.createComponent(ProjectViewComponent); //Create instance of component  
            let activatedRoute = fixture.debugElement.injector.get(ActivatedRoute);
            activatedRoute.testParams = { id: "1" };                
            let projectViewComponent = fixture.componentInstance;
            projectViewComponent.ngOnInit();
            expect(projectViewComponent.Userlist).not.toBeNull();
            done();
        });
    });

    it("should get default Project for company", done => {
        this.promise.then(() => {
            let fixture = TestBed.createComponent(ProjectViewComponent); //Create instance of component  
            let activatedRoute = fixture.debugElement.injector.get(ActivatedRoute);
            activatedRoute.testParams = { id: "1" };
            let projectViewComponent = fixture.componentInstance;
            projectViewComponent.ngOnInit();
            expect(projectViewComponent.project).not.toBeNull();
            done();
        });
    });

    it("should check Team Leader Name", done => {
        this.promise.then(() => {
            let fixture = TestBed.createComponent(ProjectViewComponent); //Create instance of component  
            let activatedRoute = fixture.debugElement.injector.get(ActivatedRoute);
            activatedRoute.testParams = { id: "1" };
            let projectViewComponent = fixture.componentInstance;
            projectViewComponent.ngOnInit();
            expect(projectViewComponent.teamLeaderFirstName).not.toBeNull();
            done();
        });
    });

    it("should check Team Leader Email", done => {
        this.promise.then(() => {
            let fixture = TestBed.createComponent(ProjectViewComponent); //Create instance of component  
            let activatedRoute = fixture.debugElement.injector.get(ActivatedRoute);
            activatedRoute.testParams = { id: "1" };
            let projectViewComponent = fixture.componentInstance;
            projectViewComponent.ngOnInit();
            expect(projectViewComponent.teamLeaderEmail).not.toBeNull();
            done();
        });
    });
});

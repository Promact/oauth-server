declare var describe, it, beforeEach, expect, spyOn;
import { async, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { ProjectModel } from "../project.model";
import { ProjectViewComponent } from "../project-view/project-view.component";
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
import { MockLocation } from '../../shared/mocks/mock.location';
import { MockRouter } from '../../shared/mocks/mock.router';
import { Location } from "@angular/common";

let stringConstant = new StringConstant();

let mockUser = new UserModel();
mockUser.FirstName = stringConstant.firstName;
mockUser.LastName = stringConstant.lastName;
mockUser.Email = stringConstant.email;
mockUser.IsActive = true;
mockUser.Id = stringConstant.id;
let mockList = new Array<UserModel>();
mockList.push(mockUser);


describe('Project View Test', () => {
    const routes: Routes = [];
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            imports: [ProjectModule, RouterModule.forRoot(routes, { useHash: true }) //Set LocationStrategy for component. 
            ],
            providers: [
                { provide: ActivatedRoute, useClass: ActivatedRouteStub },
                { provide: ProjectService, useClass: MockProjectService },
                { provide: UserModel, useClass: UserModel },
                { provide: ProjectModel, useClass: ProjectModel },
                { provide: Md2Toast, useClass: MockToast },
                { provide: StringConstant, useClass: StringConstant },
                { provide: Router, useClass: MockRouter },
                { provide: Location, useClass: MockLocation }
                 
            ]
        }).compileComponents();

    }));

    it("should be defined ProjectViewComponent", () => {
        let fixture = TestBed.createComponent(ProjectViewComponent);
        let projectViewComponent = fixture.componentInstance;
        expect(projectViewComponent).toBeDefined();
    });

  
    it("should get default Project for company", fakeAsync(() => {
        let fixture = TestBed.createComponent(ProjectViewComponent); //Create instance of component  
        let activatedRoute = fixture.debugElement.injector.get(ActivatedRoute);
        activatedRoute.testParams = { id: stringConstant.id };
        let projectViewComponent = fixture.componentInstance;
        projectViewComponent.ngOnInit();
        tick();
        expect(projectViewComponent.Userlist).not.toBeNull();
    }));

    it("should get default Project for company", fakeAsync(() => {

        let fixture = TestBed.createComponent(ProjectViewComponent); //Create instance of component  
        let activatedRoute = fixture.debugElement.injector.get(ActivatedRoute);
        activatedRoute.testParams = { id: stringConstant.id };
        let projectViewComponent = fixture.componentInstance;
        projectViewComponent.ngOnInit();
        tick();
        expect(projectViewComponent.project).not.toBeNull();
    }));

    it("should check Team Leader Name", fakeAsync(() => {
        let fixture = TestBed.createComponent(ProjectViewComponent); //Create instance of component  
        let activatedRoute = fixture.debugElement.injector.get(ActivatedRoute);
        activatedRoute.testParams = { id: stringConstant.id };
        let projectViewComponent = fixture.componentInstance;
        projectViewComponent.ngOnInit();
        tick();
        expect(projectViewComponent.teamLeaderFirstName).not.toBeNull();
    }));

    it("should check Team Leader Email", fakeAsync(() => {

        let fixture = TestBed.createComponent(ProjectViewComponent); //Create instance of component  
        let activatedRoute = fixture.debugElement.injector.get(ActivatedRoute);
        activatedRoute.testParams = { id: stringConstant.id };
        let projectViewComponent = fixture.componentInstance;
        projectViewComponent.ngOnInit();
        tick();
        expect(projectViewComponent.teamLeaderEmail).not.toBeNull();
    }));

    it('should be list of users', fakeAsync(() => {
        let fixture = TestBed.createComponent(ProjectViewComponent);
        let activatedRoute = fixture.debugElement.injector.get(ActivatedRoute);
        activatedRoute.testParams = { id: stringConstant.id };
        let projectViewComponent = fixture.componentInstance;
        let projectService = fixture.debugElement.injector.get(ProjectService);
        let projectModel = new ProjectModel();
        projectModel.TeamLeaderId = null;
        projectModel.ApplicationUsers = mockList;
        let project = new ProjectModel();
        spyOn(projectService, "getProject", activatedRoute.testParams).and.returnValue((Promise.resolve(projectModel)));
        spyOn(projectService, "getUsers", activatedRoute.testParams).and.returnValue((Promise.resolve(mockList)));
        projectViewComponent.ngOnInit();
        tick();
        expect(projectModel.TeamLeaderId).toBeNull();
    }));

    it('should be TeamLeader Null', fakeAsync(() => {
        let fixture = TestBed.createComponent(ProjectViewComponent);
        let activatedRoute = fixture.debugElement.injector.get(ActivatedRoute);
        activatedRoute.testParams = { id: stringConstant.id };
        let projectViewComponent = fixture.componentInstance;
        let projectService = fixture.debugElement.injector.get(ProjectService);
        let projectModel = new ProjectModel();
        projectModel.TeamLeaderId = null;
        projectModel.ApplicationUsers = new Array<UserModel>();
        let project = new ProjectModel();
        spyOn(projectService, "getProject", activatedRoute.testParams).and.returnValue((Promise.resolve(projectModel)));
        spyOn(projectService, "getUsers", activatedRoute.testParams).and.returnValue((Promise.resolve(mockList)));
        projectViewComponent.ngOnInit();
        tick();
        expect(projectModel.TeamLeaderId).toBeNull();
    }));

    it("should get error on selected Project Id", fakeAsync(() => {
        let fixture = TestBed.createComponent(ProjectViewComponent); //Create instance of component            
        let projectViewComponent = fixture.componentInstance;
        let activatedRoute = fixture.debugElement.injector.get(ActivatedRoute);
        activatedRoute.testParams = { id: stringConstant.id };
        let projectService = fixture.debugElement.injector.get(ProjectService);
        spyOn(projectService, "getProject").and.returnValue(Promise.reject(""));
        projectViewComponent.ngOnInit();
        tick();
        expect(projectViewComponent.Userlist).not.toBeNull();
    }));

    it('should be Team Leader not null', fakeAsync(() => {
        let fixture = TestBed.createComponent(ProjectViewComponent);
        let activatedRoute = fixture.debugElement.injector.get(ActivatedRoute);
        activatedRoute.testParams = { id: stringConstant.id };
        let projectViewComponent = fixture.componentInstance;
        let projectService = fixture.debugElement.injector.get(ProjectService);
        let projectModel = new ProjectModel();
        projectModel.TeamLeaderId = stringConstant.id;
        projectModel.TeamLeader = mockUser;
        
        projectModel.ApplicationUsers = new Array<UserModel>();
        let project = new ProjectModel();
        spyOn(projectService, "getProject", activatedRoute.testParams).and.returnValue((Promise.resolve(projectModel)));
        spyOn(projectService, "getUsers", activatedRoute.testParams).and.returnValue((Promise.resolve(mockList)));
        projectViewComponent.ngOnInit();
        tick();
        expect(projectModel.TeamLeaderId).toBe(stringConstant.id);
    }));

    it('should be rediration to project list', fakeAsync(() => {
        let fixture = TestBed.createComponent(ProjectViewComponent);
        let projectViewComponent = fixture.componentInstance;
        let location = fixture.debugElement.injector.get(Location);
        spyOn(location, "back");
        projectViewComponent.gotoProjects();
        tick();
        expect(location.back).toHaveBeenCalled();
    }));
   
});

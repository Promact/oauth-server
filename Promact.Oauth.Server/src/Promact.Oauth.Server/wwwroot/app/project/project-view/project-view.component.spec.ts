declare var describe, it, beforeEach, expect;
import { async, TestBed} from '@angular/core/testing';
import { ProjectModel } from "../project.model";
import { ProjectViewComponent } from "../project-view/project-view.component";
import { ProjectService } from "../project.service";
import { UserModel } from '../../users/user.model';
import { ActivatedRoute, RouterModule, Routes } from '@angular/router';
import { Md2Toast } from 'md2';
import { MockToast } from "../../shared/mocks/mock.toast";
import { MockProjectService } from "../../shared/mocks/project/mock.project.service";
import { ProjectModule } from '../project.module';
import { LoaderService } from '../../shared/loader.service';
import { ActivatedRouteStub } from "../../shared/mocks/mock.activatedroute";
import { StringConstant } from '../../shared/stringconstant';

let stringConstant = new StringConstant();
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
                { provide: StringConstant, useClass: StringConstant }
            ]
        }).compileComponents();

    }));

    it("should get default Project for company", () => {
        let fixture = TestBed.createComponent(ProjectViewComponent); //Create instance of component  
        let activatedRoute = fixture.debugElement.injector.get(ActivatedRoute);
        activatedRoute.testParams = { id: stringConstant.id };
        let projectViewComponent = fixture.componentInstance;
        projectViewComponent.ngOnInit();
        expect(projectViewComponent.Userlist).not.toBeNull();
    });

    it("should get default Project for company", () => {

        let fixture = TestBed.createComponent(ProjectViewComponent); //Create instance of component  
        let activatedRoute = fixture.debugElement.injector.get(ActivatedRoute);
        activatedRoute.testParams = { id: stringConstant.id};
        let projectViewComponent = fixture.componentInstance;
        projectViewComponent.ngOnInit();
        expect(projectViewComponent.project).not.toBeNull();
    });

    it("should check Team Leader Name", () => {
        let fixture = TestBed.createComponent(ProjectViewComponent); //Create instance of component  
        let activatedRoute = fixture.debugElement.injector.get(ActivatedRoute);
        activatedRoute.testParams = { id: stringConstant.id };
        let projectViewComponent = fixture.componentInstance;
        projectViewComponent.ngOnInit();
        expect(projectViewComponent.teamLeaderFirstName).not.toBeNull();
    });

    it("should check Team Leader Email", () => {

        let fixture = TestBed.createComponent(ProjectViewComponent); //Create instance of component  
        let activatedRoute = fixture.debugElement.injector.get(ActivatedRoute);
        activatedRoute.testParams = { id: stringConstant.id};
        let projectViewComponent = fixture.componentInstance;
        projectViewComponent.ngOnInit();
        expect(projectViewComponent.teamLeaderEmail).not.toBeNull();

    });

});

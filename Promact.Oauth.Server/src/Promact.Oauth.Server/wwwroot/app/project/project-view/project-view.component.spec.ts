declare var describe, it, beforeEach, expect;
import { async, inject, TestBed, ComponentFixture } from '@angular/core/testing';
import { Provider } from "@angular/core";
import { ProjectModel } from "../project.model";
import { ProjectViewComponent } from "../project-view/project-view.component";
import { ProjectService } from "../project.service";
import { UserModel } from '../../users/user.model';
import { Router, ActivatedRoute, RouterModule, Routes } from '@angular/router';
import { Md2Toast } from 'md2/toast/toast';
import { MockToast } from "../../shared/mocks/mock.toast";
import { Md2Multiselect } from 'md2/multiselect';
import { MockProjectService } from "../../shared/mocks/project/mock.project.service";
import { MockRouter } from '../../shared/mocks/mock.router';
import { Observable } from 'rxjs/Observable';
import { RouterLinkStubDirective } from '../../shared/mocks/mock.routerLink';
import { ProjectModule } from '../project.module';
import { LoaderService } from '../../shared/loader.service';

let promise: TestBed;


describe('Project View Test', () => {
    class MockRouter { }
    class MockLocation { }
    class MockLoaderService { }
    const routes: Routes = [];
    class MockActivatedRoute extends ActivatedRoute {
        constructor() {
            super();
            this.params = Observable.of({ id: "1" });
        }
    }


    beforeEach(async(() => {
        this.promise = TestBed.configureTestingModule({
            declarations: [RouterLinkStubDirective], //Declaration of mock routerLink used on page.
            imports: [ProjectModule, RouterModule.forRoot(routes, { useHash: true }) //Set LocationStrategy for component. 
            ],
            providers: [
                { provide: ActivatedRoute, useClass: MockActivatedRoute },
                { provide: Router, useClass: MockRouter },
                { provide: ProjectService, useClass: MockProjectService },
                { provide: UserModel, useClass: UserModel },
                { provide: ProjectModel, useClass: ProjectModel },
                { provide: Location, useClass: MockLocation }
            ]
        }).compileComponents();

    }));

    it("should get default Project for company", done => {
        this.promise.then(() => {
            let fixture = TestBed.createComponent(ProjectViewComponent); //Create instance of component            
            let projectViewComponent = fixture.componentInstance;
            projectViewComponent.ngOnInit();
            expect(projectViewComponent.Userlist).not.toBeNull();
            done();
        });
    });
});

import {async,inject,TestBed,ComponentFixture} from '@angular/core/testing';
import { By } from "@angular/platform-browser";
import { Provider } from "@angular/core";
import { ProjectService }   from '../project.service';
import {ProjectModel} from '../project.model';
import { Router, RouterModule, Routes} from '@angular/router';
import {ProjectListComponent} from './project-list.component';
import {MockProjectService} from "../../shared/mocks/project/mock.project.service";
import { LoginService } from '../../login.service';
import { ProjectModule } from '../project.module';
import { Md2Toast } from 'md2';
import { LoaderService } from '../../shared/loader.service';
import { RouterLinkStubDirective } from '../../shared/mocks/mock.routerLink';
import { UserRole } from "../../shared/userrole.model";
import { StringConstant } from '../../shared/stringconstant';
import { MockRouter } from '../../shared/mocks/mock.router';
import { MockToast } from "../../shared/mocks/mock.toast";

declare var describe, it, beforeEach, expect;
let comp: ProjectListComponent;
let fixture: ComponentFixture<ProjectListComponent>;

let promise: TestBed;

describe("Project List Test", () => {
   
    const routes: Routes = [];
    beforeEach(async(() => {
        this.promise = TestBed.configureTestingModule({
            declarations: [RouterLinkStubDirective], //Declaration of mock routerLink used on page.
            imports: [ProjectModule,RouterModule.forRoot(routes, { useHash: true }) //Set LocationStrategy for component. 
            ],
            providers: [
                { provide: Router, useClass: MockRouter },
                { provide: ProjectService, useClass: MockProjectService },
                { provide: Md2Toast, useClass: MockToast },
                { provide: LoaderService, useClass: LoaderService },
                { provide: UserRole, useClass: UserRole },
                { provide: StringConstant, useClass: StringConstant }
            ]
        }).compileComponents();
    }));

    it("should be defined ProjectListComponent", () => {
        let fixture = TestBed.createComponent(ProjectListComponent);
        let projectListComponent = fixture.componentInstance;
        expect(projectListComponent).toBeDefined();
    });
 
    it("should get Projects for company", done => {
        this.promise.then(() => {
            let fixture = TestBed.createComponent(ProjectListComponent); //Create instance of component            
            let projectListComponent = fixture.componentInstance;
            projectListComponent.getProjects();
            expect(projectListComponent.projects.length).toBe(1);
            done();
        });
    });
});


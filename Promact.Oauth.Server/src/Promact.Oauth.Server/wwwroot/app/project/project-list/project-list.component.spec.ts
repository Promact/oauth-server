import { async, inject, TestBed, ComponentFixture } from '@angular/core/testing';
import { By } from "@angular/platform-browser";
import { Provider } from "@angular/core";
import { ProjectService } from '../project.service';
import { ProjectModel } from '../project.model';
import { Router, RouterModule, Routes } from '@angular/router';
import { ProjectListComponent } from './project-list.component';
import { MockProjectService } from "../../shared/mocks/project/mock.project.service";
import { LoginService } from '../../login.service';
import { ProjectModule } from '../project.module';
import { Md2Toast } from 'md2';
import { LoaderService } from '../../shared/loader.service';
import { UserRole } from "../../shared/userrole.model";


declare var describe, it, beforeEach, expect;
let comp: ProjectListComponent;
let fixture: ComponentFixture<ProjectListComponent>;

describe("Project List Test", () => {

    class MockRouter { }
    class McokLogin { }
    class Md2Toast { }
    class MockHttpService { }
    class MockLoaderService { }
    class MockUserRole { }
    const routes: Routes = [];
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            imports: [ProjectModule, RouterModule.forRoot(routes, { useHash: true }) //Set LocationStrategy for component. 
            ],
            providers: [
                { provide: Router, useClass: MockRouter },
                { provide: ProjectService, useClass: MockProjectService },
                { provide: Md2Toast, useClass: Md2Toast },
                { provide: LoginService, useClass: McokLogin },
                { provide: LoaderService, useClass: MockLoaderService },
                { provide: UserRole, useClass: MockUserRole }

            ]
        }).compileComponents();
    }));


    it("should get Projects for company", () => {
        let fixture = TestBed.createComponent(ProjectListComponent); //Create instance of component            
        let projectListComponent = fixture.componentInstance;
        projectListComponent.getProjects();
        expect(projectListComponent.projects.length).toBe(1);
    });
});


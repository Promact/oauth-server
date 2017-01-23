import { async, TestBed, ComponentFixture } from '@angular/core/testing';
import { ProjectService } from '../project.service';
import { ProjectModel } from '../project.model';
import { RouterModule, Routes } from '@angular/router';
import { ProjectListComponent } from './project-list.component';
import { MockProjectService } from "../../shared/mocks/project/mock.project.service";
import { ProjectModule } from '../project.module';
import { Md2Toast } from 'md2';
import { LoaderService } from '../../shared/loader.service';
import { UserRole } from "../../shared/userrole.model";
import { StringConstant } from '../../shared/stringconstant';
import { MockRouter } from '../../shared/mocks/mock.router';
import { MockToast } from "../../shared/mocks/mock.toast";

declare var describe, it, beforeEach, expect;
let fixture: ComponentFixture<ProjectListComponent>;

describe("Project List Test", () => {
   
    const routes: Routes = [];
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            imports: [ProjectModule, RouterModule.forRoot(routes, { useHash: true }) //Set LocationStrategy for component. 
            ],
            providers: [
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


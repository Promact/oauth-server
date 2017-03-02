declare var describe, it, beforeEach, expect;
import { async, TestBed } from '@angular/core/testing';
import { Router, RouterModule, Routes } from '@angular/router';
import { MockRouter } from '../shared/mocks/mock.router';
import { UserRole } from "../shared/userrole.model";
import { RouterLinkStubDirective } from '../shared/mocks/mock.routerLink';
import { ProjectComponent } from './project.component';
import { StringConstant } from '../shared/stringconstant';
import { ProjectModule } from './project.module';

let stringConstant = new StringConstant();
describe('Project Component Test', () => {
    const routes: Routes = [];

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [RouterLinkStubDirective],
            imports: [ProjectModule, RouterModule.forRoot(routes, { useHash: true })],
            providers: [
                { provide: Router, useClass: MockRouter },
                { provide: UserRole, useClass: UserRole },
               { provide: StringConstant, useClass: StringConstant }]
        }).compileComponents();
    }));

    it("Load project Component", () => {
        let fixture = TestBed.createComponent(ProjectComponent);
        let projectComponent = fixture.componentInstance;
        expect(projectComponent).toBeDefined();
    });
    it("test ngOnInit method", () => {
        let fixture = TestBed.createComponent(ProjectComponent); //Create instance of component     
        let projectComponent = fixture.componentInstance;
        projectComponent.ngOnInit();
        expect(projectComponent.admin).not.toBeNull();
    });
    it("test ngOnInit method user role not Admin", () => {
        let fixture = TestBed.createComponent(ProjectComponent); //Create instance of component     
        let user: UserRole = fixture.debugElement.injector.get(UserRole);
        user.Role = stringConstant.userRole;
        fixture.detectChanges();
        let projectComponent = fixture.componentInstance;
        projectComponent.ngOnInit();
        expect(projectComponent.admin).not.toBeNull();
   });
});
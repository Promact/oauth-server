declare var describe, it, beforeEach, expect;
import { async, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { Provider } from "@angular/core";
import { Router, RouterModule, Routes } from '@angular/router';
import { MockRouter } from '../shared/mocks/mock.router';
import { UserRole } from "../shared/userrole.model";
import { UserComponent } from './user.component';
import { UserModule } from './user.module';
import { StringConstant } from '../shared/stringconstant';

let stringConstant = new StringConstant();

describe('App Component Test', () => {
    const routes: Routes = [];

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            imports: [UserModule, RouterModule.forRoot(routes, { useHash: true })],
            providers: [
                { provide: Router, useClass: MockRouter },
                { provide: UserRole, useClass: UserRole },
                { provide: StringConstant, useClass: StringConstant }]
        }).compileComponents();
    }));


    it("Load component for admin user", fakeAsync(() => {
        let fixture = TestBed.createComponent(UserComponent); //Create instance of component     
        let userComponent = fixture.componentInstance;
        userComponent.ngOnInit();
        tick();
        expect(userComponent.admin).toBe(true);
    }));

    it("Load component", fakeAsync(() => {
        let fixture = TestBed.createComponent(UserComponent); //Create instance of component     
        let user = fixture.debugElement.injector.get(UserRole);
        user.Role = stringConstant.employee;
        fixture.detectChanges();
        let userComponent = fixture.componentInstance;
        userComponent.ngOnInit();
        tick();
        expect(userComponent.admin).toBe(false);
    }));


}); 
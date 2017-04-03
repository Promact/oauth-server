declare let describe, it, beforeEach, expect, spyOn;
import { async, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { UserModel } from '../../users/user.model';
import { UserEditComponent } from "../user-edit/user-edit.component";
import { UserService } from "../user.service";
import { UserModule } from '../user.module';
import { Router, ActivatedRoute, RouterModule, Routes } from '@angular/router';
import { Md2Toast } from 'md2';
import { MockToast } from "../../shared/mocks/mock.toast";
import { MockUserService } from "../../shared/mocks/user/mock.user.service";
import { MockRouter } from '../../shared/mocks/mock.router';
import { LoaderService } from '../../shared/loader.service';
import { ActivatedRouteStub } from "../../shared/mocks/mock.activatedroute";
import { UserRole } from "../../shared/userrole.model";
import { StringConstant } from '../../shared/stringconstant';

let stringConstant = new StringConstant();
let userRole = new UserRole();

describe("User Edit Test", () => {
    let userService: UserService;
    const routes: Routes = [];
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            imports: [UserModule, RouterModule.forRoot(routes, { useHash: true }) //Set LocationStrategy for component. 
            ],
            providers: [
                { provide: UserService, useClass: MockUserService },
                { provide: ActivatedRoute, useClass: ActivatedRouteStub },
                { provide: Router, useClass: MockRouter },
                { provide: Md2Toast, useClass: MockToast },
                { provide: UserModel, useClass: UserModel },
                { provide: LoaderService, useClass: LoaderService },
                { provide: UserRole, useClass: UserRole },
                { provide: StringConstant, useClass: StringConstant }
            ]
        }).compileComponents();

    }));


    it("should get particular user details", fakeAsync(() => {
        let fixture = TestBed.createComponent(UserEditComponent); //Create instance of component     
        let activatedRoute = fixture.debugElement.injector.get(ActivatedRoute);
        activatedRoute.testParams = { id: stringConstant.id };
        let userEditComponent = fixture.componentInstance;
        let expectedFirstName = stringConstant.testfirstName;
        userEditComponent.ngOnInit();
        tick();
        expect(userEditComponent.user.FirstName).toBe(expectedFirstName);
    }));


    it("Get roles but user is Not Admin", fakeAsync(() => {
        let fixture = TestBed.createComponent(UserEditComponent); //Create instance of component     
        let activatedRoute = fixture.debugElement.injector.get(ActivatedRoute);
        activatedRoute.testParams = { id: stringConstant.id };
        let user = fixture.debugElement.injector.get(UserRole);
        user.Role = stringConstant.employee;
        fixture.detectChanges();
        let userEditComponent = fixture.componentInstance;
        userEditComponent.ngOnInit();
        tick();
        expect(userEditComponent.admin).toBe(false);
    }));


    it("should not get particular user details", fakeAsync(() => {
        let fixture = TestBed.createComponent(UserEditComponent); //Create instance of component     
        let activatedRoute = fixture.debugElement.injector.get(ActivatedRoute);
        activatedRoute.testParams = { id: stringConstant.testfirstName };
        let userEditComponent = fixture.componentInstance;
        let expectedFirstName = stringConstant.testfirstName;
        userEditComponent.ngOnInit();
        tick();
        expect(userEditComponent.user.FirstName).not.toBe(expectedFirstName);
    }));


    it("should check User first name before update", fakeAsync(() => {
        let fixture = TestBed.createComponent(UserEditComponent); //Create instance of component            
        let userEditComponent = fixture.componentInstance;
        let toast = fixture.debugElement.injector.get(Md2Toast);
        let router = fixture.debugElement.injector.get(Router);
        spyOn(router, stringConstant.navigate);
        let expectedFirstName = stringConstant.testfirstName;
        let userModel = new UserModel();
        userModel.FirstName = expectedFirstName;
        userEditComponent.editUser(userModel);
        tick();
        expect(router.navigate).toHaveBeenCalled();
    }));


    it("should not check User first name before update", fakeAsync(() => {
        let fixture = TestBed.createComponent(UserEditComponent); //Create instance of component            
        let userEditComponent = fixture.componentInstance;
        let toast = fixture.debugElement.injector.get(Md2Toast);
        let expectedFirstName = stringConstant.testfirstName;
        let userModel = new UserModel();
        userModel.FirstName = "";
        userEditComponent.editUser(userModel);
        tick();
        expect(userModel.FirstName).not.toBe(expectedFirstName);
    }));


    it("should check total role", fakeAsync(() => {
        let fixture = TestBed.createComponent(UserEditComponent); //Create instance of component            
        let userEditComponent = fixture.componentInstance;
        userEditComponent.getRoles();
        tick();
        expect(userEditComponent.listOfRoles.length).toBe(1);
    }));


    it('Calls goBack', fakeAsync(() => {
        let fixture = TestBed.createComponent(UserEditComponent); //Create instance of component            
        let userEditComponent = fixture.componentInstance;
        let router = fixture.debugElement.injector.get(Router);
        spyOn(router, stringConstant.navigate);
        userEditComponent.goBack();
        tick();
        expect(router.navigate).toHaveBeenCalled();
    }));


});
//declare var describe, it, beforeEach, expect, spyOn;
//import { async, TestBed, fakeAsync, tick } from '@angular/core/testing';
//import { UserModel } from '../../users/user.model';
//import { UserDetailsComponent } from "../user-details/user-details.component";
//import { UserService } from "../user.service";
//import { UserModule } from '../user.module';
//import { Router, ActivatedRoute, RouterModule, Routes } from '@angular/router';
//import { Md2Toast } from 'md2';
//import { MockToast } from "../../shared/mocks/mock.toast";
//import { MockUserService } from "../../shared/mocks/user/mock.user.service";
//import { LoaderService } from '../../shared/loader.service';
//import { ActivatedRouteStub } from "../../shared/mocks/mock.activatedroute";
//import { UserRole } from "../../shared/userrole.model";
//import { StringConstant } from '../../shared/stringconstant';
//import { MockRouter } from '../../shared/mocks/mock.router';

//let stringConstant = new StringConstant();


//describe("User Details Test", () => {
//    let userService: UserService;
//    const routes: Routes = [];
//    beforeEach(async(() => {
//        TestBed.configureTestingModule({
//            imports: [UserModule, RouterModule.forRoot(routes, { useHash: true }) //Set LocationStrategy for component. 
//            ],
//            providers: [
//                { provide: UserService, useClass: MockUserService },
//                { provide: ActivatedRoute, useClass: ActivatedRouteStub },
//                { provide: Router, useClass: MockRouter },
//                { provide: Md2Toast, useClass: MockToast },
//                { provide: UserModel, useClass: UserModel },
//                { provide: LoaderService, useClass: LoaderService },
//                { provide: UserRole, useValue: new UserRole() },
//                { provide: StringConstant, useClass: StringConstant }

//            ]
//        }).compileComponents();

//    }));


//    it("should be defined UserDetailsComponent", () => {
//        let fixture = TestBed.createComponent(UserDetailsComponent);
//        let userDetailsComponent = fixture.componentInstance;
//        expect(userDetailsComponent).toBeDefined();
//    });

//    it("should get default Project for company", fakeAsync(() => {
//        let fixture = TestBed.createComponent(UserDetailsComponent); //Create instance of component
//        let activatedRoute = fixture.debugElement.injector.get(ActivatedRoute);
//        activatedRoute.testParams = { id: stringConstant.id };
//        let userDetailsComponent: UserDetailsComponent = fixture.componentInstance;
//        let expectedFirstName = stringConstant.testfirstName;
//        fixture.detectChanges();
//        tick();
//        expect(userDetailsComponent.user.FirstName).toBe(expectedFirstName);
//    }));


//    it("should not get particular user details", fakeAsync(() => {
//        let fixture = TestBed.createComponent(UserDetailsComponent); //Create instance of component
//        let activatedRoute = fixture.debugElement.injector.get(ActivatedRoute);
//        activatedRoute.testParams = { id: stringConstant.testfirstName };
//        let userDetailsComponent = fixture.componentInstance;
//        let expectedFirstName = stringConstant.testfirstName;
//        userDetailsComponent.ngOnInit();
//        tick();
//        expect(userDetailsComponent.user.FirstName).not.toBe(expectedFirstName);
//    }));


//    it("Get user details but user is Not Admin", fakeAsync(() => {
//        let fixture = TestBed.createComponent(UserDetailsComponent); //Create instance of component
//        let activatedRoute = fixture.debugElement.injector.get(ActivatedRoute);
//        activatedRoute.testParams = { id: stringConstant.testfirstName };
//        let user = fixture.debugElement.injector.get(UserRole);
//        user.Role = stringConstant.employee;
//        fixture.detectChanges();
//        let userDetailsComponent = fixture.componentInstance;
//        userDetailsComponent.ngOnInit();
//        tick();
//        expect(userDetailsComponent.admin).toBe(false);
//    }));


//    it('Calls goBack', fakeAsync(() => {
//        let fixture = TestBed.createComponent(UserDetailsComponent); //Create instance of component          
//        let userDetailsComponent = fixture.componentInstance;
//        let router = fixture.debugElement.injector.get(Router);
//        spyOn(router, stringConstant.navigate);
//        userDetailsComponent.goBack();
//        tick();
//        expect(router.navigate).toHaveBeenCalled();
//    }));


//    it('Calls edit', fakeAsync(() => {
//        let fixture = TestBed.createComponent(UserDetailsComponent); //Create instance of component          
//        let userDetailsComponent = fixture.componentInstance;
//        let router = fixture.debugElement.injector.get(Router);
//        spyOn(router, stringConstant.navigate);
//        userDetailsComponent.edit(parseInt(stringConstant.id));
//        tick();
//        expect(router.navigate).toHaveBeenCalled();
//    }));

//});
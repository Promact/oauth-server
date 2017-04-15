//declare var describe, it, beforeEach, expect, spyOn;
//import { async, TestBed, ComponentFixture, fakeAsync, tick } from '@angular/core/testing';
//import { BehaviorSubject } from 'rxjs/BehaviorSubject';
//import { UserService } from '../user.service';
//import { UserModel } from '../user.model';
//import { Router, RouterModule, Routes } from '@angular/router';
//import { Md2Toast } from 'md2';
//import { UserListComponent } from '../user-list/user-list.component';
//import { MockToast } from "../../shared/mocks/mock.toast";
//import { MockUserService } from "../../shared/mocks/user/mock.user.service";
//import { UserModule } from '../user.module';
//import { LoaderService } from '../../shared/loader.service';
//import { MockRouter } from '../../shared/mocks/mock.router';
//import { StringConstant } from '../../shared/stringconstant';
//let loader = new LoaderService();
//let stringConstant = new StringConstant();
//let fixture: ComponentFixture<UserListComponent>;

//describe("User List Test", () => {
//    class MockLoaderService { }
//    const routes: Routes = [];
//    beforeEach(async(() => {
//        TestBed.configureTestingModule({
//            imports: [UserModule, RouterModule.forRoot(routes, { useHash: true }) //Set LocationStrategy for component. 
//            ],
//            providers: [
//                { provide: UserService, useClass: MockUserService },
//                { provide: Md2Toast, useClass: MockToast },
//                { provide: Router, useClass: MockRouter },
//                { provide: StringConstant, useClass: StringConstant },
//                { provide: LoaderService, useClass: MockLoaderService }
//            ]
//        }).compileComponents();
//    }));


//    it("should be defined userListComponent", () => {
//        let fixture = TestBed.createComponent(UserListComponent);
//        let userListComponent = fixture.componentInstance;
//        expect(userListComponent).toBeDefined();
//    });


//    it("should get default Users for company", fakeAsync(() => {
//        let fixture = TestBed.createComponent(UserListComponent); //Create instance of component            
//        let userListComponent = fixture.componentInstance;
//        userListComponent.ngOnInit();
//        tick();
//        expect(userListComponent.users.length).not.toBeNull();
//    }));


//    it('Calls userDetails', fakeAsync(() => {
//        let fixture = TestBed.createComponent(UserListComponent); //Create instance of component            
//        let userListComponent = fixture.componentInstance;
//        let router = fixture.debugElement.injector.get(Router);
//        spyOn(router, stringConstant.navigate);
//        userListComponent.userDetails(stringConstant.id);
//        tick();
//        expect(router.navigate).toHaveBeenCalled();
//    }));


//    it('Calls userEdit', fakeAsync(() => {
//        let fixture = TestBed.createComponent(UserListComponent); //Create instance of component            
//        let userListComponent = fixture.componentInstance;
//        let router = fixture.debugElement.injector.get(Router);
//        spyOn(router, stringConstant.navigate);
//        userListComponent.userEdit(stringConstant.id);
//        tick();
//        expect(router.navigate).toHaveBeenCalled();
//    }));


//    it("should delete user", fakeAsync(() => {
//        let fixture = TestBed.createComponent(UserListComponent); //Create instance of component            
//        let userListComponent = fixture.componentInstance;
//        userListComponent.userDelete(stringConstant.id);
//        tick();
//        expect(userListComponent.users.length).not.toBe(0);
//    }));


//    it("should not delete user", fakeAsync(() => {
//        let fixture = TestBed.createComponent(UserListComponent); //Create instance of component            
//        let userListComponent = fixture.componentInstance;
//        let userModel = new UserModel();
//        userModel.Id = "2";
//        userListComponent.userDelete(userModel.Id);
//        tick();
//        expect(userListComponent.users.length).toBe(0);
//    }));


//    it("should resend mail to user ", fakeAsync(() => {
//        let fixture = TestBed.createComponent(UserListComponent); //Create instance of component            
//        let userListComponent = fixture.componentInstance;
//        let userModel = new UserModel();
//        userModel.Id = stringConstant.id;
//        userListComponent.reSendMail(userModel);
//        tick();
//        expect(userListComponent.reSendMail).toBeDefined();
//    }));

//});
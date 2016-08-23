//import {addProviders, async, inject, TestBed, ComponentFixture, TestComponentBuilder} from '@angular/core/testing';
//import {provide} from "@angular/core";
//import {Location} from "@angular/common";
//import {LocationStrategy} from "@angular/common";
//import {UserEditComponent} from "../user-edit/user-edit.component";
//import {UserService} from "../user.service";
//import {UserModel} from '../../users/user.model';
//import {ROUTER_DIRECTIVES, Router, ActivatedRoute } from '@angular/router';
//import {Md2Toast} from 'md2/toast';
//import {MockToast} from "../../shared/mocks/mock.toast";
//import {Md2Multiselect } from 'md2/multiselect';
//import {TestConnection} from "../../shared/mocks/test.connection";
//import {MockUserService} from "../../shared/mocks/user/mock.user.service";
//import {MockBaseService} from '../../shared/mocks/mock.base';
//import {MockRouter} from '../../shared/mocks/mock.router';

//describe("Project Edit Test", () => {
//    let userEditComponent: UserEditComponent;
//    let userService: UserService;
//    class MockActivatedRoute { }
//    class MockLocation { }
//    beforeEach(() => {
//        TestBed.configureTestingModule({
//            providers: [
//                    provide(ActivatedRoute, { useClass: MockActivatedRoute }),
//                    provide(Router, { useClass: MockRouter }),
//                    provide(TestConnection, { useClass: TestConnection }),
//                    provide(UserService, { useClass: MockUserService }),
//                    provide(Md2Toast, { useClass: MockToast }),
//                    provide(MockBaseService, { useClass: MockBaseService }),
//                    provide(UserModel, { useClass: UserModel }),
//                    provide(Location, { useClass: MockLocation })
//            ]
//        });
//    });
//    beforeEach(inject([UserService, ActivatedRoute, Router, Md2Toast], (userService: UserService, route: ActivatedRoute, router: Router, toast: Md2Toast) => {
//        userEditComponent = new UserEditComponent(userService,route, router, toast);
//    }));
//    /**
//    * should check User name before update
//    */
//    it("should check User first name before update", inject([UserModel], (userModel: UserModel) => {
//        let expectedFirstName = "First Name";
//        userModel.FirstName = expectedFirstName;
//        userEditComponent.editUser(userModel);
//        expect(userModel.FirstName).toBe(expectedFirstName);
       
//    }));

//});
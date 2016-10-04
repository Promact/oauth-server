declare var describe, it, beforeEach, expect;
import {async, inject, TestBed, ComponentFixture} from '@angular/core/testing';
import {Provider} from "@angular/core";

//import {TestConnection} from "../../shared/mocks/test.connection";
//import { UserService }   from '../user/user.service';
//import {PasswordModel} from '../user/user-password.model';
//import {Router, ActivatedRoute} from '@angular/router';
//import {MockUserService} from "../../shared/mocks/user/mock.user.service";
//import {Md2Toast} from 'md2/toast';
//import {MockBaseService} from '../../shared/mocks/mock.base';
//import {MockRouter} from '../../shared/mocks/mock.router';
//import {MockToast} from "../../shared/mocks/mock.toast";
//import { ChangePasswordComponent } from "../change-password/change-password.component";

describe('demo test case', function () {
    it('test case', function () {
        expect(1+1).toEqual(2);
    });
});



//describe('User Add Test', () => {
//    let changePasswordComponent: ChangePasswordComponent;
//    //class MockRouter { }
//    class MockActivatedRoute { }

//    beforeEach(() => {
//        TestBed.configureTestingModule({
//            providers: [
//                { provide: ActivatedRoute, useClass: MockActivatedRoute },
//                { provide: Router, useClass: MockRouter },
//                { provide: TestConnection, useClass: TestConnection },
//                { provide: UserService, useClass: MockUserService },
//                { provide: Md2Toast, useClass: MockToast },
//                { provide: MockBaseService, useClass: MockBaseService },
//                { provide: PasswordModel, useClass: PasswordModel }
//            ]
//        });

//    });
//    beforeEach(inject([UserService, Router, ActivatedRoute, Md2Toast], (userService: UserService, router: Router, route: ActivatedRoute, toast: Md2Toast) => {
//         changePasswordComponent = new ChangePasswordComponent(userService, router, route, toast);
//    }));
//    it("should be defined", () => {
//        expect(changePasswordComponent).toBeDefined();
//    });
//    it("should check password before change", inject([PasswordModel], (passwordModel: PasswordModel) => {
//        passwordModel.NewPassword = "test123";
//        passwordModel.OldPassword = "test";
//        passwordModel.ConfirmPassword = "test123";
//        passwordModel.Email = "test@yahoo.com";
//        let result = true;
//        let method = changePasswordComponent.changePassword(passwordModel);
//        expect(method).not.toBe(result);
//    }));
//});  
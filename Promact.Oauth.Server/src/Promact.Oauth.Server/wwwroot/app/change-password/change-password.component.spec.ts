declare var describe, it, beforeEach, expect;
import {async, inject, TestBed, ComponentFixture, TestComponentBuilder, addProviders} from '@angular/core/testing';
import {provide} from "@angular/core";
import {Component, Input} from '@angular/core';
import {TestConnection} from "../shared/mocks/test.connection";
import { UserService }   from '../users/user.service';
import {PasswordModel} from '../users/user-password.model';
import {FORM_DIRECTIVES, FormBuilder, Validators } from '@angular/forms';
import {Router, ROUTER_DIRECTIVES, ActivatedRoute} from '@angular/router';
import {MockUserService} from "../shared/mocks/user/mock.user.service";
import {Md2Toast} from 'md2/toast';
import {MockBaseService} from '../shared/mocks/mock.base';
import {MockRouter} from '../shared/mocks/mock.router';
import {MockToast} from "../shared/mocks/mock.toast";
import {ChangePasswordComponent} from "../change-password/change-password.component";

describe('User Change Password Test', () => {
    let changePasswordComponent: ChangePasswordComponent;
    class MockActivatedRoute { }

    beforeEach(() => {
        TestBed.configureTestingModule({
            providers: [
                { provide: ActivatedRoute, useClass: MockActivatedRoute },
                { provide: Router, useClass: MockRouter },
                { provide: Md2Toast, useClass: MockToast }
            ]
        });
    });

    beforeEach(inject([UserService, Router, ActivatedRoute, Md2Toast], (userService: UserService, router: Router, route: ActivatedRoute, toast: Md2Toast) => {
         changePasswordComponent = new ChangePasswordComponent(userService, router, route, toast);
    }));

    it("contains spec with an expectation", function () {
        expect(true).toBe(true);
    });

    it("should check password before change", inject([PasswordModel], (passwordModel: PasswordModel) => {
        passwordModel.NewPassword = "test123";
        passwordModel.OldPassword = "test";
        passwordModel.ConfirmPassword = "test123";
        passwordModel.Email = "test@yahoo.com";
        var result = true;
        var method = changePasswordComponent.changePassword(passwordModel);
        expect(method).not.toBe(result);
    }));

}); 
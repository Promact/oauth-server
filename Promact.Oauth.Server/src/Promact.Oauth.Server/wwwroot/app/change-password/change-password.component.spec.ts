//declare var describe, it, beforeEach, expect;
//import {async, inject, TestBed, ComponentFixture, TestComponentBuilder} from '@angular/core/testing';
//import {provide} from "@angular/core";
//import {Component, Input} from '@angular/core';
//import {TestConnection} from "../../shared/mocks/test.connection";
//import { UserService }   from '../user.service';
//import {PasswordModel} from '../user-password.model';
//import {FORM_DIRECTIVES, FormBuilder, Validators } from '@angular/forms';
//import {Router, ROUTER_DIRECTIVES, ActivatedRoute} from '@angular/router';
//import {MockUserService} from "../../shared/mocks/user/mock.user.service";
//import {Md2Toast} from 'md2/toast';
//import {MockBaseService} from '../../shared/mocks/mock.base';
//import {MockRouter} from '../../shared/mocks/mock.router';
//import {MockToast} from "../../shared/mocks/mock.toast";
//import {ChangePasswordComponent} from "../user-change-password/user-change-password.component";

describe("Consumerapp Test Case", function () {

    class MockActivatedRoute { }
    beforeEach(() => {
        TestBed.configureTestingModule({
            providers: [
                { provide: ActivatedRoute, useClass: MockActivatedRoute },
                { provide: Router, useClass: MockRouter },
                { provide: TestConnection, useClass: TestConnection },
                { provide: UserService, useClass: MockUserService },
                { provide: Md2Toast, useClass: MockToast },
                { provide: MockBaseService, useClass: MockBaseService }]
        });
    });

    it("This is a spec with expectations", function () {
        var expectedValue = 12;
        var actualValue = 12;
        expect(expectedValue).toBe(actualValue);
    });
});



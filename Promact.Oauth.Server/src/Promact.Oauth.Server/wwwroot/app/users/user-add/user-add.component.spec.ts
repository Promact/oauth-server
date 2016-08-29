declare var describe, it, beforeEach, expect;
import {async, inject, TestBed, ComponentFixture, TestComponentBuilder} from '@angular/core/testing';
import {provide} from "@angular/core";
import {UserAddComponent} from "../user-add/user-add.component";
import { DeprecatedFormsModule } from '@angular/common';
import {UserService} from "../user.service";
import {UserModel} from '../../users/user.model';
import {ROUTER_DIRECTIVES, Router, ActivatedRoute} from '@angular/router';
import {Md2Toast} from 'md2/toast';
import {MockToast} from "../../shared/mocks/mock.toast";
import {Md2Multiselect } from 'md2/multiselect';
import {TestConnection} from "../../shared/mocks/test.connection";
import {MockUserService} from "../../shared/mocks/user/mock.user.service";
import {MockBaseService} from '../../shared/mocks/mock.base';
import {MockRouter} from '../../shared/mocks/mock.router';


describe('User Add Test', () => {
    let userAddComponent: UserAddComponent;
    
    class MockActivatedRoute { }

    beforeEach(() => {
        TestBed.configureTestingModule({
            providers: [
                { provide: ActivatedRoute, useClass: MockActivatedRoute },
                { provide: Router, useClass: MockRouter },
                { provide: TestConnection, useClass: TestConnection },
                { provide: UserService, useClass: MockUserService },
                { provide: Md2Toast, useClass: MockToast },
                { provide: MockBaseService, useClass: MockBaseService },
                { provide: UserModel, useClass: UserModel }
            ]
        });

    });
    beforeEach(inject([UserService, Router, ActivatedRoute, Md2Toast], (userService: UserService, router: Router,route: ActivatedRoute,toast: Md2Toast) => {
        //userAddComponent = new UserAddComponent(userService, router, route, toast);
    }));
    it("should be defined", () => {
        expect(userAddComponent).toBeDefined();
    });
     /**
     * should check Project name and Slack Channel Name before add
     */
    
    it("should check user first name before add", inject([UserModel], (userModel: UserModel) => {
        let expectedFristName = "First Name";
        userModel.FirstName = expectedFristName;
        userAddComponent.addUser(userModel);
        expect(userModel.FirstName).toBe(expectedFristName);
    }));

    it("should check user email before add", () => {
        let email = "test@promactinfo.com";
        let result = true;
        let method = userAddComponent.checkEmail(email);
        expect(method).not.toBe(result);
    });


});    
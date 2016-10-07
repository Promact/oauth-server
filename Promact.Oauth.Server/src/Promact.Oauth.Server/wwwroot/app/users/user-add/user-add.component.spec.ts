declare var describe, it, beforeEach, expect;
import { async, inject, TestBed, ComponentFixture } from '@angular/core/testing';
import { Provider } from "@angular/core";
import {UserAddComponent} from "../user-add/user-add.component";
import {UserService} from "../user.service";
import {UserModel} from '../../users/user.model';
import { Router, ActivatedRoute, RouterModule, Routes} from '@angular/router';
import { Md2Toast } from 'md2/toast/toast';
import {MockToast} from "../../shared/mocks/mock.toast";
import {MockUserService} from "../../shared/mocks/user/mock.user.service";
import { MockRouter } from '../../shared/mocks/mock.router';
import { UserModule } from '../user.module';
import { RouterLinkStubDirective } from '../../shared/mocks/mock.routerLink';
import { LoaderService } from '../../shared/loader.service';
let promise: TestBed;

describe('User Add Test', () => {
    let userAddComponent: UserAddComponent;

    class MockActivatedRoute { }
    class MockLoaderService { }
    const routes: Routes = [];

    beforeEach(async(() => {
        this.promise = TestBed.configureTestingModule({
            declarations: [RouterLinkStubDirective], //Declaration of mock routerLink used on page.
            imports: [UserModule, RouterModule.forRoot(routes, { useHash: true }) //Set LocationStrategy for component. 
            ],
            providers: [
                { provide: ActivatedRoute, useClass: MockActivatedRoute },
                { provide: Router, useClass: MockRouter },
                { provide: UserService, useClass: MockUserService },
                { provide: Md2Toast, useClass: MockToast },
                { provide: UserModel, useClass: UserModel }
                 { provide: LoaderService, useClass: MockLoaderService }
           }).compileComponents();

    }));
    it("should check user first name before add", done => {
        this.promise.then(() => {
            //expect(projectAddComponent).toBeDefined();
            let fixture = TestBed.createComponent(UserAddComponent); //Create instance of component            
            let userAddComponent = fixture.componentInstance;
            let expectedFristName = "First Name";
            let userModel = new UserModel();
            userModel.FirstName = expectedFristName;
            userAddComponent.addUser(userModel);
            expect(userModel.FirstName).toBe(expectedFristName);
            done();
        });
        
    });
});

//declare var describe, it, beforeEach, expect;
//import {async, inject, TestBed, ComponentFixture, TestComponentBuilder} from '@angular/core/testing';
//import {provide} from "@angular/core";
//import {UserAddComponent} from "../user-add/user-add.component";
//import { DeprecatedFormsModule } from '@angular/common';
//import {UserService} from "../user.service";
//import {UserModel} from '../../users/user.model';
//import {ROUTER_DIRECTIVES, Router, ActivatedRoute} from '@angular/router';
//import {Md2Toast} from 'md2/toast';
//import {MockToast} from "../../shared/mocks/mock.toast";
//import {Md2Multiselect } from 'md2/multiselect';
//import {TestConnection} from "../../shared/mocks/test.connection";
//import {MockUserService} from "../../shared/mocks/user/mock.user.service";
//import {MockBaseService} from '../../shared/mocks/mock.base';
//import {MockRouter} from '../../shared/mocks/mock.router';


//describe('User Add Test', () => {
//    let userAddComponent: UserAddComponent;
    
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
//                { provide: UserModel, useClass: UserModel }
//            ]
//        });

////    });
////    beforeEach(inject([UserService, Router, ActivatedRoute, Md2Toast], (userService: UserService, router: Router,route: ActivatedRoute,toast: Md2Toast) => {
////        userAddComponent = new UserAddComponent(userService, router, route, toast);
////    }));
////    it("should be defined", () => {
////        expect(userAddComponent).toBeDefined();
////    });
////    it("should check user first name before add", inject([UserModel], (userModel: UserModel) => {
////        let expectedFristName = "First Name";
////        userModel.FirstName = expectedFristName;
////        userAddComponent.addUser(userModel);
////        expect(userModel.FirstName).toBe(expectedFristName);
////    }));
////    it("should check user email before add", () => {
////        let email = "test@promactinfo.com";
////        let result = true;
////        let method = userAddComponent.checkEmail(email);
////        expect(method).not.toBe(result);
////    });
////});    
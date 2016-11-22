﻿declare var describe, it, beforeEach, expect;
import { async, inject, TestBed, ComponentFixture } from '@angular/core/testing';
import { Provider } from "@angular/core";
import { UserAddComponent } from "../user-add/user-add.component";
import { UserService } from "../user.service";
import { UserModel } from '../../users/user.model';
import { Router, ActivatedRoute, RouterModule, Routes } from '@angular/router';
import { Md2Toast } from 'md2';
import { MockToast } from "../../shared/mocks/mock.toast";
import { MockUserService } from "../../shared/mocks/user/mock.user.service";
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
                { provide: UserModel, useClass: UserModel },
                { provide: LoaderService, useClass: MockLoaderService }]
        }).compileComponents();

    }));
    it("should check user added successfully", done => {
        this.promise.then(() => {
            let fixture = TestBed.createComponent(UserAddComponent); //Create instance of component            
            let userAddComponent = fixture.componentInstance;
            let userModel = new UserModel();
            let expected = "Ankit";
            userModel.FirstName = expected;
            userAddComponent.addUser(userModel);
            expect(userModel.FirstName).toBe(expected);
            done();
        });

    });

    it("should check user not added successfully", done => {
        this.promise.then(() => {
            let fixture = TestBed.createComponent(UserAddComponent); //Create instance of component            
            let userAddComponent = fixture.componentInstance;
            let userModel = new UserModel();
            let expected = "ankit@promactinfo.com";
            userModel.FirstName = "Ankit"
            userModel.Email = expected;
            userAddComponent.addUser(userModel)
            expect(userModel.Email).toBe(expected);
            done();
        });

    });

});


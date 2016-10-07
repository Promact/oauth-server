declare var describe, it, beforeEach, expect;
import {async, inject, TestBed, ComponentFixture} from '@angular/core/testing';
import { By } from "@angular/platform-browser";
import { Provider } from "@angular/core";
import { UserService }   from '../user.service';
import {UserModel} from '../user.model';
import { Router, ActivatedRoute, RouterModule, Routes } from '@angular/router';
import { Component } from '@angular/core';
import {Md2Toast} from 'md2/toast';
import {UserListComponent} from '../user-list/user-list.component';
import {MockToast} from "../../shared/mocks/mock.toast";
import {MockUserService} from "../../shared/mocks/user/mock.user.service";
import { UserModule } from '../user.module';
import { LoaderService } from '../../shared/loader.service';
import { MockRouter } from '../../shared/mocks/mock.router';
import { RouterLinkStubDirective } from '../../shared/mocks/mock.routerLink';
import { Observable } from 'rxjs/Observable';


let comp: UserListComponent;
let fixture: ComponentFixture<UserListComponent>;

let promise: TestBed;

describe("User List Test", () => {
     
  
    class MockLoaderService { }
    const routes: Routes = [];
    beforeEach(async(() => {
        this.promise = TestBed.configureTestingModule({
            declarations: [RouterLinkStubDirective], //Declaration of mock routerLink used on page.
            imports: [UserModule, RouterModule.forRoot(routes, { useHash: true }) //Set LocationStrategy for component. 
            ],
            providers: [
                { provide: UserService, useClass: MockUserService },
                { provide: Router, useClass: MockRouter },
                { provide: Md2Toast, useClass: MockToast },
                { provide: LoaderService, useClass: MockLoaderService }

            ]
        }).compileComponents();
    }));


    it("should get default Project for company", done => {
        this.promise.then(() => {
            let fixture = TestBed.createComponent(UserListComponent); //Create instance of component            
            let userListComponent = fixture.componentInstance;
            expect(userListComponent.getUsers());
            done();
        });
    });
});




////declare var describe, it, beforeEach, expect;
////import {async, inject, TestBed, ComponentFixture} from '@angular/core/testing';
////import {provide} from "@angular/core";
////import { UserService }   from '../user.service';
////import {UserModel} from '../user.model';
////import { Component } from '@angular/core';
////import { DeprecatedFormsModule } from '@angular/common';
////import {ROUTER_DIRECTIVES, Router } from '@angular/router';
////import {Md2Toast} from 'md2/toast';
////import {UserListComponent} from '../user-list/user-list.Component';
////import {MockToast} from "../../shared/mocks/mock.toast";
////import {TestConnection} from "../../shared/mocks/test.connection";
////import {MockUserService} from "../../shared/mocks/user/mock.user.service";
////import {MockBaseService} from '../../shared/mocks/mock.base';

//describe("Project List Test", () => {
//    let userListComponent: UserListComponent;
//    class MockRouter { }
//    beforeEach(() => {
//        TestBed.configureTestingModule({
//            providers: [
//                provide(Router, { useClass: MockRouter }),
//                provide(TestConnection, { useClass: TestConnection }),
//                provide(UserService, { useClass: MockUserService }),
//                provide(Md2Toast, { useClass: MockToast }),
//                provide(MockBaseService, { useClass: MockBaseService})
//            ]
//        });

//    });

//    beforeEach(inject([UserService, Router], (userService: UserService, router: Router) => {
//        userListComponent = new UserListComponent(userService,router);
//    }));
//    it("should be defined", () => {
//        expect(userListComponent).toBeDefined();
//    });
//     /**
//     * get user of Projects
//     */
//    it("should get list of Project on initialization", () => {
//        userListComponent.ngOnInit();
//        expect(userListComponent.getUsers());
//    });

//});

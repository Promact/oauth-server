//declare var describe, it, beforeEach, expect;
//import {async, inject, TestBed, ComponentFixture} from '@angular/core/testing';
//import {provide} from "@angular/core";
//import { UserService }   from '../user.service';
//import {UserModel} from '../user.model';
//import { Component } from '@angular/core';
//import { DeprecatedFormsModule } from '@angular/common';
//import {ROUTER_DIRECTIVES, Router } from '@angular/router';
//import {Md2Toast} from 'md2/toast';
//import {UserListComponent} from '../user-list/user-list.component';
//import {MockToast} from "../../shared/mocks/mock.toast";
//import {TestConnection} from "../../shared/mocks/test.connection";
//import {MockUserService} from "../../shared/mocks/user/mock.user.service";
//import {MockBaseService} from '../../shared/mocks/mock.base';

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

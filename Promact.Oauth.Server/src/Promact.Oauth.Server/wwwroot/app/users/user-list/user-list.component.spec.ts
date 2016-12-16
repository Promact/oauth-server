declare var describe, it, beforeEach, expect;
import {async, inject, TestBed, ComponentFixture} from '@angular/core/testing';
import { By } from "@angular/platform-browser";
import { Provider } from "@angular/core";
import { UserService }   from '../user.service';
import {UserModel} from '../user.model';
import { Router, ActivatedRoute, RouterModule, Routes } from '@angular/router';
import { Component } from '@angular/core';
import { Md2Toast } from 'md2';
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

describe("User List Test", () => {
    class MockLoaderService { }
    const routes: Routes = [];
    beforeEach(async(() => {
        TestBed.configureTestingModule({
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


    it("should get default Users for company", () => {
            let fixture = TestBed.createComponent(UserListComponent); //Create instance of component            
            let userListComponent = fixture.componentInstance;
            expect(userListComponent.getUsers());
      });
});





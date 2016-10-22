declare var describe, it, beforeEach, expect;
import { async, inject, TestBed, ComponentFixture } from '@angular/core/testing';
import { Router, ActivatedRoute, RouterModule, Routes } from '@angular/router';
import { Provider } from "@angular/core";
import { PasswordModel } from "../users/user-password.model";
import { ChangePasswordModule } from "../change-password/change-password.module";
import { ChangePasswordComponent } from "../change-password/change-password.component";
import { TestConnection } from "../shared/mocks/test.connection";
import { UserService } from '../users/user.service';
import { MockUserService } from "../shared/mocks/user/mock.user.service";
import { Md2Toast } from 'md2/toast/toast';
import { MockToast } from "../shared/mocks/mock.toast";
import { Observable } from "rxjs/Observable";
import {  RouterLinkStubDirective } from "../shared/mocks/mock.routerLink";
import { LoaderService } from "../shared/loader.service";

let promise: TestBed;

describe('Change Password', () => {
    class MockRouter { }
    class MockLoaderService { }
    const routes: Routes = [];

    beforeEach(async(() => {
        this.promise = TestBed.configureTestingModule({
            declarations: [RouterLinkStubDirective], //Declaration of mock routerLink used on page.
            imports: [ChangePasswordModule, RouterModule.forRoot(routes, { useHash: true }) //Set LocationStrategy for component. 
            ],
            providers: [
                { provide: Router, useClass: MockRouter },
                { provide: UserService, useClass: MockUserService },
                { provide: Md2Toast, useClass: MockToast },
                { provide: PasswordModel, useClass: PasswordModel },
                { provide: LoaderService, useClass: MockLoaderService }
            ]
        }).compileComponents();
    }));
    
    it("Change Password", () => done => {
        this.promise.then(() => {
            let fixture = TestBed.createComponent(ChangePasswordComponent); //Create instance of component            
            let changePasswordComponent = fixture.componentInstance;
            let passwordModel = new PasswordModel();
            passwordModel.NewPassword = "test123";
            passwordModel.OldPassword = "test";
            passwordModel.ConfirmPassword = "test123";
            passwordModel.Email = "test@yahoo.com";
            let result = changePasswordComponent.changePassword(passwordModel);
            expect(result).toBe(true);
            done();
        });
    });
});




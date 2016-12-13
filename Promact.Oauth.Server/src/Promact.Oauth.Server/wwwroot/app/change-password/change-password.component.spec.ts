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
import { Md2Toast } from 'md2';
import { MockToast } from "../shared/mocks/mock.toast";
import { Observable } from "rxjs/Observable";
import { RouterLinkStubDirective } from "../shared/mocks/mock.routerLink";
import { LoaderService } from "../shared/loader.service";
import { MockRouter } from '../shared/mocks/mock.router';
import { StringConstant } from '../shared/stringconstant';

let promise: TestBed;
let stringConstant = new StringConstant();

describe('Change Password', () => {
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
                { provide: LoaderService, useClass: LoaderService },
                { provide: StringConstant, useClass: StringConstant }
            ]
        }).compileComponents();
    }));

    it("should be defined ChangePasswordComponent", () => done => {
        let fixture = TestBed.createComponent(ChangePasswordComponent);
        let changePasswordComponent = fixture.componentInstance;
        expect(changePasswordComponent).toBeDefined();
    });


    it("Change Password", () => done => {
        this.promise.then(() => {
            let fixture = TestBed.createComponent(ChangePasswordComponent); //Create instance of component            
            let changePasswordComponent = fixture.componentInstance;
            let passwordModel = new PasswordModel();
            passwordModel.NewPassword = stringConstant.newPassword;
            passwordModel.OldPassword = stringConstant.oldPassword;
            passwordModel.ConfirmPassword = stringConstant.newPassword;
            passwordModel.Email = stringConstant.email;
            let result = changePasswordComponent.changePassword(passwordModel);
            expect(result).toBe(passwordModel.NewPassword);
            done();
        });
    });

    it("Check Old Password", () => done => {
        this.promise.then(() => {
            let fixture = TestBed.createComponent(ChangePasswordComponent); //Create instance of component            
            let changePasswordComponent = fixture.componentInstance;
            let passwordModel = new PasswordModel();
            let result = changePasswordComponent.checkOldPasswordIsValid();
            expect(result).toBe(true);
            done();
        });
    });

    it("Match Password", () => done => {
        this.promise.then(() => {
            let fixture = TestBed.createComponent(ChangePasswordComponent); //Create instance of component            
            let changePasswordComponent = fixture.componentInstance;
            let passwordModel = new PasswordModel();
            passwordModel.NewPassword = stringConstant.newPassword;
            passwordModel.ConfirmPassword = stringConstant.newPassword;
            let result = changePasswordComponent.matchPassword(passwordModel.ConfirmPassword, passwordModel.NewPassword);
            expect(result).toBe(true);
            done();
        });
    });

    it("Password Does Not Match", () => done => {
        this.promise.then(() => {
            let fixture = TestBed.createComponent(ChangePasswordComponent); //Create instance of component            
            let changePasswordComponent = fixture.componentInstance;
            let passwordModel = new PasswordModel();
            passwordModel.NewPassword = stringConstant.newPassword;
            passwordModel.ConfirmPassword = stringConstant.confirmPassword;
            let result = changePasswordComponent.matchPassword(passwordModel.ConfirmPassword, passwordModel.NewPassword);
            expect(result).toBe(false);
            done();
        });
    });
   
});




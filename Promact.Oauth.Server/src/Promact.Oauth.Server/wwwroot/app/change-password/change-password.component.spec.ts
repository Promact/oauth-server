declare var describe, it, beforeEach, expect, spyOn;
import { async, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { Router, RouterModule, Routes } from '@angular/router';
import { PasswordModel } from "../users/user-password.model";
import { ChangePasswordModule } from "../change-password/change-password.module";
import { ChangePasswordComponent } from "../change-password/change-password.component";
import { UserService } from '../users/user.service';
import { MockUserService } from "../shared/mocks/user/mock.user.service";
import { Md2Toast } from 'md2';
import { MockToast } from "../shared/mocks/mock.toast";
import { LoaderService } from "../shared/loader.service";
import { StringConstant } from '../shared/stringconstant';

let stringConstant = new StringConstant();


describe('Change Password', () => {
    const routes: Routes = [];

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            imports: [ChangePasswordModule, RouterModule.forRoot(routes, { useHash: true }) //Set LocationStrategy for component. 
            ],
            providers: [
                { provide: UserService, useClass: MockUserService },
                { provide: Md2Toast, useClass: MockToast },
                { provide: PasswordModel, useClass: PasswordModel },
                { provide: LoaderService, useClass: LoaderService }
            ]
        }).compileComponents();
    }));


    it("Load Change-password Component", () => {
        let fixture = TestBed.createComponent(ChangePasswordComponent);
        let changePasswordComponent = fixture.componentInstance;
        expect(changePasswordComponent).toBeDefined();
    });


    it("Did not Change Password ", fakeAsync(() => {
        let fixture = TestBed.createComponent(ChangePasswordComponent); //Create instance of component            
        let changePasswordComponent = fixture.componentInstance;
        let passwordModel = new PasswordModel();
        let expectedNewPassword = stringConstant.newPassword;
        passwordModel.NewPassword = expectedNewPassword;
        passwordModel.OldPassword = stringConstant.oldPassword;
        passwordModel.ConfirmPassword = stringConstant.newPassword;
        passwordModel.Email = stringConstant.email;
        let result = changePasswordComponent.changePassword(passwordModel);
        tick();
        expect(expectedNewPassword).toBe(passwordModel.NewPassword);
    }));


    it("Change Password", fakeAsync(() => {
        let fixture = TestBed.createComponent(ChangePasswordComponent); //Create instance of component            
        let changePasswordComponent = fixture.componentInstance;
        let userService = fixture.debugElement.injector.get(UserService);
        let mockPasswordModel = new MockPasswordModel();
        mockPasswordModel.errorMessage = null;
        spyOn(userService, stringConstant.changePassword).and.returnValue((Promise.resolve(mockPasswordModel)));
        let router = fixture.debugElement.injector.get(Router);
        spyOn(router, stringConstant.navigate);
        let passwordModel = new PasswordModel();
        passwordModel.NewPassword = stringConstant.newPassword;
        passwordModel.OldPassword = "";
        passwordModel.ConfirmPassword = stringConstant.email;
        passwordModel.Email = stringConstant.email;
        let result = changePasswordComponent.changePassword(passwordModel);
        tick();
        expect(router.navigate).toHaveBeenCalled();
    }));


    it("Password could not be changed", fakeAsync(() => {
        let fixture = TestBed.createComponent(ChangePasswordComponent); //Create instance of component            
        let changePasswordComponent = fixture.componentInstance;
        let userService = fixture.debugElement.injector.get(UserService);
        spyOn(userService, stringConstant.changePassword).and.returnValue(Promise.reject(""));
        let passwordModel = new PasswordModel();
        passwordModel.NewPassword = stringConstant.newPassword;
        passwordModel.OldPassword = "";
        passwordModel.ConfirmPassword = stringConstant.email;
        passwordModel.Email = stringConstant.email;
        let result = changePasswordComponent.changePassword(passwordModel);
        tick();
        expect(stringConstant.newPassword).toBe(passwordModel.NewPassword);
    }));


    it("Check Old Password", fakeAsync(() => {
        let fixture = TestBed.createComponent(ChangePasswordComponent); //Create instance of component            
        let changePasswordComponent = fixture.componentInstance;
        let passwordModel = new PasswordModel();
        let result = changePasswordComponent.checkOldPasswordIsValid();
        tick();
        expect(changePasswordComponent.isInCorrect).toBe(false);
    }));


    it("Old Password is empty", fakeAsync(() => {
        let fixture = TestBed.createComponent(ChangePasswordComponent); //Create instance of component     
        let changePasswordComponent = fixture.componentInstance;
        let userService = fixture.debugElement.injector.get(UserService);
        spyOn(userService, stringConstant.checkOldPasswordIsValid).and.returnValue(Promise.reject(""));
        changePasswordComponent.checkOldPasswordIsValid();
        tick();
        expect(changePasswordComponent).toBeDefined();
    }));


    it("Match Password", fakeAsync(() => {
        let fixture = TestBed.createComponent(ChangePasswordComponent); //Create instance of component            
        let changePasswordComponent = fixture.componentInstance;
        let passwordModel = new PasswordModel();
        passwordModel.NewPassword = stringConstant.newPassword;
        passwordModel.ConfirmPassword = stringConstant.newPassword;
        let result = changePasswordComponent.matchPassword(passwordModel.ConfirmPassword, passwordModel.NewPassword);
        expect(changePasswordComponent.isNotMatch).toBe(false);
    }));


    it("Password Does Not Match", fakeAsync(() => {
        let fixture = TestBed.createComponent(ChangePasswordComponent); //Create instance of component            
        let changePasswordComponent = fixture.componentInstance;
        let passwordModel = new PasswordModel();
        passwordModel.NewPassword = stringConstant.newPassword;
        passwordModel.ConfirmPassword = stringConstant.confirmPassword;
        let result = changePasswordComponent.matchPassword(passwordModel.ConfirmPassword, passwordModel.NewPassword);
        expect(changePasswordComponent.isNotMatch).toBe(true);
    }));


    it('Calls goBack', fakeAsync(() => {
        let fixture = TestBed.createComponent(ChangePasswordComponent); //Create instance of component            
        let changePasswordComponent = fixture.componentInstance;
        let router = fixture.debugElement.injector.get(Router);
        spyOn(router, stringConstant.navigate);
        changePasswordComponent.goBack();
        tick();
        expect(router.navigate).toHaveBeenCalled();
    }));

});





class MockPasswordModel extends PasswordModel {

    constructor() {
        super();
    }
    errorMessage: string;
}

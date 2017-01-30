declare var describe, it, beforeEach, expect;
import { async,  TestBed } from '@angular/core/testing';
import { RouterModule, Routes } from '@angular/router';
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
                { provide: LoaderService, useClass: LoaderService }]
        }).compileComponents();
    }));

    it("Load Change-password Component", () => {
        let fixture = TestBed.createComponent(ChangePasswordComponent);
        let comp = fixture.componentInstance;
    });

    it("Change Password", () => {
        let fixture = TestBed.createComponent(ChangePasswordComponent); //Create instance of component            
        let changePasswordComponent = fixture.componentInstance;
        let passwordModel = new PasswordModel();
        let expectedNewPassword = stringConstant.newPassword;
        passwordModel.NewPassword = expectedNewPassword;
        passwordModel.OldPassword = stringConstant.oldPassword;
        passwordModel.ConfirmPassword = stringConstant.newPassword;
        passwordModel.Email = stringConstant.email;
        let result = changePasswordComponent.changePassword(passwordModel);
        expect(expectedNewPassword).toBe(passwordModel.NewPassword);
    });

    it("Check Old Password", () => {
            let fixture = TestBed.createComponent(ChangePasswordComponent); //Create instance of component            
            let changePasswordComponent = fixture.componentInstance;
            let passwordModel = new PasswordModel();
            let result = changePasswordComponent.checkOldPasswordIsValid();
            expect(changePasswordComponent.isInCorrect).toBe(true);
    });

    it("Match Password", () => {
            let fixture = TestBed.createComponent(ChangePasswordComponent); //Create instance of component            
            let changePasswordComponent = fixture.componentInstance;
            let passwordModel = new PasswordModel();
            passwordModel.NewPassword = stringConstant.newPassword;
            passwordModel.ConfirmPassword = stringConstant.newPassword;
            let result = changePasswordComponent.matchPassword(passwordModel.ConfirmPassword, passwordModel.NewPassword);
            expect(changePasswordComponent.isNotMatch).toBe(false);
    });

    it("Password Does Not Match", () => {
            let fixture = TestBed.createComponent(ChangePasswordComponent); //Create instance of component            
            let changePasswordComponent = fixture.componentInstance;
            let passwordModel = new PasswordModel();
            passwordModel.NewPassword = stringConstant.newPassword;
            passwordModel.ConfirmPassword = stringConstant.confirmPassword;
            let result = changePasswordComponent.matchPassword(passwordModel.ConfirmPassword, passwordModel.NewPassword);
            expect(changePasswordComponent.isNotMatch).toBe(true);
    });
});




declare var describe, it, beforeEach, expect;
import { async, inject, TestBed, ComponentFixture } from '@angular/core/testing';
import { Provider } from "@angular/core";
import { UserService } from "../users/user.service";
import { MockUserService } from "../shared/mocks/user/mock.user.service";
import { PasswordModel } from "../users/user-password.model";
import { Router} from '@angular/router';
import { ChangePasswordComponent } from "../change-password/change-password.component";

//let fixture: ComponentFixture<ChangePasswordComponent>;
//let comp: ChangePasswordComponent;

//class RouterStub {
//  //  navigateByUrl(url: string) { return url; }
//}
describe('User Change Password Test', () => {
    //let changePasswordComponent: ChangePasswordComponent;
    //beforeEach(async(() => {
    //    TestBed.configureTestingModule({
    //        declarations: [ChangePasswordComponent],
    //        providers: [{ provide: UserService, useClass: MockUserService },
    //            { provide: Router, useClass: RouterStub }   ]
    //    }).compileComponents().then(() => {
    //        fixture = TestBed.createComponent(ChangePasswordComponent);
    //        comp = fixture.componentInstance;
    //    });
        
    //}));

    //it("should be defined", () => {
    //     expect(ChangePasswordComponent).toBeDefined();});


       

    //beforeEach(inject([UserService, Router, ActivatedRoute, /*Md2Toast*/], (userService: UserService, router: Router, route: ActivatedRoute, /*toast: Md2Toast*/) => {
    //     changePasswordComponent = new ChangePasswordComponent(userService, router, route, /*toast*/);
    //}));

    it("contains spec with an expectation", function () {
        expect(1+1).toBe(2);
    });

    //it("should check password before change", inject([PasswordModel], (passwordModel: PasswordModel) => {
    //    passwordModel.NewPassword = "test123";
    //    passwordModel.OldPassword = "test";
    //    passwordModel.ConfirmPassword = "test123";
    //    passwordModel.Email = "test@yahoo.com";
    //    var result = true;
    //    var method = changePasswordComponent.changePassword(passwordModel);
    //    expect(method).not.toBe(result);
    //}));

}); 
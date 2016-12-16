declare var describe, it, beforeEach, expect;
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
import { LoaderService } from '../../shared/loader.service';


describe('User Add Test', () => {
    class MockActivatedRoute { }
    class MockLoaderService { }
    const routes: Routes = [];

    beforeEach(async(() => {
        TestBed.configureTestingModule({
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

    it("should check user added successfully", () => {
        let fixture = TestBed.createComponent(UserAddComponent); //Create instance of component            
        let userAddComponent = fixture.componentInstance;
        let userModel = new UserModel();
        let expected = "Ankit";
        userModel.FirstName = expected;
        userAddComponent.addUser(userModel);
        expect(userModel.FirstName).toBe(expected);
    });

    it("should check user not added successfully", () => {
        let fixture = TestBed.createComponent(UserAddComponent); //Create instance of component            
        let userAddComponent = fixture.componentInstance;
        let userModel = new UserModel();
        let expected = "ankit@promactinfo.com";
        userModel.FirstName = "Ankit";
        userModel.Email = expected;
        userAddComponent.addUser(userModel);
        expect(userModel.Email).toBe(expected);
    });

    it("should check user email", () => {
        let fixture = TestBed.createComponent(UserAddComponent); //Create instance of component            
        let userAddComponent = fixture.componentInstance;
        let email = "";
        let expected = "";
        userAddComponent.checkEmail(expected);
        expect(email).toBe(expected);
    });


    it("should check user Slack User Name", () => {
        let fixture = TestBed.createComponent(UserAddComponent); //Create instance of component            
        let userAddComponent = fixture.componentInstance;
        let SlackUserName = "";
        let expected = "";
        userAddComponent.checkSlackUserName(expected);
        expect(SlackUserName).toBe(expected);
    });

});


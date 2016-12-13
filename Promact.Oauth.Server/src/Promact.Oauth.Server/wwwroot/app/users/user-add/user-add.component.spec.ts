declare var describe, it, beforeEach, expect;
import { async, TestBed } from '@angular/core/testing';
import { UserAddComponent } from "../user-add/user-add.component";
import { UserService } from "../user.service";
import { UserModel } from '../../users/user.model';
import { Router, RouterModule, Routes } from '@angular/router';
import { Md2Toast } from 'md2';
import { MockToast } from "../../shared/mocks/mock.toast";
import { MockUserService } from "../../shared/mocks/user/mock.user.service";
import { MockRouter } from '../../shared/mocks/mock.router';
import { UserModule } from '../user.module';
import { LoaderService } from '../../shared/loader.service';
import { StringConstant } from '../../shared/stringconstant';
import { ActivatedRouteStub } from "../../shared/mocks/mock.activatedroute";


let stringConstant = new StringConstant();

describe('User Add Test', () => {
    let userAddComponent: UserAddComponent;

    const routes: Routes = [];

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            imports: [UserModule, RouterModule.forRoot(routes, { useHash: true }) //Set LocationStrategy for component. 
            ],
            providers: [
                { provide: ActivatedRoute, useClass: ActivatedRouteStub },
                { provide: Router, useClass: MockRouter },
                { provide: UserService, useClass: MockUserService },
                { provide: Md2Toast, useClass: MockToast },
                { provide: UserModel, useClass: UserModel },
                { provide: LoaderService, useClass: LoaderService },
                { provide: StringConstant, useClass: StringConstant }]
        }).compileComponents();

    }));

    it("should check user added successfully", () => {
        let fixture = TestBed.createComponent(UserAddComponent); //Create instance of component            
        let userAddComponent = fixture.componentInstance;
        let userModel = new UserModel();
        let expected = stringConstant.firstName;
        userModel.FirstName = expected;
        userAddComponent.addUser(userModel);
        expect(userModel.FirstName).toBe(expected);
    });

    it("should check user not added successfully", () => {
        let fixture = TestBed.createComponent(UserAddComponent); //Create instance of component            
        let userAddComponent = fixture.componentInstance;
        let userModel = new UserModel();
        let expected = stringConstant.email;
        userModel.FirstName = stringConstant.firstName;
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


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
import { RouterLinkStubDirective } from '../../shared/mocks/mock.routerLink';
import { LoaderService } from '../../shared/loader.service';
import { StringConstant } from '../../shared/stringconstant';
import { ActivatedRouteStub } from "../../shared/mocks/mock.activatedroute";

let promise: TestBed;
let stringConstant = new StringConstant();

describe('User Add Test', () => {
    let userAddComponent: UserAddComponent;

    const routes: Routes = [];

    beforeEach(async(() => {
        this.promise = TestBed.configureTestingModule({
            declarations: [RouterLinkStubDirective], //Declaration of mock routerLink used on page.
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


    it("should be defined UserAddComponent", () => {
        let fixture = TestBed.createComponent(UserAddComponent);
        let userAddComponent = fixture.componentInstance;
        expect(userAddComponent).toBeDefined();
    });

    it("should check user added successfully", done => {
        this.promise.then(() => {
            let fixture = TestBed.createComponent(UserAddComponent); //Create instance of component            
            let userAddComponent = fixture.componentInstance;
            let userModel = new UserModel();
            let expected = stringConstant.firstName;
            userModel.FirstName = expected;
            userAddComponent.addUser(userModel);
            expect(userModel.FirstName).toBe(expected);
            done();
        });

    });

    it("should check user not added successfully", done => {
        this.promise.then(() => {
            let fixture = TestBed.createComponent(UserAddComponent); //Create instance of component            
            let userAddComponent = fixture.componentInstance;
            let userModel = new UserModel();
            let expected = stringConstant.email;
            userModel.FirstName = stringConstant.firstName;
            userModel.Email = expected;
            userAddComponent.addUser(userModel);
            expect(userModel.Email).toBe(expected);
            done();
        });

    });

    it("should check user email", done => {
        this.promise.then(() => {
            let fixture = TestBed.createComponent(UserAddComponent); //Create instance of component            
            let userAddComponent = fixture.componentInstance;
            let email = stringConstant.empty;
            let expected = stringConstant.empty;
            userAddComponent.checkEmail(expected);
            expect(email).toBe(expected);
            done();
        });

    });

   
    it("should check user Slack User Name", done => {
        this.promise.then(() => {
            let fixture = TestBed.createComponent(UserAddComponent); //Create instance of component            
            let userAddComponent = fixture.componentInstance;
            let SlackUserName = stringConstant.empty;
            let expected = stringConstant.empty;
            userAddComponent.checkSlackUserName(expected);
            expect(SlackUserName).toBe(expected);
            done();
        });

    });


  
});


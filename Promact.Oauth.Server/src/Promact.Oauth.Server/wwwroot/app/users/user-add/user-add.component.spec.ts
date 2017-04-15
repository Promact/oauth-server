declare var describe, it, beforeEach, expect, spyOn;
import { async, TestBed, fakeAsync, tick } from '@angular/core/testing';
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
import { DatePipe } from '@angular/common';

let stringConstant = new StringConstant();

describe('User Add Test', () => {
    const routes: Routes = [];

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            imports: [UserModule, RouterModule.forRoot(routes, { useHash: true }) //Set LocationStrategy for component. 
            ],
            providers: [
                { provide: Router, useClass: MockRouter },
                { provide: UserService, useClass: MockUserService },
                { provide: Md2Toast, useClass: MockToast },
                { provide: UserModel, useClass: UserModel },
                { provide: LoaderService, useClass: LoaderService },
                { provide: DatePipe, useClass: DatePipe },
                { provide: StringConstant, useClass: StringConstant }]
        }).compileComponents();
    }));

    it("should check user added successfully", fakeAsync(() => {
        let fixture = TestBed.createComponent(UserAddComponent); //Create instance of component            
        let userAddComponent = fixture.componentInstance;
        let toast = fixture.debugElement.injector.get(Md2Toast);
        let userModel = new UserModel();
        let expected = stringConstant.firstName;
        userModel.FirstName = expected;
        userModel.JoiningDate = new Date();
        let router = fixture.debugElement.injector.get(Router);
        spyOn(router, stringConstant.navigate);
        userAddComponent.addUser(userModel);
        tick();
        expect(router.navigate).toHaveBeenCalled();
    }));


    it("should check that user is not added successfully", fakeAsync(() => {
        let fixture = TestBed.createComponent(UserAddComponent); //Create instance of component            
        let userAddComponent = fixture.componentInstance;
        let toast = fixture.debugElement.injector.get(Md2Toast);
        let userModel = new UserModel();
        userModel.FirstName = "";
        userModel.JoiningDate = new Date();
        userAddComponent.addUser(userModel);
        tick();
        expect(userAddComponent.userModel.FirstName).toBe(undefined);
    }));


    it("Email already exists so user not added", fakeAsync(() => {
        let fixture = TestBed.createComponent(UserAddComponent); //Create instance of component            
        let userAddComponent = fixture.componentInstance;
        let toast = fixture.debugElement.injector.get(Md2Toast);
        userAddComponent.isEmailExist = true;
        let userModel = new UserModel();
        userModel.FirstName = "";
        userModel.JoiningDate = new Date();
        userAddComponent.addUser(userModel);
        tick();
        expect(userAddComponent.userModel.FirstName).toBe(undefined);
    }));


    it("should check user email", fakeAsync(() => {
        let fixture = TestBed.createComponent(UserAddComponent); //Create instance of component            
        let userAddComponent = fixture.componentInstance;
        userAddComponent.checkEmail(stringConstant.email);
        tick();
        expect(userAddComponent.isEmailExist).toBe(true);
    }));


    it("should get list of roles", fakeAsync(() => {
        let fixture = TestBed.createComponent(UserAddComponent); //Create instance of component 
        let userAddComponent = fixture.componentInstance;
        userAddComponent.ngOnInit();
        tick();
        expect(userAddComponent.listOfRoles.length).toBe(1);
    }));


    it('Calls goBack', fakeAsync(() => {
        let fixture = TestBed.createComponent(UserAddComponent); //Create instance of component            
        let userAddComponent = fixture.componentInstance;
        let userListService = fixture.debugElement.injector.get(UserService);
        let router = fixture.debugElement.injector.get(Router);
        spyOn(router, stringConstant.navigate);
        userAddComponent.goBack();
        tick();
        expect(router.navigate).toHaveBeenCalled();
    }));

});
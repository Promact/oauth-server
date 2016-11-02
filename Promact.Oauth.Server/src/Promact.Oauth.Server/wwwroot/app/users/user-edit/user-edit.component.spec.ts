declare var describe, it, beforeEach, expect;
import { async, inject, TestBed, ComponentFixture } from '@angular/core/testing';
import { Provider } from "@angular/core";
import { UserModel } from '../../users/user.model';
import { UserEditComponent } from "../user-edit/user-edit.component";
import { UserService } from "../user.service";
import { UserModule } from '../user.module';
import { Router, ActivatedRoute, RouterModule, Routes } from '@angular/router';
import { Md2Toast } from 'md2';
import { MockToast } from "../../shared/mocks/mock.toast";
import { Md2Multiselect } from 'md2/multiselect';
import { MockUserService } from "../../shared/mocks/user/mock.user.service";
import { MockRouter } from '../../shared/mocks/mock.router';
import { Observable } from 'rxjs/Observable';
import { RouterLinkStubDirective } from '../../shared/mocks/mock.routerLink';
import { LoaderService } from '../../shared/loader.service';
import { Location } from "@angular/common";   
import { LocationStrategy } from "@angular/common";
import { LoginService } from '../../login.service';
import { MockLoginService } from "../../shared/mocks/mock.login.service";
import { ActivatedRouteStub } from "../../shared/mocks/mock.activatedroute";

describe("User Edit Test", () => {
    let userEditComponent: UserEditComponent;
    let userService: UserService;
    //class MockActivatedRoute { }
   
    class MockLocation { }
    class MockLoaderService { }
    class McokLogin { }
    const routes: Routes = [];
    beforeEach(async(() => {
        this.promise = TestBed.configureTestingModule({
            declarations: [RouterLinkStubDirective], //Declaration of mock routerLink used on page.
            imports: [UserModule, RouterModule.forRoot(routes, { useHash: true }) //Set LocationStrategy for component. 
            ],
            providers: [
                { provide: UserService, useClass: MockUserService },
                { provide: ActivatedRoute, useClass: ActivatedRouteStub },
                { provide: Router, useClass: MockRouter },
                { provide: Md2Toast, useClass: MockToast },
                { provide: UserModel, useClass: UserModel },
                { provide: LoaderService, useClass: MockLoaderService },
                { provide: LoginService, useClass: MockLoginService },
                { provide: Location, useClass: MockLocation }
            ]
        }).compileComponents();
       
    }));



    it("should get particular user details", done => {
        this.promise.then(() => {
            let fixture = TestBed.createComponent(UserEditComponent); //Create instance of component     
            let activatedRoute = fixture.debugElement.injector.get(ActivatedRoute);
            activatedRoute.testParams = { id: "1" };        
            let userEditComponent = fixture.componentInstance;
            let expectedFirstName = "First Name"
            
            userEditComponent.ngOnInit();
            expect(userEditComponent.user.FirstName).toBe(expectedFirstName);
            done();
        });
    });


    it("should check User first name before update", done => {
        this.promise.then(() => {
            let fixture = TestBed.createComponent(UserEditComponent); //Create instance of component            
            let userEditComponent = fixture.componentInstance;
            let expectedFirstName = "First Name";
            let userModel = new UserModel();
            userModel.FirstName = expectedFirstName;
            userEditComponent.editUser(userModel);
            expect(userModel.FirstName).toBe(expectedFirstName);
            done();
        });
       

    });

    

});
declare var describe, it, beforeEach, expect;
import { async, inject, TestBed, ComponentFixture } from '@angular/core/testing';
import { Provider } from "@angular/core";
import { UserModel } from '../../users/user.model';
import { UserDetailsComponent } from "../user-details/user-details.component";
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
import { LoginService } from '../../login.service';
import { MockLoginService } from "../../shared/mocks/mock.login.service";
import { ActivatedRouteStub } from "../../shared/mocks/mock.activatedroute";
import { UserRole } from "../../shared/userrole.model";
import { StringConstant } from '../../shared/stringconstant';


let stringConstant = new StringConstant();

describe("User Details Test", () => {
    let userService: UserService;
    
    const routes: Routes = [];
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            imports: [UserModule, RouterModule.forRoot(routes, { useHash: true }) //Set LocationStrategy for component. 
            ],
            providers: [
                { provide: UserService, useClass: MockUserService },
                { provide: ActivatedRoute, useClass: ActivatedRouteStub },
                { provide: Router, useClass: MockRouter },
                { provide: Md2Toast, useClass: MockToast },
                { provide: UserModel, useClass: UserModel },
                { provide: LoaderService, useClass: LoaderService },
                { provide: LoginService, useClass: MockLoginService },
                { provide: Location, useClass: MockLocation },
                { provide: UserRole, useValue: new UserRole() }

            ]
        }).compileComponents();

    }));

    it("should be defined UserDetailsComponent", () => {
        let fixture = TestBed.createComponent(UserDetailsComponent);
        let userDetailsComponent = fixture.componentInstance;
        expect(userDetailsComponent).toBeDefined();
    });


    it("should get default Project for company", () => {
        let fixture = TestBed.createComponent(UserDetailsComponent); //Create instance of component
        let activatedRoute = fixture.debugElement.injector.get(ActivatedRoute);
        activatedRoute.testParams = { id: "1" };
        let userDetailsComponent: UserDetailsComponent = fixture.componentInstance;
        let expectedFirstName = "First Name";
        fixture.detectChanges();
        expect(userDetailsComponent.user.FirstName).toBe(expectedFirstName);
    });
});
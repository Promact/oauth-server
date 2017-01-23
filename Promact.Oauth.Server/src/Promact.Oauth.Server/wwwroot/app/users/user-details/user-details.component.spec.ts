declare var describe, it, beforeEach, expect;
import { async,  TestBed} from '@angular/core/testing';
import { UserModel } from '../../users/user.model';
import { UserDetailsComponent } from "../user-details/user-details.component";
import { UserService } from "../user.service";
import { UserModule } from '../user.module';
import { Router,ActivatedRoute, RouterModule, Routes } from '@angular/router';
import { Md2Toast } from 'md2';
import { MockToast } from "../../shared/mocks/mock.toast";
import { MockUserService } from "../../shared/mocks/user/mock.user.service";
import { LoaderService } from '../../shared/loader.service';
import { ActivatedRouteStub } from "../../shared/mocks/mock.activatedroute";
import { UserRole } from "../../shared/userrole.model";
import { StringConstant } from '../../shared/stringconstant';
import { MockRouter } from '../../shared/mocks/mock.router';

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
                { provide: Router, useClass: MockRouter },
                { provide: ActivatedRoute, useClass: ActivatedRouteStub },
                { provide: Md2Toast, useClass: MockToast },
                { provide: UserModel, useClass: UserModel },
                { provide: LoaderService, useClass: LoaderService },
                { provide: UserRole, useValue: new UserRole()},
                { provide: StringConstant, useClass: StringConstant }
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
        activatedRoute.testParams = { id: stringConstant.id };
        let userDetailsComponent: UserDetailsComponent = fixture.componentInstance;
        let expectedFirstName = stringConstant.testfirstName;
        fixture.detectChanges();
        expect(userDetailsComponent.user.FirstName).toBe(expectedFirstName);
    });
});
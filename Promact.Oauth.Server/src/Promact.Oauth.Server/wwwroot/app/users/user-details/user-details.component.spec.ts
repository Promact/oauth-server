declare var describe, it, beforeEach, expect;
import {addProviders, async, inject, TestBed, ComponentFixture, TestComponentBuilder} from '@angular/core/testing';
import {provide} from "@angular/core";
import {Location} from "@angular/common";
import {LocationStrategy} from "@angular/common";
import {UserDetailsComponent} from "../user-details/user-details.component";
import {UserService} from "../user.service";
import {UserModel} from '../../users/user.model';
import {ROUTER_DIRECTIVES, Router, ActivatedRoute } from '@angular/router';
import {Md2Toast} from 'md2/toast';
import {MockToast} from "../../shared/mocks/mock.toast";
import {Md2Multiselect } from 'md2/multiselect';
import {TestConnection} from "../../shared/mocks/test.connection";
import {MockUserService} from "../../shared/mocks/user/mock.user.service";
import {MockBaseService} from '../../shared/mocks/mock.base';
import {MockRouter} from '../../shared/mocks/mock.router';

describe("Project Edit Test", () => {
    let userDetailsComponent: UserDetailsComponent;
    let userService: UserService;
    class MockActivatedRoute { }
    class MockLocation { }
    beforeEach(() => {
        TestBed.configureTestingModule({
            providers: [
                provide(ActivatedRoute, { useClass: MockActivatedRoute }),
                provide(Router, { useClass: MockRouter }),
                provide(TestConnection, { useClass: TestConnection }),
                provide(UserService, { useClass: MockUserService }),
                provide(Md2Toast, { useClass: MockToast }),
                provide(MockBaseService, { useClass: MockBaseService }),
                provide(UserModel, { useClass: UserModel }),
                provide(Location, { useClass: MockLocation })
            ]
        });
    });
    beforeEach(inject([UserService, ActivatedRoute, Router], (userService: UserService, route: ActivatedRoute, router: Router) => {
        userDetailsComponent = new UserDetailsComponent(userService, route, router);
    }));
    it("should be defined", () => {
        expect(userDetailsComponent).toBeDefined();
    });
    

});
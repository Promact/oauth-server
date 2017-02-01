declare var describe, it, beforeEach, expect;
import { async, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { ConsumerAppModel } from "../consumerapp-model";
import { ConsumerappEditComponent } from "../consumerapp-edit/consumer-edit.component";
import { ConsumerAppService } from "../consumerapp.service";
import { Router, ActivatedRoute, RouterModule, Routes } from '@angular/router';
import { Md2Toast } from 'md2';
import { MockToast } from "../../shared/mocks/mock.toast";
import { MockConsumerappService } from "../../shared/mocks/consumerapp/mock.consumerapp.service";
import { ConsumerAppModule } from '../consumerapp.module';
import { LoaderService } from '../../shared/loader.service';
import { ActivatedRouteStub } from "../../shared/mocks/mock.activatedroute";
import { StringConstant } from '../../shared/stringconstant';
import { MockRouter } from '../../shared/mocks/mock.router';

let stringConstant = new StringConstant();

describe('Consumer Edit Test', () => {
    const routes: Routes = [];
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            imports: [ConsumerAppModule, RouterModule.forRoot(routes, { useHash: true }) //Set LocationStrategy for component. 
            ],
            providers: [
                { provide: Router, useClass: MockRouter },
                { provide: ActivatedRoute, useClass: ActivatedRouteStub },
                { provide: ConsumerAppService, useClass: MockConsumerappService },
                { provide: Md2Toast, useClass: MockToast },
                { provide: ConsumerAppModel, useClass: ConsumerAppModel },
                { provide: LoaderService, useClass: LoaderService },
                { provide: StringConstant, useClass: StringConstant },
            ]
        }).compileComponents();
    }));

    it("Get consumerApp by id", fakeAsync(() => {
        let fixture = TestBed.createComponent(ConsumerappEditComponent); //Create instance of component            
        let activatedRoute = fixture.debugElement.injector.get(ActivatedRoute);
        activatedRoute.testParams = { id: stringConstant.id };
        let projectEditComponent = fixture.componentInstance;
        projectEditComponent.ngOnInit();
        tick();
        expect(projectEditComponent.consumerModel).not.toBeNull();
    }));

    it("Edit consumer app", fakeAsync(() => {
        let fixture = TestBed.createComponent(ConsumerappEditComponent); //Create instance of component            
        let consumerappEditComponent = fixture.componentInstance;
        let consumerAppModel = new ConsumerAppModel();
        let expectedconsumerappname = stringConstant.consumerappname;
        consumerAppModel.Name = expectedconsumerappname;
        consumerAppModel.CallbackUrl = stringConstant.callbackUrl;
        consumerAppModel.AuthSecret = stringConstant.authSecret;
        consumerAppModel.AuthId = stringConstant.authId;
        consumerAppModel.Id = 1;
        consumerAppModel.LogoutUrl = stringConstant.loginUrl;
        consumerAppModel.Scopes = [];
        consumerappEditComponent.updateApps(consumerAppModel);
        tick();
        expect(consumerAppModel.Scopes.length).toBe(0);
    }));

    it("Random number Edit consumer app AuthId", fakeAsync(() => {
        let fixture = TestBed.createComponent(ConsumerappEditComponent); //Create instance of component            
        let consumerappEditComponent = fixture.componentInstance;
        let toast = fixture.debugElement.injector.get(Md2Toast);
        let expectedValue = stringConstant.consumerAppExpectedValue;
        let consumerAppModel = new ConsumerAppModel();
        consumerappEditComponent.getRandomNumber(true);
        tick();
        expect(consumerappEditComponent.consumerModel.AuthId).toBe(expectedValue);
    }));

    it("Random number Edit consumer app AuthSecret", fakeAsync(() => {
        let fixture = TestBed.createComponent(ConsumerappEditComponent); //Create instance of component            
        let consumerappEditComponent = fixture.componentInstance;
        let toast = fixture.debugElement.injector.get(Md2Toast);
        let expectedValue = stringConstant.consumerAppExpectedValue;
        let consumerAppModel = new ConsumerAppModel();
        consumerappEditComponent.getRandomNumber(false);
        tick();
        expect(consumerappEditComponent.consumerModel.AuthSecret).toBe(expectedValue);
    }));
});






declare var describe, it, beforeEach, expect;
import { async, TestBed } from '@angular/core/testing';
import { ConsumerAppModel } from "../consumerapp-model";
import { ConsumerappEditComponent } from "../consumerapp-edit/consumer-edit.component";
import { ConsumerAppService } from "../consumerapp.service";
import { ActivatedRoute, RouterModule, Routes } from '@angular/router';
import { Md2Toast } from 'md2';
import { MockToast } from "../../shared/mocks/mock.toast";
import { MockConsumerappService } from "../../shared/mocks/consumerapp/mock.consumerapp.service";
import { ConsumerAppModule } from '../consumerapp.module';
import { LoaderService } from '../../shared/loader.service';
import { ActivatedRouteStub } from "../../shared/mocks/mock.activatedroute";
import { StringConstant } from '../../shared/stringconstant';

let stringConstant = new StringConstant();
describe('Consumer Edit Test', () => {
    const routes: Routes = [];
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            imports: [ConsumerAppModule, RouterModule.forRoot(routes, { useHash: true }) //Set LocationStrategy for component. 
            ],
            providers: [
                { provide: ActivatedRoute, useClass: ActivatedRouteStub },
                { provide: ConsumerAppModel, useClass: ConsumerAppModel },
                { provide: StringConstant, useClass: StringConstant },
                { provide: LoaderService, useClass: LoaderService },
                { provide: ConsumerAppService, useClass: MockConsumerappService },
                { provide: Md2Toast, useClass: MockToast },
            ]
        }).compileComponents();
    }));

    it("Get consumerApp by id", () => {
            let fixture = TestBed.createComponent(ConsumerappEditComponent); //Create instance of component            
            let activatedRoute = fixture.debugElement.injector.get(ActivatedRoute);
            activatedRoute.testParams = { id: stringConstant.id };
            let consumerappEditComponent = fixture.componentInstance;
            consumerappEditComponent.ngOnInit();
            expect(consumerappEditComponent.consumerModel).not.toBeNull();
    });

    it("Edit consumer app", () => {
            let fixture = TestBed.createComponent(ConsumerappEditComponent); //Create instance of component            
            let consumerappEditComponent = fixture.componentInstance;
            let toast = fixture.debugElement.injector.get(Md2Toast);
            let consumerAppModel = new ConsumerAppModel();
            let expectedconsumerappname = stringConstant.consumerappname;
            consumerAppModel.Name = expectedconsumerappname;
            consumerAppModel.CallbackUrl = stringConstant.callbackUrl;
            consumerAppModel.AuthSecret = stringConstant.authSecret;
            consumerAppModel.AuthId = stringConstant.authId;
            consumerAppModel.Id = 1;
            consumerAppModel.LogoutUrl = "www.google.com";
            consumerAppModel.Scopes = [];
            consumerappEditComponent.updateApps(consumerAppModel);
            expect(consumerAppModel.Scopes.length).toBe(0);
    });

    it("Random number Edit consumer app AuthId", () => {
        let fixture = TestBed.createComponent(ConsumerappEditComponent); //Create instance of component            
        let consumerappEditComponent = fixture.componentInstance;
        let toast = fixture.debugElement.injector.get(Md2Toast);
        let expectedValue = "SFDASFADSFSAD";
        let consumerAppModel = new ConsumerAppModel();
        consumerappEditComponent.getRandomNumber(true);
        expect(consumerappEditComponent.consumerModel.AuthId).toBe(expectedValue);
    });

    it("Random number Edit consumer app AuthSecret", () => {
        let fixture = TestBed.createComponent(ConsumerappEditComponent); //Create instance of component            
        let consumerappEditComponent = fixture.componentInstance;
        let toast = fixture.debugElement.injector.get(Md2Toast);
        let expectedValue = "SFDASFADSFSAD";
        let consumerAppModel = new ConsumerAppModel();
        consumerappEditComponent.getRandomNumber(false);
        expect(consumerappEditComponent.consumerModel.AuthSecret).toBe(expectedValue);
    });
});






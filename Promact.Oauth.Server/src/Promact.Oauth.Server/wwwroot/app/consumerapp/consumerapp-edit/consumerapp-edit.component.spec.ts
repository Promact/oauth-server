declare var describe, it, beforeEach, expect;
import { async, inject, TestBed, ComponentFixture } from '@angular/core/testing';
import { Provider } from "@angular/core";
import { ConsumerAppModel } from "../consumerapp-model";
import { ConsumerappEditComponent } from "../consumerapp-edit/consumer-edit.component";
import { ConsumerAppService } from "../consumerapp.service";
import { Router, ActivatedRoute, RouterModule, Routes } from '@angular/router';
import { Md2Toast } from 'md2';
import { MockToast } from "../../shared/mocks/mock.toast";
import { MockConsumerappService } from "../../shared/mocks/consumerapp/mock.consumerapp.service";
import { MockRouter } from '../../shared/mocks/mock.router';
import { Observable } from 'rxjs/Observable';
import { ConsumerAppModule } from '../consumerapp.module';
import { LoaderService } from '../../shared/loader.service';
import { ActivatedRouteStub } from "../../shared/mocks/mock.activatedroute";
import { StringConstant } from '../../shared/stringconstant';



describe('Consumer Edit Test', () => {
    const routes: Routes = [];
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            imports: [ConsumerAppModule, RouterModule.forRoot(routes, { useHash: true }) //Set LocationStrategy for component. 
            ],
            providers: [
                { provide: ActivatedRoute, useClass: ActivatedRouteStub },
                { provide: Router, useClass: MockRouter },
                { provide: ConsumerAppService, useClass: MockConsumerappService },
                { provide: Md2Toast, useClass: MockToast },
                { provide: ConsumerAppModel, useClass: ConsumerAppModel },
                { provide: LoaderService, useClass: LoaderService },
                { provide: StringConstant, useClass: StringConstant }
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
            consumerAppModel.Description = stringConstant.description;
            consumerAppModel.CallbackUrl = stringConstant.callbackUrl;
            consumerAppModel.AuthSecret = stringConstant.authSecret;
            consumerAppModel.AuthId = stringConstant.authId;
            consumerappEditComponent.updateApps(consumerAppModel);
            expect(consumerAppModel.Name).toBe(expectedconsumerappname);
    });

});






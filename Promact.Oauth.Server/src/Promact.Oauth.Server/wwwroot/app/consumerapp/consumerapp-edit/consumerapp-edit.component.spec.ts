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



describe('Consumer Edit Test', () => {
    class MockRouter { }
    class MockLoaderService { }
    const routes: Routes = [];
    class MockActivatedRoute extends ActivatedRoute {
        constructor() {
            super();
            this.params = Observable.of({ id: "1" });
        }
    }

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            imports: [ConsumerAppModule, RouterModule.forRoot(routes, { useHash: true }) //Set LocationStrategy for component. 
            ],
            providers: [
                { provide: ActivatedRoute, useClass: MockActivatedRoute },
                { provide: Router, useClass: MockRouter },
                { provide: ConsumerAppService, useClass: MockConsumerappService },
                { provide: Md2Toast, useClass: MockToast },
                { provide: ConsumerAppModel, useClass: ConsumerAppModel },
                { provide: LoaderService, useClass: MockLoaderService }
            ]
        }).compileComponents();
    }));

    it("Get consumerApp by id", () => {
            let fixture = TestBed.createComponent(ConsumerappEditComponent); //Create instance of component            
            let consumerappEditComponent = fixture.componentInstance;
            consumerappEditComponent.ngOnInit();
            expect(consumerappEditComponent.consumerModel).not.toBeNull();
    });

    it("Edit consumer app", () => {
            let fixture = TestBed.createComponent(ConsumerappEditComponent); //Create instance of component            
            let consumerappEditComponent = fixture.componentInstance;
            let toast = fixture.debugElement.injector.get(Md2Toast);
            let consumerAppModel = new ConsumerAppModel();
            let expectedconsumerappname = "slack";
            consumerAppModel.Name = expectedconsumerappname;
            consumerAppModel.CallbackUrl = "www.google.com";
            consumerAppModel.AuthSecret = "dsdsdsdsdsdsd";
            consumerAppModel.AuthId = "ASASs5454545455";
            consumerAppModel.Id = 1;
            consumerAppModel.LogoutUrl = "www.google.com";
            consumerAppModel.Scopes = [];
            consumerappEditComponent.updateApps(consumerAppModel);
            expect(consumerappEditComponent.consumerModel.Scopes.length).toBe(0);
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






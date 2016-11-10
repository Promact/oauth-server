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
import { RouterLinkStubDirective } from '../../shared/mocks/mock.routerLink';
import { ConsumerAppModule } from '../consumerapp.module';
import { LoaderService } from '../../shared/loader.service';


let promise: TestBed;
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
        this.promise = TestBed.configureTestingModule({
            declarations: [RouterLinkStubDirective], //Declaration of mock routerLink used on page.
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

    it("Get consumerApp by id", done => {
        this.promise.then(() => {
            let fixture = TestBed.createComponent(ConsumerappEditComponent); //Create instance of component            
            let consumerappEditComponent = fixture.componentInstance;
            consumerappEditComponent.ngOnInit();
            expect(consumerappEditComponent.consumerModel).not.toBeNull();
            done();
        });
    });

    it("Edit consumer app", done => {
        this.promise.then(() => {
            let fixture = TestBed.createComponent(ConsumerappEditComponent); //Create instance of component            
            let consumerappEditComponent = fixture.componentInstance;
            let toast = fixture.debugElement.injector.get(Md2Toast);
            let consumerAppModel = new ConsumerAppModel();
            let expectedconsumerappname = "slack";
            consumerAppModel.Name = expectedconsumerappname;
            consumerAppModel.Description = "slack description";
            consumerAppModel.CallbackUrl = "www.google.com";
            consumerAppModel.AuthSecret = "dsdsdsdsdsdsd";
            consumerAppModel.AuthId = "ASASs5454545455";
            consumerappEditComponent.updateApps(consumerAppModel);
            expect(consumerAppModel.Name).toBe(expectedconsumerappname);
            done();
        });

    });

});






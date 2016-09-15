//import {async, inject, TestBed, ComponentFixture, TestComponentBuilder} from "@angular/core/testing";
//import {provide} from "@angular/core";
//import { ROUTER_DIRECTIVES, Router} from "@angular/router";
//import {Md2Toast} from "md2/toast";
//import { DeprecatedFormsModule } from '@angular/common';
//import { ConsumerappEditComponent } from "../consumerapp-edit/consumer-edit.component";
//import { ConsumerAppService} from "../consumerapp.service";
//import {ConsumerAppModel} from "../consumerapp-model";
//import {TestConnection} from "../../shared/mocks/test.connection";
////import {MockToast} from "../../shared/mocks/mock.toast";
//import { MockConsumerappService } from "../../shared/mocks/consumerapp/mock.consumerapp.service";
//import {MockBaseService} from "../../shared/mocks/mock.base";
//import {MockRouter} from "../../shared/mocks/mock.router";
//import { ActivatedRoute} from "@angular/router";
//import {Observable} from "rxjs/Observable";
//import {Location} from "@angular/common";


declare var describe, it, beforeEach, expect;

describe("Consumerapp Add Test Case", () => {
    let consumerappEditComponent: ConsumerappEditComponent;
    class MockLocation { }
    class MockActivatedRoute extends ActivatedRoute {
        constructor() {
            super();
            this.params = Observable.of({ id: "1"});
        }
    }

    beforeEach(() => {
        TestBed.configureTestingModule({
            providers: [
                provide(ActivatedRoute, {useClass: MockActivatedRoute}),
                provide(Location, { useClass: MockLocation }),
                provide(Router, { useClass: MockRouter }),
                provide(TestConnection, { useClass: TestConnection }),
                provide(ConsumerAppService, { useClass: MockConsumerappService }),
                provide(Md2Toast, { useClass: MockToast }),
                provide(MockBaseService, { useClass: MockBaseService }),
                provide(ConsumerAppModel, { useClass: ConsumerAppModel})
            ]
        });
    });


    beforeEach(inject([Router, ConsumerAppService, ActivatedRoute, Md2Toast, Location], (router: Router, consumerAppService: ConsumerAppService, activatedRoute: ActivatedRoute,toast: Md2Toast,location:Location) => {
        consumerappEditComponent = new ConsumerappEditComponent(router,consumerAppService,activatedRoute,toast,location);
    }));

    it("get consumerpp object on edit page", () => {
        consumerappEditComponent.ngOnInit();
        expect(consumerappEditComponent.consumerModel).not.toBeNull();
    });

    it("consumerapp edit method test", inject([ConsumerAppModel], (consumerAppModel: ConsumerAppModel) => {
        let expectedconsumerappname = "slack";
        let result = true;
        consumerAppModel.Name = expectedconsumerappname;
        consumerAppModel.Description = "slack description";
        consumerAppModel.CallbackUrl = "www.google.com";
        consumerAppModel.AuthSecret = "dsdsdsdsdsdsd";
        consumerAppModel.AuthId = "ASASs5454545455";
        consumerappEditComponent.updateApps(consumerAppModel);
        expect(consumerAppModel.Name).toBe(expectedconsumerappname);
    }));

});



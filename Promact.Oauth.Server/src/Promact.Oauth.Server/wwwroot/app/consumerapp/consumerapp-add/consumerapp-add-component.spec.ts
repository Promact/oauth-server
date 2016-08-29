import {async, inject, TestBed, ComponentFixture, TestComponentBuilder} from "@angular/core/testing";
import {provide} from "@angular/core";
import { DeprecatedFormsModule } from "@angular/common";
import { ROUTER_DIRECTIVES, Router} from "@angular/router";
import {Md2Toast} from "md2/toast";
import { ConsumerappAddComponent } from "../consumerapp-add/consumerapp-add.component";
import { ConsumerAppService} from "../consumerapp.service";
import {ConsumerAppModel} from "../consumerapp-model";
import {TestConnection} from "../../shared/mocks/test.connection";
import {MockToast} from "../../shared/mocks/mock.toast";
import { MockConsumerappService } from "../../shared/mocks/consumerapp/mock.consumerapp.service";
import {MockBaseService} from "../../shared/mocks/mock.base";
import {MockRouter} from "../../shared/mocks/mock.router";


describe("Consumerapp Add Test Case", () => {
    let consumerappAddComponent: ConsumerappAddComponent;
  
    beforeEach(() => {
        TestBed.configureTestingModule({
            providers: [
                provide(Router, { useClass: MockRouter }),
                provide(TestConnection, {useClass : TestConnection }),
                provide(ConsumerAppService, { useClass: MockConsumerappService }),
                provide(Md2Toast, { useClass: MockToast }),
                provide(MockBaseService, { useClass: MockBaseService }),
                provide(ConsumerAppModel, { useClass: ConsumerAppModel })
            ]
        });
    });


    beforeEach(inject([ConsumerAppService, Router, Md2Toast], (consumerAppService: ConsumerAppService, router: Router, toast: Md2Toast) => {
        consumerappAddComponent = new ConsumerappAddComponent(consumerAppService, router, toast);
    }));

    //it("consumer test", () => {
    //    expect(1+1).toEqual(2);
    //});

    it("consumerapp add method test", inject([ConsumerAppModel], (consumerAppModel: ConsumerAppModel) => {
        let expectedconsumerappname = "slack";
        let result = true;
        consumerAppModel.Name = expectedconsumerappname;
        consumerAppModel.Description = "slack description";
        consumerAppModel.CallbackUrl = "www.google.com";
        consumerAppModel.AuthSecret = "dsdsdsdsdsdsd";
        consumerAppModel.AuthId = "ASASs5454545455";
        let method = consumerappAddComponent.submitApps(consumerAppModel);
        expect(method).toEqual(result);
    }));
});



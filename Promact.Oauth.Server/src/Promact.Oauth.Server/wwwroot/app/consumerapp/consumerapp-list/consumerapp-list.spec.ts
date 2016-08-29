import {async, inject, TestBed, ComponentFixture, TestComponentBuilder} from '@angular/core/testing';
import {provide} from "@angular/core";
import { DeprecatedFormsModule } from "@angular/common";
import { ROUTER_DIRECTIVES, Router} from "@angular/router";
import { ConsumerappListComponent } from "../consumerapp-list/consumerapp-list.component";
import { ConsumerAppService} from "../consumerapp.service";
import {TestConnection} from "../../shared/mocks/test.connection";
import { MockConsumerappService } from "../../shared/mocks/consumerapp/mock.consumerapp.service";
import {MockBaseService} from "../../shared/mocks/mock.base";
import {ConsumerAppModel} from '../consumerapp-model';


describe('Consumerapp List Test Case', () => {
    let consumerappListComponent: ConsumerappListComponent;
    class MockRouter { }

    beforeEach(() => {
        TestBed.configureTestingModule({
            providers: [
               provide(Router, { useClass: MockRouter }),
               provide(TestConnection, { useClass: TestConnection }),
               provide(ConsumerAppService, { useClass: MockConsumerappService }),
               provide(MockBaseService, { useClass: MockBaseService }),
               provide(ConsumerAppModel, { useClass: ConsumerAppModel })
            ]
        });
    });

    beforeEach(inject([Router, ConsumerAppService], (router: Router, consumerAppService: ConsumerAppService) => {
        consumerappListComponent = new ConsumerappListComponent(router,consumerAppService);
    }));




    it("consumer test", () => {
        let listOfConsumrApp = consumerappListComponent.getConsumerApps();
        expect(consumerappListComponent.listOfConsumerApps.length).toEqual(1);
    });

});


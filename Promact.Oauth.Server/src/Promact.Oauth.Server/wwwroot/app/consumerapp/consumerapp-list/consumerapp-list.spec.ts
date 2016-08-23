//import {async, inject, TestBed, ComponentFixture, TestComponentBuilder} from '@angular/core/testing';
//import {provide} from "@angular/core";
//import { DeprecatedFormsModule } from "@angular/common";
//import { ROUTER_DIRECTIVES, Router} from "@angular/router";
//import { ConsumerappListComponent } from "../consumerapp-list/consumerapp-list.component";
//import { ConsumerAppService} from '../consumerapp.service';
//import {TestConnection} from "../../shared/mocks/test.connection";
//import { MockConsumerappService } from "../../shared/mocks/consumerapp/mock.consumerapp.service";
//import {MockBaseService} from '../../shared/mocks/mock.base';


//describe('Consumerapp List Test Case', () => {
//    let consumerappListComponent: ConsumerappListComponent;
//    class MockRouter { }

//    beforeEach(() => {
//        TestBed.configureTestingModule({
//            providers: [
//                { provide: ConsumerAppService, userClass: MockConsumerappService },
//                { provide: Router, userClass: MockRouter },
//                { provide: TestConnection, useClass: TestConnection },
//                { provide: MockBaseService, useClass: MockBaseService }
//            ]
//        });
//    });
//    beforeEach(inject([Router, ConsumerAppService], (router: Router, proService: ConsumerAppService) => {
//        consumerappListComponent = new ConsumerappListComponent(router, proService);
//    }));

//    //beforeEach(inject([Router, ConsumerAppService], (router: Router, consumerAppService: ConsumerAppService) => {
//    //    consumerappListComponent = new ConsumerappListComponent(router,consumerAppService);
//    //}));


//    it("consumer test", () => {
//        let listOfConsumrApp = consumerappListComponent.ngOnInit();
//        expect(consumerappListComponent.listOfConsumerApps.length).toEqual(1);
//    });

//});


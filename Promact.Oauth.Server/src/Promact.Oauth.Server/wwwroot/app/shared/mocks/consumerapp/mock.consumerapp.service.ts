//import {TestConnection} from "../test.connection";
//import {Injectable} from '@angular/core';
//import {ResponseOptions, Response} from "@angular/http";
////import {Md2Toast} from 'md2/toast';
//import {Subject} from 'rxjs/Rx';
//import {MockBaseService} from '../mock.base';
//import {ConsumerAppModel} from "../../../consumerapp/consumerapp-model";

//@Injectable()
//export class MockConsumerappService {
//    private consumerAppUrl = 'api/consumerapp';
//    constructor(private mockBaseService: MockBaseService) {

//    }

//    /*This method used for call the method for add new aaps*
//    * /
//    * @param consumerAppsAc
//    */
//    addConsumerApps(consumerAppModel: ConsumerAppModel) {
//        let result = true;
//        let connection = this.mockBaseService.getMockResponse(this.consumerAppUrl, result);
//        return connection;
//    }

//    /*This method used for get consumer apps list*
//     * 
//     */
//    getConsumerApps() {
//        let listOfConsumerApp = new Array<MockConsumers>();
//        let mockConsumerApp = new MockConsumers();
//        mockConsumerApp.Name = "slack";
//        mockConsumerApp.Description = "slack Description";
//        mockConsumerApp.CallbackUrl = "www.google.com";
//        listOfConsumerApp.push(mockConsumerApp);
//        let connection = this.mockBaseService.getMockResponse(this.consumerAppUrl, listOfConsumerApp);
//        return connection;
//    }

//    /*This method used for get consumer app object by id.*
//     * 
//     * @param id 
//     */
//    getConsumerAppById(id: number) {
//        let mockConsumerApp = new MockConsumer(id);
//        if (id == 1) {
//            mockConsumerApp.Name = "slack";
//            mockConsumerApp.Description = "slack Description";
//            mockConsumerApp.CallbackUrl = "www.google.com";
//        }
//        let connection = this.mockBaseService.getMockResponse(this.consumerAppUrl + id, mockConsumerApp);
//        return connection;
//    }

//    /*This method used for update consumer apps*
//     * /
//     * @param consumerAppsAc
//     */
//    updateConsumerApps(consumerAppModel: ConsumerAppModel) {
//        let connection = this.mockBaseService.getMockResponse(this.consumerAppUrl, consumerAppModel);
//        return connection;
//    }
//}

//class MockConsumers extends ConsumerAppModel {

//    constructor() {
//        super();
//    }

//}

//class MockConsumer extends ConsumerAppModel {
//    constructor(id: number) {
//        super();
//        this.Id = id;
//    }
//}

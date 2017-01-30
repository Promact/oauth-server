﻿import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { ConsumerAppModel } from "../../../consumerapp/consumerapp-model";
import { StringConstant } from '../../stringconstant';

@Injectable()
export class MockConsumerappService {
    stringConstant: StringConstant = new StringConstant();
    constructor() {
    }

    /*This method used for call the method for add new aaps*
    * /
    * @param consumerAppsAc
    */
    addConsumerApps(consumerAppModel: ConsumerAppModel) {
        consumerAppModel.Id = 1;
        return new BehaviorSubject(consumerAppModel).asObservable();
    }

    /*This method used for get consumer apps list*
     * 
     */
    getConsumerApps() {
        let listOfConsumerApp = new Array<MockConsumers>();
        let mockConsumerApp = new MockConsumers();
        mockConsumerApp.Name = this.stringConstant.consumerappname;
       // mockConsumerApp.Description = this.stringConstant.description;
        mockConsumerApp.CallbackUrl = this.stringConstant.callbackUrl;
        listOfConsumerApp.push(mockConsumerApp);
        return new BehaviorSubject(listOfConsumerApp).asObservable();
    }

    /*This method used for get consumer app object by id.*
     * 
     * @param id 
     */
    getConsumerAppById(id: string) {
        let mockConsumerApp = new MockConsumer(id);
        if (id === "slack") {
            mockConsumerApp.Name = this.stringConstant.consumerappname;
            //mockConsumerApp.Description = this.stringConstant.description;
            mockConsumerApp.CallbackUrl = this.stringConstant.callbackUrl;
        }
        return new BehaviorSubject(mockConsumerApp).asObservable();
    }

    /*This method used for update consumer apps*
     * /
     * @param consumerAppsAc
     */
    updateConsumerApps(consumerAppModel: ConsumerAppModel) {
        return new BehaviorSubject(consumerAppModel).asObservable();
    }

    /*This method used for get consumer apps list*
    * @param isAuthId
    */
    getRandomNumber(isAuthId: boolean) {

        return new BehaviorSubject("SFDASFADSFSAD").asObservable();
    }
}

class MockConsumers extends ConsumerAppModel {

    constructor() {
        super();
    }

}

class MockConsumer extends ConsumerAppModel {
    constructor(id: string) {
        super();
        this.AuthId = id;
    }
}

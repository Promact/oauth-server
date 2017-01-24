import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { ConsumerAppModel } from "../../../consumerapp/consumerapp-model";





@Injectable()
export class MockConsumerappService {
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
        mockConsumerApp.Name = "slack";
        //mockConsumerApp.Description = "slack Description";
        mockConsumerApp.CallbackUrl = "www.google.com";
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
            mockConsumerApp.Name = "slack";
            //mockConsumerApp.Description = "slack Description";
            mockConsumerApp.CallbackUrl = "www.google.com";
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

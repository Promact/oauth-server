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
        return Promise.resolve(consumerAppModel);
    }

    /*This method used for get consumer apps list*
      *
      */
    getConsumerApps() {
        let listOfConsumerApp = new Array<MockConsumers>();
        let mockConsumerApp = new MockConsumers();
        mockConsumerApp.Name = "slack";
        mockConsumerApp.CallbackUrl = "www.google.com";
        listOfConsumerApp.push(mockConsumerApp);
        return Promise.resolve(listOfConsumerApp);
    }

    /*This method used for get consumer app object by id.*
      *
      * @param id
      */
    getConsumerAppById(id: string) {
        let mockConsumerApp = new MockConsumer(id);
        if (id === "slack") {
            mockConsumerApp.Name = "slack";
            mockConsumerApp.CallbackUrl = "www.google.com";
        }
        return Promise.resolve(mockConsumerApp);
    }

    /*This method used for update consumer apps*
      * /
      * @param consumerAppsAc
      */
    updateConsumerApps(consumerAppModel: ConsumerAppModel) {
        return Promise.resolve(consumerAppModel);
    }


    /*This method used for get consumer apps list*
   * @param isAuthId
   */
    getRandomNumber(isAuthId: boolean) {
        return Promise.resolve("SFDASFADSFSAD");
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


















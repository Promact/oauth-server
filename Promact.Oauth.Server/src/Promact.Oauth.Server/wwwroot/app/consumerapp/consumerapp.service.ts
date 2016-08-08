import { Injectable } from '@angular/core';
import {HttpService} from "../http.service";
import 'rxjs/add/operator/toPromise';
import {ConsumerAppModel} from './consumerapp-model';

@Injectable()
export class ConsumerAppService {
    private consumerAppUrl = 'api/consumerapp';  // URL to web api
    constructor(private httpService: HttpService<ConsumerAppModel>) {

    }

    /*This method used for call the method for add new aaps*
     * /
     * @param consumerAppsAc
     */
    addConsumerApps(consumerAppsAc: ConsumerAppModel) {
        return this.httpService.post(this.consumerAppUrl + "/addConsumer", consumerAppsAc);
    }

    /*This method used for get consumer apps list*
     * 
     */
    getConsumerApps() {
        return this.httpService.get(this.consumerAppUrl + "/getConsumerApps");
    }

    /*This method used for get consumer app object by id.*
     * 
     * @param id 
     */
    getConsumerAppById(id: number) {
        return this.httpService.get(this.consumerAppUrl + "/getConsumerById/" + id);

    }

    /*This method used for update consumer apps*
     * /
     * @param consumerAppsAc
     */
    updateConsumerApps(consumerAppsAc: ConsumerAppModel) {
        return this.httpService.post(this.consumerAppUrl + "/updateConsumer", consumerAppsAc);
    }
}

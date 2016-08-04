import { Injectable } from '@angular/core';
import {HttpService} from "../http.service";
import 'rxjs/add/operator/toPromise';
import {ConsumerAppModel} from './consumerapp-model';

@Injectable()
export class ConsumerAppService {
    private consumerAppUrl = 'api/consumerapp';  // URL to web api
    constructor(private httpService: HttpService<ConsumerAppModel>) {

    }

    /*This method used for call method from consumer app controller for add new aaps*
     * /
     * @param consumerAppsAc
     */
    addConsumerApps(consumerAppsAc: ConsumerAppModel) {
        return this.httpService.post(this.consumerAppUrl + "/addConsumer", consumerAppsAc);
    }

}

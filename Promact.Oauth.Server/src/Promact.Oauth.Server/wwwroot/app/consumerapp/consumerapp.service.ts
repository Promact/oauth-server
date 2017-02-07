import { Injectable } from '@angular/core';
import { Http, Headers, RequestOptions, Response } from "@angular/http";
import { ConsumerAppModel } from './consumerapp-model';
import 'rxjs/add/operator/toPromise';

@Injectable()
export class ConsumerAppService {
    private consumerAppUrl = 'api/consumerapp';  // URL to web api
    private headers = new Headers({ 'Content-Type': 'application/json' });
    constructor(private http: Http) {

    }

    /*This service used for add new consumer aap*
     * /
     * @param consumerAppsAc
     */
    addConsumerApps(consumerAppsAc: ConsumerAppModel) {
        return this.http
            .post(this.consumerAppUrl, JSON.stringify(consumerAppsAc), { headers: this.headers })
            .map(res => res.json())
            .toPromise();
    }

    /*This service used for get consumer apps list*
    *
    */
    getConsumerApps(): Promise<ConsumerAppModel[]> {
        return this.http.get(this.consumerAppUrl)
            .map(res => res.json())
            .toPromise();

    }

    /*This service used for get consumer app object by id.*
  *
  * @param id
  */
    getConsumerAppById(id: number) {
        return this.http.get(this.consumerAppUrl + "/" + id)
            .map(res => res.json())
            .toPromise();
    }

    /*This service used for update consumer apps*
    * /
    * @param consumerAppsAc
    */
    updateConsumerApps(consumerAppsAc: ConsumerAppModel) {
        return this.http.put(this.consumerAppUrl, JSON.stringify(consumerAppsAc), { headers: this.headers })
            .map(res => res.json())
            .toPromise();

    }


    /*This method used for get consumer apps list*
    * @param isAuthId
    */
    getRandomNumber(isAuthId: boolean) {
        return this.http.get(this.consumerAppUrl + "/" + isAuthId).map(res => res.text())
            .toPromise();
    }
}

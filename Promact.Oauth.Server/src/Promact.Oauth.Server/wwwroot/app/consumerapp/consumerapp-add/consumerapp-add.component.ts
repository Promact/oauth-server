import {Component} from "@angular/core";
import {ConsumerAppModel} from '../consumerapp-model';
import { Router, ROUTER_DIRECTIVES}from '@angular/router';
import { ConsumerAppService} from '../consumerapp.service';

@Component({
    templateUrl: "app/consumerapp/consumerapp-add/consumerapp-add.html",
    directives: []
})
export class ConsumerappAddComponent {
    consumerModel: ConsumerAppModel;
    constructor(private router: Router, private consumerAppService: ConsumerAppService) {
        this.consumerModel = new ConsumerAppModel();
    }

    submitApps(consumerModel) {
        this.consumerAppService.addConsumerApps(consumerModel).subscribe((consumerModel) => {

        }, err => {

            });
    }
}
import {Component} from "@angular/core";
import {ConsumerAppModel} from '../consumerapp-model';
import { ROUTER_DIRECTIVES, Router } from '@angular/router';
import { ConsumerAppService} from '../consumerapp.service';

@Component({
    templateUrl: "app/consumerapp/consumerapp-add/consumerapp-add.html",
    directives: []
})
export class ConsumerappAddComponent {
    consumerModel: ConsumerAppModel;
    constructor(private consumerAppService: ConsumerAppService,private router: Router) {
        this.consumerModel = new ConsumerAppModel();
    }

    submitApps(consumerModel) {
        this.consumerAppService.addConsumerApps(consumerModel).subscribe((result) => {
            if (result == true) {

            }
        }, err => {

        });
    }

    cancel() {
        //this.router.navigate(['/consumerapp/consumerapp']);
    }

    ngOnInit() {
        this.consumerModel = new ConsumerAppModel();
    }
}
import {Component} from "@angular/core";
import { Router, ROUTER_DIRECTIVES}from '@angular/router';
import { ConsumerAppService} from '../consumerapp.service';

@Component({
    templateUrl: "app/consumerapp/consumerapp-list/consumerapp-list.html",
    directives: []
})
export class ConsumerappListComponent {
    listOfConsumerApps: any;
    constructor(private router: Router, private consumerAppService: ConsumerAppService) {

    }

    getConsumerApps() {
        this.consumerAppService.getConsumerApps().subscribe((result) => {
            if (result.length > 0)
                this.listOfConsumerApps = result;
        }, err => {

        });
    }

    ngOnInit() {
        this.getConsumerApps();
    }

    editDetails(consumerId) {
        this.router.navigate(['/consumerapp/edit', consumerId]);
    }

    addNewApp() {
        this.router.navigate(['/consumerapp/add']);
    }
} 
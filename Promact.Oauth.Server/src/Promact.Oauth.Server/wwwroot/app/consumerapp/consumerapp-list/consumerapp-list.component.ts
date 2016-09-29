import {Component} from "@angular/core";
import { Router }from '@angular/router';
import { ConsumerAppService } from '../consumerapp.service';
import { LoaderService } from '../../shared/loader.service';

@Component({
    templateUrl: "app/consumerapp/consumerapp-list/consumerapp-list.html"
})
export class ConsumerappListComponent {
    listOfConsumerApps: any;
    constructor(private router: Router, private consumerAppService: ConsumerAppService, private loader :LoaderService) {

    }
    
    ngOnInit() {
        this.getConsumerApps();
    }

    getConsumerApps()
    {
        this.loader.loader = true;
        this.consumerAppService.getConsumerApps().subscribe((result) => {
            if (result.length > 0)
                this.listOfConsumerApps = result;
        }, err => {

        });
        this.loader.loader = false;
    }


    editDetails(consumerId) {
        this.router.navigate(['/consumerapp/edit', consumerId]);
    }

    addNewApp() {
        this.router.navigate(['/consumerapp/add']);
    }
} 
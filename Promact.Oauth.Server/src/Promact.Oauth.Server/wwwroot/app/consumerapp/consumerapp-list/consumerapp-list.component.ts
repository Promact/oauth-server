import {Component , OnInit} from "@angular/core";
import { Router }from '@angular/router';
import { ConsumerAppService } from '../consumerapp.service';
import { LoaderService } from '../../shared/loader.service';
import { Md2Toast } from 'md2';
import { ConsumerAppModel } from "../consumerapp-model";
import { StringConstant } from '../../shared/stringconstant';


@Component({
    templateUrl: "app/consumerapp/consumerapp-list/consumerapp-list.html"
})
export class ConsumerappListComponent implements OnInit {
    listOfConsumerApps: Array<ConsumerAppModel>;
    constructor(private router: Router, private consumerAppService: ConsumerAppService, private toast: Md2Toast, private loader: LoaderService) {
        this.listOfConsumerApps = new Array<ConsumerAppModel>();
    }
    
    ngOnInit() {
        this.getConsumerApps();
    }

    getConsumerApps() {
        this.loader.loader = true;
        this.consumerAppService.getConsumerApps().subscribe((result) => {
            this.listOfConsumerApps = result;
            this.loader.loader = false;
        }, err => {
            this.toast.show('Consumer App list empty.');
            this.loader.loader = false;
        });
    }


    editDetails(consumerId) {
        this.router.navigate(['/consumerapp/edit', consumerId]);
    }

    addNewApp() {
        this.router.navigate(['/consumerapp/add']);
    }
} 
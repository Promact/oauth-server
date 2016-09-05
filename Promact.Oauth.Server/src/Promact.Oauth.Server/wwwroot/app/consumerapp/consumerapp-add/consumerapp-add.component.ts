import {Component} from "@angular/core";
import {ConsumerAppModel} from '../consumerapp-model';
import { ROUTER_DIRECTIVES, Router } from '@angular/router';
import { ConsumerAppService} from '../consumerapp.service';
import {Md2Toast} from 'md2/toast';

@Component({
    templateUrl: "app/consumerapp/consumerapp-add/consumerapp-add.html",
    directives: [],
    providers: [Md2Toast]
})
export class ConsumerappAddComponent {
    consumerModel: ConsumerAppModel;
    constructor(private consumerAppService: ConsumerAppService, private router: Router ,private toast: Md2Toast) {
        this.consumerModel = new ConsumerAppModel();
    }

    submitApps(consumerModel) {
        this.consumerAppService.addConsumerApps(consumerModel).subscribe((result) => {
            if (result == true) {
                this.toast.show('Consumer App is added successfully.');
                this.cancel();
            }
            else if (result == false) {
                this.toast.show('Consumer App Name is already exists.');
            }


        }, err => {

        });
    }

    cancel() {
        this.router.navigate(['/consumerapp']);
    }

    ngOnInit() {
        this.consumerModel = new ConsumerAppModel();
    }
}
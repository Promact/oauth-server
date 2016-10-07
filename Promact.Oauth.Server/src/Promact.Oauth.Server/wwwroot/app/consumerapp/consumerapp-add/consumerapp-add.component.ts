import {Component} from "@angular/core";
import {ConsumerAppModel} from "../consumerapp-model";
import { Router } from "@angular/router";
import { ConsumerAppService} from "../consumerapp.service";
import { Md2Toast } from 'md2/toast/toast';
import { LoaderService } from '../../shared/loader.service';

@Component({
    templateUrl: "app/consumerapp/consumerapp-add/consumerapp-add.html",
})
export class ConsumerappAddComponent {
    consumerModel: ConsumerAppModel;
    constructor(private consumerAppService: ConsumerAppService, private router: Router, private toast: Md2Toast, private loader: LoaderService) {
        this.consumerModel = new ConsumerAppModel();
    }

    submitApps(consumerModel) {
        this.loader.loader = true;
        this.consumerAppService.addConsumerApps(consumerModel).subscribe((result) => {
            if (result == true) {
                this.toast.show('Consumer App is added successfully.');
                this.cancel();
            }
            else if (result == false) {
                this.toast.show('Consumer App Name is already exists.');
            }
            this.loader.loader = false;
        }, err => {

        });
    }

    cancel() {
        this.router.navigate(['/consumerapp']);
    }

   
}
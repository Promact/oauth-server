import {Component} from "@angular/core";
import {ConsumerAppModel} from '../consumerapp-model';
import { Router, ActivatedRoute } from '@angular/router';
import { ConsumerAppService} from '../consumerapp.service';
import { Md2Toast } from 'md2/toast/toast';
import {Location} from "@angular/common";
import { LoaderService } from '../../shared/loader.service';

@Component({
    templateUrl: "app/consumerapp/consumerapp-edit/consumerapp-edit.html",
})
export class ConsumerappEditComponent {
    consumerModel: ConsumerAppModel;
    private sub: any;
    constructor(private router: Router, private consumerAppService: ConsumerAppService, private route: ActivatedRoute, private toast: Md2Toast, private location: Location, private loader: LoaderService) {
        this.consumerModel = new ConsumerAppModel();
    }

    ngOnInit() {
        this.loader.loader = true;
        this.route.params.subscribe(params => {
            let id = +params['id']; // (+) converts string 'id' to a number
            this.consumerAppService.getConsumerAppById(id).subscribe((result) => {
                this.consumerModel.Name = result.name;
                this.consumerModel.Description = result.description;
                this.consumerModel.CallbackUrl = result.callbackUrl;
                this.consumerModel.Id = result.id;

                this.consumerModel.AuthId = result.authId;
                this.consumerModel.AuthSecret = result.authSecret;
                this.loader.loader = false;
            }
                , err => {

                });
        });
    }

    updateApps(consumerModel) {
        this.loader.loader = true;
        this.consumerAppService.updateConsumerApps(consumerModel).subscribe((result) => {
            if (result == true) {
                this.toast.show('Consumer App is updated successfully.');
                this.cancel();
                this.loader.loader = false;
            }
            else if (result == false) {
                this.toast.show('Consumer App Name is already exists.');
                this.loader.loader = false;
            }
        }, err => {

        });
    }

    cancel() {
        this.location.back();
    }
}
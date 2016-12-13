import { Component, OnInit } from "@angular/core";
import { ConsumerAppModel } from '../consumerapp-model';
import { Router, ActivatedRoute } from '@angular/router';
import { ConsumerAppService } from '../consumerapp.service';
import { Md2Toast } from 'md2';
import { Location } from "@angular/common";
import { LoaderService } from '../../shared/loader.service';
import { StringConstant } from '../../shared/stringconstant';

@Component({
    templateUrl: "app/consumerapp/consumerapp-edit/consumerapp-edit.html",
})
export class ConsumerappEditComponent implements OnInit {
    consumerModel: ConsumerAppModel;
    constructor(private router: Router, private consumerAppService: ConsumerAppService, private route: ActivatedRoute, private toast: Md2Toast, private location: Location,
        private loader: LoaderService, private stringConstant: StringConstant) {
        this.consumerModel = new ConsumerAppModel();
    }

    ngOnInit() {
        this.loader.loader = true;
        this.route.params.subscribe(params => {
            let id = +params[this.stringConstant.paramsId]; // (+) converts string 'id' to a number
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
                    this.toast.show('Consumer App dose not exists.');
                    this.loader.loader = false;
                });
        });
    }

    updateApps(consumerModel) {
        this.loader.loader = true;
        this.consumerAppService.updateConsumerApps(consumerModel).subscribe((result) => {
            this.toast.show('Consumer App is updated successfully.');
            this.cancel();
            this.loader.loader = false;
        }, err => {
            this.toast.show('Consumer App Name is already exists.');
            this.loader.loader = false;
        });
    }

    cancel() {
        this.location.back();
    }
}
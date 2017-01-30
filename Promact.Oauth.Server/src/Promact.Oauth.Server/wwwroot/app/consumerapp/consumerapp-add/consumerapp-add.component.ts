﻿import { Component } from "@angular/core";
import { ConsumerAppModel, consumerappallowedscopes } from "../consumerapp-model";
import { Router } from "@angular/router";
import { ConsumerAppService } from "../consumerapp.service";
import { Md2Toast } from 'md2';
import { LoaderService } from '../../shared/loader.service';


@Component({
    templateUrl: "app/consumerapp/consumerapp-add/consumerapp-add.html",
})
export class ConsumerappAddComponent {
    scopes: any;
    consumerModel: ConsumerAppModel;
    clientSecretIndicator: boolean = false;
    constructor(private consumerAppService: ConsumerAppService, private router: Router, private toast: Md2Toast, private loader: LoaderService) {
        this.consumerModel = new ConsumerAppModel();
        this.scopes = [];
        for (let scope in consumerappallowedscopes) {
            let scopeIntegerValue: any = parseInt(scope);
            if (!isNaN(parseFloat(scope)) && isFinite(scopeIntegerValue)) {
                this.scopes.push({ value: scopeIntegerValue, name: consumerappallowedscopes[scopeIntegerValue] });
            }
        }
    };

    submitApps(consumerModel) {
        this.loader.loader = true;
        this.consumerAppService.addConsumerApps(consumerModel).subscribe((result) => {
            this.toast.show('Consumer App is added successfully.');
            this.cancel();
        }, err => {
            this.toast.show('Consumer App Name is already exists.');
            this.loader.loader = false;
        });
    };

    cancel() {
        this.router.navigate(['/consumerapp']);
    };
    getRandomNumber(isAuthId: boolean) {
        this.consumerAppService.getRandomNumber(isAuthId).subscribe((result) => {
            if (isAuthId === true) {
                this.consumerModel.AuthId = result;
            }
            else {
                this.clientSecretIndicator = true;
                this.consumerModel.AuthSecret = result;
            }
        }), err => {
            this.toast.show('Error generating random number');
        };
        

    };
}
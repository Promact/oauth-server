import { Component, OnInit } from "@angular/core";
import { ConsumerAppModel, consumerappallowedscopes } from '../consumerapp-model';
import { Router, ActivatedRoute } from '@angular/router';
import { ConsumerAppService } from '../consumerapp.service';
import { Md2Toast } from 'md2';
import { Location } from "@angular/common";
import { LoaderService } from '../../shared/loader.service';

@Component({
    templateUrl: "app/consumerapp/consumerapp-edit/consumerapp-edit.html",
})
export class ConsumerappEditComponent implements OnInit {
    consumerModel: ConsumerAppModel;
    scopes: any;
    clientSecretIndicator: boolean;
    constructor(private router: Router, private consumerAppService: ConsumerAppService, private route: ActivatedRoute, private toast: Md2Toast, private location: Location, private loader: LoaderService) {
        this.consumerModel = new ConsumerAppModel();
        this.scopes = [];
        this.clientSecretIndicator = false;
        for (let scope in consumerappallowedscopes) {
            let scopeIntegerValue: any = parseInt(scope);
            if (!isNaN(parseFloat(scope)) && isFinite(scopeIntegerValue)) {
                this.scopes.push({ value: scopeIntegerValue, name: consumerappallowedscopes[scopeIntegerValue] });
            }
        }
    }

    ngOnInit() {
        this.loader.loader = true;
        this.route.params.subscribe(params => {
            let id = params['id'];
            this.consumerAppService.getConsumerAppById(id).subscribe((result) => {
                this.consumerModel.Name = result.name;
                this.consumerModel.CallbackUrl = result.callbackUrl;
                this.consumerModel.Scopes = result.scopes;
                this.consumerModel.LogoutUrl = result.logoutUrl;
                this.consumerModel.AuthId = result.authId;
                this.consumerModel.AuthSecret = result.authSecret;
                this.consumerModel.Id = result.id;
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
    }
}
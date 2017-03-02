import { Component, OnInit } from "@angular/core";
import { ConsumerAppModel, consumerappallowedscopes } from "../consumerapp-model";
import { Router } from "@angular/router";
import { ConsumerAppService } from "../consumerapp.service";
import { Md2Toast } from 'md2';
import { LoaderService } from '../../shared/loader.service';
@Component({
    templateUrl: "app/consumerapp/consumerapp-add/consumerapp-add.html",
})
export class ConsumerappAddComponent implements OnInit {
    scopes: any;
    consumerModel: ConsumerAppModel;
    clientSecretIndicator: boolean = false;
    clientScopeIndicator: boolean = false;
    constructor(private consumerAppService: ConsumerAppService, private router: Router, private toast: Md2Toast, private loader: LoaderService) {
        this.consumerModel = new ConsumerAppModel();
        this.scopes = [];
        for (let scope in consumerappallowedscopes) {
            let scopeIntegerValue: any = parseInt(scope);
            if (!isNaN(parseFloat(scope)) && isFinite(scopeIntegerValue)) {
                this.scopes.push({ value: scopeIntegerValue, name: consumerappallowedscopes[scopeIntegerValue] });
            }
        }
        this.clientScopeIndicator = false;
    };
    ngOnInit() {
        this.loader.loader = true;
        this.getRandomNumber(true);
        this.getRandomNumber(false);
        this.loader.loader = false;
    };
    submitApps(consumerModel: ConsumerAppModel) {
        this.loader.loader = true;
        this.consumerAppService.addConsumerApps(consumerModel).then((result) => {
            this.toast.show('Consumer App is added successfully.');
            this.cancel();
        });
        this.loader.loader = false;
    };
    cancel() {
        this.router.navigate(['/consumerapp']);
    };
    getRandomNumber(isAuthId: boolean) {
        this.consumerAppService.getRandomNumber(isAuthId).then((result) => {
            if (isAuthId === true) {
                this.consumerModel.AuthId = result;
            }
            else {
                this.clientSecretIndicator = true;
                this.consumerModel.AuthSecret = result;
            }
        });
    };
    scopeOnChange(scopes: Array<consumerappallowedscopes>) {
        if (scopes.length === 0) {
            this.clientScopeIndicator = true;
        }
    }
}
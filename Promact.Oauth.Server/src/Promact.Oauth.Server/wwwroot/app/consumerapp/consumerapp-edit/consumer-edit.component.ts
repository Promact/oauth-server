import { Component, OnInit } from "@angular/core";
import { ConsumerAppModel, consumerappallowedscopes } from '../consumerapp-model';
import { Router, ActivatedRoute } from '@angular/router';
import { ConsumerAppService } from '../consumerapp.service';
import { Md2Toast } from 'md2';
import { LoaderService } from '../../shared/loader.service';
import { StringConstant } from '../../shared/stringconstant';

@Component({
    templateUrl: "app/consumerapp/consumerapp-edit/consumerapp-edit.html",
})
export class ConsumerappEditComponent implements OnInit {
    consumerModel: ConsumerAppModel;
    scopes: any;
    clientSecretIndicator: boolean;
    clientScopeIndicator: boolean = false;
    constructor(private router: Router, private consumerAppService: ConsumerAppService, private route: ActivatedRoute,
        private toast: Md2Toast,private loader: LoaderService, private stringConstant: StringConstant) {
        this.consumerModel = new ConsumerAppModel();
        this.scopes = [];
        this.clientSecretIndicator = false;
        for (let scope in consumerappallowedscopes) {
            let scopeIntegerValue: any = parseInt(scope);
            if (!isNaN(parseFloat(scope)) && isFinite(scopeIntegerValue)) {
                this.scopes.push({ value: scopeIntegerValue, name: consumerappallowedscopes[scopeIntegerValue] });
            }
        }
        this.clientScopeIndicator = false;
    }

    ngOnInit() {
        this.loader.loader = true;
        this.route.params.subscribe(params => {
            let id = params[this.stringConstant.paramsId];
            this.consumerAppService.getConsumerAppById(id).then((result) => {
                this.consumerModel.Name = result.name;
                this.consumerModel.CallbackUrl = result.callbackUrl;
                this.consumerModel.Scopes = result.scopes;
                this.consumerModel.LogoutUrl = result.logoutUrl;
                this.consumerModel.AuthId = result.authId;
                this.consumerModel.AuthSecret = result.authSecret;
                this.consumerModel.Id = result.id;
            });
            this.loader.loader = false;
        });
    }

    updateApps(consumerModel) {
        this.loader.loader = true;
        this.consumerAppService.updateConsumerApps(consumerModel).then((result) => {
            this.toast.show('Consumer App is updated successfully.');
            this.cancel();
        });
        this.loader.loader = false;
    }

    cancel() {
        this.router.navigate(['/consumerapp']);
    }

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
    }

    scopeOnChange(scopes: Array<consumerappallowedscopes>) {
        if (scopes.length === 0) {
            this.clientScopeIndicator = true;
        }
    }
}
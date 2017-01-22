import { Component } from "@angular/core";
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
    constructor(private consumerAppService: ConsumerAppService, private router: Router, private toast: Md2Toast, private loader: LoaderService) {
        this.consumerModel = new ConsumerAppModel();
        this.scopes = [];
        for (var scope in consumerappallowedscopes) {
            var scopeIntegerValue: any = parseInt(scope);
            if (!isNaN(parseFloat(scope)) && isFinite(scopeIntegerValue)) {
                this.scopes.push({ value: scopeIntegerValue, name: consumerappallowedscopes[scopeIntegerValue] });
            }
        }
    }

    submitApps(consumerModel) {
        this.loader.loader = true;
        this.consumerAppService.addConsumerApps(consumerModel).subscribe((result) => {
            this.toast.show('Consumer App is added successfully.');
            this.cancel();
        }, err => {
            this.toast.show('Consumer App Name is already exists.');
            this.loader.loader = false;
        });
    }

    cancel() {
        this.router.navigate(['/consumerapp']);
    }


}
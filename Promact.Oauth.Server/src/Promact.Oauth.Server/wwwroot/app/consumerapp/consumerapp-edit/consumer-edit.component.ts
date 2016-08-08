import {Component} from "@angular/core";
import {ConsumerAppModel} from '../consumerapp-model';
import { ROUTER_DIRECTIVES, Router, ActivatedRoute } from '@angular/router';
import { ConsumerAppService} from '../consumerapp.service';


@Component({
    templateUrl: "app/consumerapp/consumerapp-edit/consumerapp-edit.html",
    directives: []
})
export class ConsumerappEditComponent {
    consumerModel: ConsumerAppModel;
    private sub: any;
    constructor(private router: Router, private consumerAppService: ConsumerAppService, private route: ActivatedRoute) {
        this.consumerModel = new ConsumerAppModel();
    }

    ngOnInit() {
         this.route.params.subscribe(params => {
            let id = +params['id']; // (+) converts string 'id' to a number
            this.consumerAppService.getConsumerAppById(id).subscribe((result) => {
                this.consumerModel.Name = result.name;
                this.consumerModel.Description = result.description;
                this.consumerModel.CallbackUrl = result.callbackUrl;
                this.consumerModel.Id = result.id;
                this.consumerModel.AuthId = result.authId;
                this.consumerModel.AuthSecret = result.authSecret;
            }
                , err => {

                });
        });
    }

    updateApps(consumerModel) {
        this.consumerAppService.updateConsumerApps(consumerModel).subscribe((result) => {

        }, err => {

        });
    }

    cancel() {
        this.router.navigate(['/consumerapp/']);
    }
}
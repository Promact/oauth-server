import {Component} from "@angular/core";
import {ConsumerAppModel} from '../consumerapp-model';
import { Router, ActivatedRoute } from '@angular/router';
import { ConsumerAppService} from '../consumerapp.service';
//import {Md2Toast} from 'md2/toast';
import {Location} from "@angular/common";


@Component({
    templateUrl: "app/consumerapp/consumerapp-edit/consumerapp-edit.html",
    //directives: [],
    //providers: [Md2Toast]
})
export class ConsumerappEditComponent {
    consumerModel: ConsumerAppModel;
    private sub: any;
    constructor(private router: Router, private consumerAppService: ConsumerAppService, private route: ActivatedRoute,/* private toast: Md2Toast,*/ private location: Location) {
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
            if (result == true) {
                //this.toast.show('Consumer App is updated successfully.');
                this.cancel();
            }
            else if (result == false) {
                //this.toast.show('Consumer App Name is already exists.');
            }
        }, err => {

        });
    }

    cancel() {
        this.location.back();
        //this.router.navigate(['admin/consumerapp']);
    }
}
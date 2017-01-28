import { NgModule } from "@angular/core";
import { ConsumerAppComponent } from "./consumerapp.component";
import { ConsumerappListComponent } from "./consumerapp-list/consumerapp-list.component";
import { ConsumerappAddComponent } from "./consumerapp-add/consumerapp-add.component";
import { ConsumerappEditComponent } from "./consumerapp-edit/consumer-edit.component";
import { CommonModule } from "@angular/common";
import { ConsumerAppService } from "./consumerapp.service";
import { consumerRoute } from "./consumerapp-routes";
import { SharedModule } from '../shared/shared.module';
import { URLValidatorDirective } from "../shared/url.validator";

@NgModule({
    imports: [
        consumerRoute,
        SharedModule
    ],
    declarations: [
        ConsumerAppComponent,
        ConsumerappListComponent,
        ConsumerappAddComponent,
        ConsumerappEditComponent,
        URLValidatorDirective
    ],
    exports: [URLValidatorDirective],
    providers: [
        ConsumerAppService
    ]
})
export class ConsumerAppModule { }
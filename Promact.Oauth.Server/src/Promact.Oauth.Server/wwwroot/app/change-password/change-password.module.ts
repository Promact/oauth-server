import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from "@angular/core";
import { ChangePasswordComponent } from "./change-password.component";
import { ChangePasswordMainComponent } from "./changepassword.component";
import { changePasswordRoute } from "./change-password.routes";
import { UserService } from '../users/user.service';
import { SharedModule } from '../shared/shared.module';

@NgModule({
    imports: [
        changePasswordRoute,
        SharedModule
    ],
    declarations: [
        ChangePasswordMainComponent,
        ChangePasswordComponent
    ],
    providers: [
        UserService
    ],
    schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class ChangePasswordModule { }
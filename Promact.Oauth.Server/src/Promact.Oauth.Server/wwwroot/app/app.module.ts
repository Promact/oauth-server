import { NgModule } from "@angular/core";
import { BrowserModule } from "@angular/platform-browser";
// Imports for loading & configuring the in-memory web api
import { HttpModule, XHRBackend } from "@angular/http";
import { AppComponent } from "./app.component";
import { routing } from "./app.routes";
import { HashLocationStrategy, LocationStrategy } from '@angular/common';
import { ProjectModule } from "./project/project.module";
import { ConsumerAppModule } from "./consumerapp/consumerapp.module";
import { UserModule } from "./users/user.module";
import { ChangePasswordModule } from "./change-password/change-password.module";
import { MaterialModule } from "@angular/material";
import { LoaderService } from "./shared/loader.service";
import { UserRole } from "./shared/userrole.model";
import { StringConstant } from './shared/stringconstant';
import { AuthenticationService } from "./authentication.service";



@NgModule({
    declarations: [AppComponent],
    imports: [
        BrowserModule,
        HttpModule,
        MaterialModule.forRoot(),
        routing,
        ProjectModule,
        ConsumerAppModule,
        UserModule,
        ChangePasswordModule
    ],
    bootstrap: [AppComponent],
    providers: [StringConstant, LoaderService, AuthenticationService, { provide: LocationStrategy, useClass: HashLocationStrategy }, UserRole],
})
export class AppModule { }
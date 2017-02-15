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
import { LoaderService } from "./shared/loader.service";
import { UserRole } from "./shared/userrole.model";
import { StringConstant } from './shared/stringconstant';
import {MaterialModule } from "@angular/material";



@NgModule({
    declarations: [AppComponent],
    imports: [
        BrowserModule,
        HttpModule,
        routing,
        ProjectModule,
        MaterialModule,
        ConsumerAppModule,
        UserModule,
        ChangePasswordModule
    ],
    bootstrap: [AppComponent],
    providers: [StringConstant, LoaderService, { provide: LocationStrategy, useClass: HashLocationStrategy }, UserRole],
})
export class AppModule { }
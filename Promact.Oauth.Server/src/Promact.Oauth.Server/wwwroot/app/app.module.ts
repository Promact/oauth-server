import { NgModule } from "@angular/core";
import { BrowserModule } from "@angular/platform-browser";
// Imports for loading & configuring the in-memory web api
import { HttpModule, XHRBackend } from "@angular/http";
import { AppComponent } from "./app.component";
import { routing } from "./app.routes";
import { HttpService } from "./http.service";
import { HashLocationStrategy, LocationStrategy } from '@angular/common';
import { ProjectModule } from "./project/project.module";
import { ConsumerAppModule } from "./consumerapp/consumerapp.module";
import { UserModule } from "./users/user.module";
import { ChangePasswordModule } from "./change-password/change-password.module";
import { LoginService } from "./login.service";
import { LoaderService } from "./shared/loader.service";
import { UserRole } from "./shared/userrole.model";
import { SharedModule } from "./shared/shared.module";
import { MaterialModule } from '@angular/material';

@NgModule({
    declarations: [AppComponent],
    imports: [
        BrowserModule,
        HttpModule,
        routing,
        MaterialModule,
        SharedModule,
        ProjectModule,
        ConsumerAppModule,
        UserModule,
        ChangePasswordModule
    ],
    bootstrap: [AppComponent],
    providers: [LoginService, LoaderService, { provide: LocationStrategy, useClass: HashLocationStrategy }, UserRole],
})
export class AppModule { }
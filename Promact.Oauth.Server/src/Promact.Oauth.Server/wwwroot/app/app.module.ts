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
import { MdSidenavModule } from "@angular2-material/sidenav";
import { MdToolbarModule } from "@angular2-material/toolbar";
import { LoaderService } from "./shared/loader.service";
import { UserRole } from "./shared/userrole.model";


@NgModule({
    declarations: [AppComponent],
    imports: [
        BrowserModule,
        HttpModule,
        MdToolbarModule.forRoot(),
        MdSidenavModule.forRoot(),
        routing,
        ProjectModule,
        ConsumerAppModule,
        UserModule,
        ChangePasswordModule
    ],
    bootstrap: [AppComponent],
    providers: [HttpService, LoginService, LoaderService, { provide: LocationStrategy, useClass: HashLocationStrategy }, UserRole],
})
export class AppModule { }
import { NgModule } from "@angular/core";
import { BrowserModule  } from "@angular/platform-browser";
import { MdButtonModule } from "@angular2-material/button";
import { MdToolbarModule } from "@angular2-material/toolbar";
import {MdInputModule} from "@angular2-material/input";
import {MdSidenavModule} from "@angular2-material/sidenav";
import {MdCheckboxModule} from "@angular2-material/checkbox";
// Imports for loading & configuring the in-memory web api
import { HttpModule, XHRBackend } from "@angular/http";
import { AppComponent } from "./app.component";
import { FormsModule } from "@angular/forms";
import { routing } from "./app.routes";
import { ConsumerAppComponent} from "./consumerapp/consumerapp.component";
import {ConsumerappListComponent} from "./consumerapp/consumerapp-list/consumerapp-list.component";
import {ConsumerappAddComponent} from "./consumerapp/consumerapp-add/consumerapp-add.component";
import {ConsumerappEditComponent} from "./consumerapp/consumerapp-edit/consumer-edit.component";
import {ProjectComponent} from "./project/project.component";
import {ProjectListComponent} from "./project/project-list/project-list.component";
import {ProjectAddComponent} from "./project/project-add/project-add.component";
import {ProjectEditComponent} from "./project/project-edit/project-edit.component";
import {ProjectViewComponent} from "./project/project-view/project-view.component";
import {UserComponent} from "./users/user.component";
import {UserListComponent} from "./users/user-list/user-list.component";
import {UserAddComponent} from "./users/user-add/user-add.component";
import {UserEditComponent} from "./users/user-edit/user-edit.component";
import {UserDetailsComponent} from "./users/user-details/user-details.component";
import {ChangePasswordComponent} from "./change-password/change-password.component";
import {ChangePassword} from "./change-password/changepassword.component";
import {HttpService} from "./http.service";
import { ConsumerAppService }   from "./consumerapp/consumerapp.service";
import {ProjectService} from "./project/project.service";
import {UserService} from "./users/user.service";
import { LoginService } from "./login.service";
import {MdSelect, MdOption, MdSelectDispatcher} from './select/select';
import {MdMultiselect} from './multiselect/multiselect';
import { HashLocationStrategy, LocationStrategy } from '@angular/common';


@NgModule({
    declarations: [AppComponent, ConsumerAppComponent, ConsumerappListComponent, ConsumerappAddComponent, ConsumerappEditComponent, ProjectComponent, ProjectListComponent, ProjectAddComponent, ProjectEditComponent, ProjectViewComponent, UserComponent, UserListComponent, UserAddComponent, UserEditComponent, UserDetailsComponent, ChangePasswordComponent, MdSelect, MdOption, MdMultiselect, ChangePassword],

    imports: [
        BrowserModule,
        HttpModule,
        FormsModule,
        routing,
        MdButtonModule, 
        MdToolbarModule,
        MdSidenavModule,
        MdInputModule,
        MdCheckboxModule, 
        

    ],
    bootstrap: [AppComponent],
    providers: [HttpService, ConsumerAppService, ProjectService, UserService, LoginService, MdSelectDispatcher, { provide: LocationStrategy, useClass: HashLocationStrategy }
    ],
})
export class AppModule { } 
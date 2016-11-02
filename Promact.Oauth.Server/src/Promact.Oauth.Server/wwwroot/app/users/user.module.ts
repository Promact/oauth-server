import { NgModule } from "@angular/core";
import { UserComponent } from "./user.component";
import { UserListComponent } from "./user-list/user-list.component";
import { UserAddComponent } from "./user-add/user-add.component";
import { UserEditComponent } from "./user-edit/user-edit.component";
import { UserDetailsComponent } from "./user-details/user-details.component";
import { UserService } from "./user.service";
import { userRoute } from "./user.routes";
import { SharedModule } from '../shared/shared.module';



@NgModule({
    imports: [
        userRoute,
        SharedModule
    ],
    declarations: [
        UserComponent,
        UserListComponent,
        UserDetailsComponent, 
        UserAddComponent,
        UserEditComponent
    ],
    providers: [
        UserService
    ]
})
export class UserModule { }
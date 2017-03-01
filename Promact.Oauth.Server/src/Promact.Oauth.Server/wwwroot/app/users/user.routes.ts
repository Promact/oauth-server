import { ModuleWithProviders } from "@angular/core";
import { Routes, RouterModule} from '@angular/router';
import {UserComponent} from './user.component';
import {UserListComponent} from './user-list/user-list.component';
import {UserAddComponent} from './user-add/user-add.component';
import {UserEditComponent} from './user-edit/user-edit.component';
import {UserDetailsComponent} from './user-details/user-details.component';
import { AuthenticationService } from "../authentication.service";

const userRoutes: Routes = [{
    path: "user",
    component: UserComponent,
    children: [
        { path: '', component: UserListComponent, canActivate: [AuthenticationService] },
        { path: 'list', component: UserListComponent, canActivate: [AuthenticationService]},
        { path: 'add', component: UserAddComponent, canActivate: [AuthenticationService]},
        { path: 'edit/:id', component: UserEditComponent, canActivate: [AuthenticationService] },
        { path: 'details/:id', component: UserDetailsComponent }
    ]
}];
export const userRoute : ModuleWithProviders = RouterModule.forChild(userRoutes);






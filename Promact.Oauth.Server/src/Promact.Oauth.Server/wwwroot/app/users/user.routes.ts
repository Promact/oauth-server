import { ModuleWithProviders } from "@angular/core";
import { Routes, RouterModule} from '@angular/router';
import {UserComponent} from './user.component';
import {UserListComponent} from './user-list/user-list.component';
import {UserAddComponent} from './user-add/user-add.component';
import {UserEditComponent} from './user-edit/user-edit.component';
import {UserDetailsComponent} from './user-details/user-details.component';

const userRoutes: Routes = [{
    path: "user",
    component: UserComponent,
    children: [
        { path: '', component: UserListComponent },
        { path: 'list', component: UserListComponent },
        { path: 'add', component: UserAddComponent },
        { path: 'edit/:id', component: UserEditComponent },
        { path: 'details/:id', component: UserDetailsComponent }
    ]
}];
export const userRoute : ModuleWithProviders = RouterModule.forChild(userRoutes);






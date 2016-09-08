import { provideRouter, RouterConfig } from '@angular/router';

import {UserComponent} from './user.component';
import {UserListComponent} from './user-list/user-list.component';
import {UserAddComponent} from './user-add/user-add.component';
import {UserEditComponent} from './user-edit/user-edit.component';
import {UserDetailsComponent} from './user-details/user-details.component';
import {ChangePasswordComponent} from './user-change-password/user-change-password.component';

export const userRoutes: RouterConfig = [{
    path: "user",
    component: UserComponent,
    children: [
        { path: 'list', component: UserListComponent },
        { path: 'add', component: UserAddComponent },
        { path: 'edit/:id', component: UserEditComponent },
        { path: 'details/:id', component: UserDetailsComponent },
        { path: 'changePassword', component: ChangePasswordComponent },

    ]
}];
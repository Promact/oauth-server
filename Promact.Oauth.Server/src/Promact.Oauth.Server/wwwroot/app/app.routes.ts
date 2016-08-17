
import {consumerRoute} from './consumerapp/consumerapp-routes';
import { Routes, RouterModule } from '@angular/router';
import {ConsumerAppComponent} from './consumerapp/consumerapp.component';
import {ConsumerappListComponent} from './consumerapp/consumerapp-list/consumerapp-list.component';
import {ConsumerappEditComponent} from "./consumerapp/consumerapp-edit/consumer-edit.component";
import {ConsumerappAddComponent} from "./consumerapp/consumerapp-add/consumerapp-add.component";

import {ProjectComponent} from "./project/project.component";
import {ProjectListComponent} from "./project/project-list/project-list.component";
import {ProjectAddComponent} from "./project/project-add/project-add.component";
import {ProjectEditComponent} from "./project/project-edit/project-edit.component";
import {ProjectViewComponent} from "./project/project-view/project-view.component";

import {UserComponent} from './users/user.component';
import {UserListComponent} from './users/user-list/user-list.component';
import {UserAddComponent} from './users/user-add/user-add.component';
import {UserEditComponent} from './users/user-edit/user-edit.component';
import {UserDetailsComponent} from './users/user-details/user-details.component';
import {ChangePasswordComponent} from './users/user-change-password/user-change-password.component';


const appRoutes: Routes = [

    // ...consumerRoute,
    { path: '', component: ConsumerAppComponent },
    { path: 'consumerapp', component: ConsumerappListComponent },
    { path: 'consumerapp/add', component: ConsumerappAddComponent },
    { path: 'consumerapp/edit/:id', component: ConsumerappEditComponent },

    { path: '', component: ProjectComponent },
    { path: 'project', component: ProjectListComponent },
    { path: 'project/add', component: ProjectAddComponent },
    { path: 'project/edit/:id', component: ProjectEditComponent },
    { path: 'project/view/:id', component: ProjectViewComponent }

    { path: '', component: UserComponent },
    { path: 'user', component: UserListComponent },
    { path: 'user/user/add', component: UserAddComponent },
    { path: 'user/edit/:id', component: UserEditComponent },
    { path: 'user/details/:id', component: UserDetailsComponent },
    { path: 'user/changePassword', component: ChangePasswordComponent }
]

export const routing = RouterModule.forRoot(appRoutes);
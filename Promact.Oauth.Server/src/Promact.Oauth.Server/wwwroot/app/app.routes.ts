import {consumerRoute} from './consumerapp/consumerapp-routes';
import {userRoutes} from './users/user.routes';
import {projectRoutes} from './project/project.routes';
import {Routes, RouterModule } from '@angular/router';
import {ConsumerAppComponent} from './consumerapp/consumerapp.component';
import {ProjectComponent} from "./project/project.component";
import {UserComponent} from './users/user.component';
import {changePasswordRoute} from './change-password/change-password.routes';
import {ChangePassword} from './change-password/changepassword.component';

const appRoutes: Routes = [

    { path: '', component: UserComponent },
    ...consumerRoute,
    { path: 'consumerapp', component: ConsumerAppComponent },
    ...projectRoutes,
    { path: 'project', component: ProjectComponent },
    ...userRoutes,
    { path: 'user', component: UserComponent },
    ...changePasswordRoute,
    { path: 'changepassword', component: ChangePassword },
]

export const routing = RouterModule.forRoot(appRoutes);
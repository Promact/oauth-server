import { ModuleWithProviders } from '@angular/core';
import { consumerRoute } from './consumerapp/consumerapp-routes';
import { userRoute } from './users/user.routes';
import { projectRoute } from './project/project.routes';
import { Routes, RouterModule } from '@angular/router';
import { ConsumerAppComponent } from './consumerapp/consumerapp.component';
import { ProjectComponent } from "./project/project.component";
import { UserComponent } from './users/user.component';
import { changePasswordRoute } from './change-password/change-password.routes';
import { ChangePassword } from './change-password/changepassword.component';

const appRoutes: Routes =
    [
        { path: '', component: UserComponent }
    ];
export const routing: ModuleWithProviders = RouterModule.forRoot(appRoutes);
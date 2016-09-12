
import { provideRouter, RouterConfig } from '@angular/router';
import {ChangePasswordComponent} from './change-password.component';
import {ChangePassword } from './changepassword.component';

export const changePasswordRoute: RouterConfig = [{
    path: "changepassword",
    component: ChangePassword,
    children: [
        { path: '', component: ChangePasswordComponent },
    ]
}];
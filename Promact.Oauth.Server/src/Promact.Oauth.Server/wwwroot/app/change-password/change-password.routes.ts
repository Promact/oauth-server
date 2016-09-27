import { ModuleWithProviders } from "@angular/core";
import { Routes, RouterModule } from '@angular/router';
import { ChangePasswordComponent } from './change-password.component';
import { ChangePassword } from './changepassword.component';
const changePasswordRoutes: Routes = [{
    path: "changepassword",
    component: ChangePassword,
    children: [
        { path: '', component: ChangePasswordComponent },
    ]
}];
export const changePasswordRoute: ModuleWithProviders = RouterModule.forChild(changePasswordRoutes);

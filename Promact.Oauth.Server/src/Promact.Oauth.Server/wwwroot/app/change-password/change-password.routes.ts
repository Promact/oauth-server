import { ModuleWithProviders } from "@angular/core";
import { Routes, RouterModule } from '@angular/router';
import { ChangePasswordComponent } from './change-password.component';
import { ChangePasswordMainComponent } from './changepassword.component';
const changePasswordRoutes: Routes = [{
    path: "changepassword",
    component: ChangePasswordMainComponent,
    children: [
        { path: '', component: ChangePasswordComponent },
    ]
}];
export const changePasswordRoute: ModuleWithProviders = RouterModule.forChild(changePasswordRoutes);

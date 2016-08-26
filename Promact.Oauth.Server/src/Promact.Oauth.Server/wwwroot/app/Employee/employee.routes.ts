
import {userRoutes} from '../users/user.routes';
import {provideRouter, RouterConfig} from '@angular/router';
import { AdminComponent } from '../Admin/admin.component';
import {UserComponent} from '../users/user.component';
import {UserEditComponent} from '../users/user-edit/user-edit.component';


export const EmployeeRoute: RouterConfig = [{
    path: "employee",
    component: AdminComponent,
    children: [
        ...userRoutes,
        { path: 'user/:id', component: UserEditComponent },
    ]
}]; 
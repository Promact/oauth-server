
import {consumerRoute} from './consumerapp/consumerapp-routes';
import {userRoutes} from './users/user.routes';
import {projectRoutes} from './project/project.routes';
import {Routes, RouterModule } from '@angular/router';
import {ConsumerAppComponent} from './consumerapp/consumerapp.component';
import {ProjectComponent} from "./project/project.component";
import {UserComponent} from './users/user.component';
import { AdminComponent } from './Admin/admin.component';
import { AdminRoute } from './Admin/admin.routes';
import { EmployeeComponent } from './Employee/employee.component';
import { EmployeeRoute } from './Employee/employee.routes';
import { EmployeeEditComponent } from './Employee/employee-edit.component';


const appRoutes: Routes = [

    { path: '', component: AdminComponent },
    ...AdminRoute,
    { path: 'admin', component: AdminComponent },
    ...EmployeeRoute,
    { path: 'employee/:id', component: EmployeeComponent },
    { path: 'employee/edit/:id', component: EmployeeEditComponent },
]

export const routing = RouterModule.forRoot(appRoutes);
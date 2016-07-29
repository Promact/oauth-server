import { provideRouter, RouterConfig } from '@angular/router';

import {UserComponent} from './user.component';
import {UserListComponent} from './user-list/user-list.component';

export const projectRoutes: RouterConfig = [{
    path: "user",
    component: UserComponent,
    children: [
        { path: '', component: UserListComponent }
    ]
}];
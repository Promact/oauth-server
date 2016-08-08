import { Injectable } from '@angular/core';
import {HttpService} from "../http.service";
import 'rxjs/add/operator/toPromise';

import {UserModel} from './user.model';

@Injectable()
export class UserService {
    private UserUrl = 'api/user';

    constructor(private httpService: HttpService<UserModel>) { }

    getUsers() {
        return this.httpService.get(this.UserUrl + "/users");
    }

    registerUser(newUser: UserModel) {
        newUser.IsActive = true;
        return this.httpService.post(this.UserUrl + "/add", newUser);
    }

    getUserById(userId: string) {
        return this.httpService.get(this.UserUrl + "/" + userId);
    }

    editUser(editedUser: UserModel) {
        return this.httpService.put(this.UserUrl + "/edit", editedUser);
    }
}
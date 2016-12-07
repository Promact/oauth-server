import { Injectable } from '@angular/core';
import { HttpService } from "../http.service";
import 'rxjs/add/operator/toPromise';

import { UserModel } from './user.model';
import { PasswordModel } from './user-password.model';

@Injectable()
export class UserService {
    private UserUrl = 'api/user';

    constructor(private httpService: HttpService<UserModel>, private httpServiceForPassword: HttpService<PasswordModel>) { }

    getUsers() {
        return this.httpService.get(this.UserUrl + "/users");
    }

    registerUser(newUser: UserModel) {
        newUser.IsActive = true;
        newUser.Email = newUser.Email + "@promactinfo.com";
        return this.httpService.post(this.UserUrl + "/add", newUser);
    }

    getUserById(userId: string) {
        return this.httpService.get(this.UserUrl + "/" + userId);
    }

    editUser(editedUser: UserModel) {
        return this.httpService.put(this.UserUrl + "/edit", editedUser);
    }

    changePassword(newPassword: PasswordModel) {
        return this.httpServiceForPassword.post(this.UserUrl + "/changePassword", newPassword);
    }

    findUserByUserName(userName: string) {
        return this.httpService.get(this.UserUrl + "/findbyusername/" + userName);
    }

    checkEmailIsExists(email: string) {
        return this.httpService.get(this.UserUrl + "/checkEmailIsExists/" + email);
    }
    checkUserIsExistsBySlackUserName(slackUserName: string) {
        return this.httpService.get(this.UserUrl + "/checkUserIsExistsBySlackUserName/" + slackUserName);
    }
    getRoles() {
        return this.httpService.get(this.UserUrl + "/getRole");
    }

    reSendMail(id: string) {
        return this.httpService.get(this.UserUrl + "/reSendMail" + "/" + id);
    }
    checkOldPasswordIsValid(oldPassword: string) {
        return this.httpService.get(this.UserUrl + "/checkOldPasswordIsValid/" + oldPassword);
    }

   //fetchSlackUserDetails() {
   //    return this.httpService.get(this.UserUrl + "/slackUserDetails");
   // }

}
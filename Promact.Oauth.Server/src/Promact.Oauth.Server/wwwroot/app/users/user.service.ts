import { Injectable } from '@angular/core';
import { HttpService } from "../http.service";
import 'rxjs/add/operator/toPromise';

import { UserModel } from './user.model';
import { PasswordModel } from './user-password.model';

@Injectable()
export class UserService {
    private UserUrl = 'api/users';

    constructor(private httpService: HttpService<UserModel>, private httpServiceForPassword: HttpService<PasswordModel>) { }

    getUsers() {
        return this.httpService.get(this.UserUrl);
    }

    registerUser(newUser: UserModel) {
        newUser.IsActive = true;
        newUser.Email = newUser.Email + "@promactinfo.com";
        return this.httpService.post(this.UserUrl, newUser);
    }

    getUserById(userId: string) {
        return this.httpService.get(this.UserUrl + "/" + userId);
    }

    editUser(editedUser: UserModel) {
        return this.httpService.put(this.UserUrl + "/" + editedUser.Id , editedUser);
    }

    changePassword(newPassword: PasswordModel) {
        return this.httpServiceForPassword.post(this.UserUrl + "/password", newPassword);
    }

    findUserByUserName(userName: string) {
        return this.httpService.get(this.UserUrl + "/detail/" + userName);
    }

    checkEmailIsExists(email: string) {
        return this.httpService.get(this.UserUrl + "/available/" + email);
    }
    checkUserIsExistsBySlackUserName(slackUserName: string) {
        return this.httpService.get(this.UserUrl + "/availableUser/" + slackUserName);
    }
    getRoles() {
        return this.httpService.get(this.UserUrl + "/roles");
    }

    reSendMail(id: string) {
        return this.httpService.get(this.UserUrl + "/email/" + id +"/send");
    }
    checkOldPasswordIsValid(password: string) {
        return this.httpService.get(this.UserUrl + "/" + password +"/available");
    }

   fetchSlackUserDetails() {
       return this.httpService.get(this.UserUrl + "/slackUserDetails");
    }

}
import { Injectable } from '@angular/core';
import { HttpService } from "../http.service";
import 'rxjs/add/operator/toPromise';

import { UserModel } from './user.model';
import { PasswordModel } from './user-password.model';
import { StringConstant } from '../shared/stringconstant';

@Injectable()
export class UserService {
    private UserUrl = 'api/users';

    constructor(private httpService: HttpService<UserModel>, private httpServiceForPassword: HttpService<PasswordModel>, private stringConstant: StringConstant) { }

    getUsers() {
        return this.httpService.get(this.UserUrl);
    }

    registerUser(newUser: UserModel) {
        newUser.IsActive = true;
        newUser.Email = newUser.Email + "@promactinfo.com";
        return this.httpService.post(this.UserUrl, newUser);
    }

    getUserById(userId: string) {
        return this.httpService.get(this.stringConstant.userUrl + this.stringConstant.slash + userId);
    }

    editUser(editedUser: UserModel) {
        return this.httpService.put(this.UserUrl + "/" + editedUser.Id , editedUser);
    }

    changePassword(newPassword: PasswordModel) {
        return this.httpServiceForPassword.post(this.UserUrl + "/password", newPassword);
    }

    checkEmailIsExists(email: string) {
        return this.httpService.get(this.UserUrl + "/available/email/" + email);
    }
    checkUserIsExistsBySlackUserName(slackUserName: string) {
        return this.httpService.get(this.UserUrl + "/available/" + slackUserName);
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

   
}
import { Injectable } from '@angular/core';
import { HttpService } from "../http.service";
import 'rxjs/add/operator/toPromise';

import { UserModel } from './user.model';
import { PasswordModel } from './user-password.model';
import { StringConstant } from '../shared/stringconstant';

@Injectable()
export class UserService {
    private UserUrl = 'api/user';

    constructor(private httpService: HttpService<UserModel>, private httpServiceForPassword: HttpService<PasswordModel>, private stringConstant: StringConstant) { }

    getUsers() {
        return this.httpService.get(this.stringConstant.userUrl + "/users");
    }

    registerUser(newUser: UserModel) {
        newUser.IsActive = true;
        newUser.Email = newUser.Email + this.stringConstant.emailExtension;
        return this.httpService.post(this.stringConstant.userUrl + "/add", newUser);
    }

    getUserById(userId: string) {
        return this.httpService.get(this.stringConstant.userUrl + this.stringConstant.slash + userId);
    }

    editUser(editedUser: UserModel) {
        return this.httpService.put(this.stringConstant.userUrl + "/edit", editedUser);
    }

    changePassword(newPassword: PasswordModel) {
        return this.httpServiceForPassword.post(this.stringConstant.userUrl + "/changePassword", newPassword);
    }

    findUserByUserName(userName: string) {
        return this.httpService.get(this.stringConstant.userUrl + "/findbyusername/" + userName);
    }

    checkEmailIsExists(email: string) {
        return this.httpService.get(this.stringConstant.userUrl + "/checkEmailIsExists/" + email);
    }
    checkUserIsExistsBySlackUserName(slackUserName: string) {
        return this.httpService.get(this.stringConstant.userUrl + "/checkUserIsExistsBySlackUserName/" + slackUserName);
    }
    getRoles() {
        return this.httpService.get(this.stringConstant.userUrl + "/getRole");
    }

    reSendMail(id: string) {
        return this.httpService.get(this.stringConstant.userUrl + "/reSendMail" + "/" + id);
    }
    checkOldPasswordIsValid(oldPassword: string) {
        return this.httpService.get(this.stringConstant.userUrl + "/checkOldPasswordIsValid/" + oldPassword);
    }

   fetchSlackUserDetails() {
       return this.httpService.get(this.stringConstant.userUrl + "/slackUserDetails");
    }

}
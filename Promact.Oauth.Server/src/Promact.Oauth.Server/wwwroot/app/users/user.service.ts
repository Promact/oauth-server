import { Injectable } from '@angular/core';
import { HttpService } from "../http.service";
import 'rxjs/add/operator/toPromise';

import { UserModel } from './user.model';
import { PasswordModel } from './user-password.model';
import { StringConstant } from '../shared/stringconstant';

@Injectable()
export class UserService {
    constructor(private httpService: HttpService<UserModel>, private httpServiceForPassword: HttpService<PasswordModel>, private stringConstant: StringConstant) { }

    getUsers() {
        return this.httpService.get(this.stringConstant.userUrl);
    }

    registerUser(newUser: UserModel) {
        newUser.IsActive = true;
        newUser.Email = newUser.Email + this.stringConstant.emailExtension;
        return this.httpService.post(this.stringConstant.userUrl, newUser);
    }

    getUserById(userId: string) {
        return this.httpService.get(this.stringConstant.userUrl + '/' + userId);
    }

    editUser(editedUser: UserModel) {
        return this.httpService.put(this.stringConstant.userUrl + "/" + editedUser.Id , editedUser);
    }

    changePassword(newPassword: PasswordModel) {
        return this.httpServiceForPassword.post(this.stringConstant.userUrl + "/password", newPassword);
    }

    checkEmailIsExists(email: string) {
        return this.httpService.get(this.stringConstant.userUrl + "/available/email/" + email);
    }
    checkUserIsExistsBySlackUserName(slackUserName: string) {
        return this.httpService.get(this.stringConstant.userUrl + "/available/" + slackUserName);
    }
    getRoles() {
        return this.httpService.get(this.stringConstant.userUrl + "/roles");
    }

    reSendMail(id: string) {
        return this.httpService.get(this.stringConstant.userUrl + "/email/" + id +"/send");
    }
    checkOldPasswordIsValid(password: string) {
        return this.httpService.get(this.stringConstant.userUrl + "/" + password +"/available");
    }

   fetchSlackUserDetails() {
       return this.httpService.get(this.stringConstant.userUrl + "/slackUserDetails");
    }

}
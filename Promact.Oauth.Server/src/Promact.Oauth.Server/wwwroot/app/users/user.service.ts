import { Injectable } from '@angular/core';
import { Http, Headers, RequestOptions, Response } from "@angular/http";
import 'rxjs/add/operator/toPromise';
import 'rxjs/add/operator/map';
import { UserModel } from './user.model';
import { PasswordModel } from './user-password.model';

@Injectable()
export class UserService {
    private UserUrl = 'api/users';
    private headers = new Headers({ 'Content-Type': 'application/json' });
    constructor(private http: Http) { }


    /*This service used for get user list*
     *
    */
    getUsers(): Promise<UserModel[]> {
        return this.http.get(this.UserUrl).map(res => res.json()).toPromise();

    }


    /*This service used for add new user*
    *
    * @param newUser
    */
    registerUser(newUser: UserModel) {
        newUser.IsActive = true;
        newUser.Email = newUser.Email + "@promactinfo.com";
        return this.http.post(this.UserUrl, JSON.stringify(newUser), { headers: this.headers })
            .map(res => res.json())
            .toPromise();
    }

    /*This service used for get user by userId*
    *
    * @param userId
    */
    getUserById(userId: string) {
        return this.http.get(this.UserUrl + "/" + userId)
            .map(res => res.json())
            .toPromise();
    }


    /*This service used for update user*
    *
    * @param editedUser
    */
    editUser(editedUser: UserModel) {
        return this.http.put(this.UserUrl + "/", JSON.stringify(editedUser), { headers: this.headers })
            .map(res => res.json())
            .toPromise();
    }


    /*this service used for change password*
    *
    * @param newPassword
    */
    changePassword(newPassword: PasswordModel) {
        return this.http.post(this.UserUrl + "/password", JSON.stringify(newPassword), { headers: this.headers })
            .map(res => res.json())
            .toPromise();
    }

    /*This service used for check email is already exists*
    *
    * @param email
    */
    checkEmailIsExists(email: string) {
        return this.http.get(this.UserUrl + "/available/email/" + email)
            .map(res => res.json())
            .toPromise();
    }

    /*This service used for check user is exists by slack username*
     *
     * @param slackUserName
     */
    checkUserIsExistsBySlackUserName(slackUserName: string) {
        return this.http.get(this.UserUrl + "/available/" + slackUserName)
            .map(res => res.json())
            .toPromise();
    }

    /*This service used for get user roles*
      *
      */
    getRoles() {
        return this.http.get(this.UserUrl + "/roles")
            .map(res => res.json())
            .toPromise();
    }

    /*This service used for resend mail to the user of the given id*
    *
    * @param id
    */
    reSendMail(id: string) {
        return this.http.get(this.UserUrl + "/email/" + id + "/send")
            .map(res => res.json())
            .toPromise();
    }

    /*This service used for check old password is valid or not*
      *
      * @param password
      */
    checkOldPasswordIsValid(password: string) {
        return this.http.get(this.UserUrl + "/" + password + "/available")
            .map(res => res.json())
            .toPromise();
    }
}
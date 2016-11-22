
import { Injectable } from '@angular/core';
import { ResponseOptions, Response } from "@angular/http";
import { UserModel } from "../../../users/user.model";
import { PasswordModel } from "../../../users/user-password.model";
import { Subject } from 'rxjs/Rx';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';



@Injectable()
export class MockUserService {
    //private UserUrl = 'api/user';
    constructor() { }

    getUsers() {
        let mockUser = new MockUser();
        mockUser.FirstName = "First Name";
        mockUser.LastName = "Last Name";
        mockUser.Email = "test@promactinfo.com";

        return new BehaviorSubject(mockUser).asObservable();

    }

    registerUser(newUser: UserModel) {
        if (newUser.Email === "ankit@promactinfo.com")
            return new BehaviorSubject(newUser.Email).asObservable();
        else
            return new BehaviorSubject(newUser.FirstName).asObservable();
    }

    getUserById(userId: string) {
        let mockUser = new MockUsers(userId);
        if (userId === "1") {
            mockUser.FirstName = "First Name";
            mockUser.LastName = "Last Name";
            mockUser.Email = "test@promactinfo.com";
        }
        return new BehaviorSubject(mockUser).asObservable();
        //return this.mockBaseService.get(this.UserUrl + "/" + userId);
    }

    editUser(editedUser: UserModel) {
        return new BehaviorSubject(editedUser).asObservable();
    }

    changePassword(newPassword: PasswordModel) {
        let result = newPassword.NewPassword;
        return new BehaviorSubject(result).asObservable();
    }

    ////    return this.mockBaseService.get(this.UserUrl + "/findbyusername/" + userName);
    ////}

    //findUserByEmail(email: string) {
    //    let isEmailExist = true;
    //    let connection = this.mockBaseService.getMockResponse(this.UserUrl, isEmailExist);
    //    return connection;
    //}

    getRoles() {
        let listOfRole = new Array<MockRole>();
        let mockRole = new MockRole();
        mockRole.Id = "1";
        mockRole.RoleName = "Employee";
        listOfRole.push(mockRole);
        //let connection = this.mockBaseService.getMockResponse(this.UserUrl, listOfRole);
        return new BehaviorSubject(listOfRole).asObservable();
    }

    checkOldPasswordIsValid() {
        return new BehaviorSubject(true).asObservable();
    }
}

class MockRole extends UserModel {

    constructor() {
        super();
        //this.Id = id;
    }
}

class MockUser extends UserModel {

    constructor() {
        super();
        //this.Id = id;
    }
}

class MockUsers extends UserModel {

    constructor(id: string) {
        super();
        this.Id = id;
    }

}
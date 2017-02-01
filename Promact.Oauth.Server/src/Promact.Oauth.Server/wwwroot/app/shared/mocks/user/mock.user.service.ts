import { Injectable } from '@angular/core';
import { ResponseOptions, Response } from "@angular/http";
import { UserModel } from "../../../users/user.model";
import { PasswordModel } from "../../../users/user-password.model";

@Injectable()
export class MockUserService {
    //private UserUrl = 'api/user';
    constructor() { }

    getUsers() {
        let mockUser = new MockUser();
        mockUser.FirstName = "First Name";
        mockUser.LastName = "Last Name";
        mockUser.Email = "test@promactinfo.com";

        return Promise.resolve(mockUser);

    }

    registerUser(newUser: UserModel) {
        if (newUser.Email === "ankit@promactinfo.com")
            return Promise.resolve(newUser.Email);
        else
            return Promise.resolve(newUser.FirstName);
    }


    getUserById(userId: string) {
        let mockUser = new MockUsers(userId);
        if (userId === "1") {
            mockUser.FirstName = "First Name";
            mockUser.LastName = "Last Name";
            mockUser.Email = "test@promactinfo.com";
        }
        return Promise.resolve(mockUser);
    }

    editUser(editedUser: UserModel) {
        return Promise.resolve(editedUser);
    }


    changePassword(newPassword: PasswordModel) {
        let result = newPassword.NewPassword;
        return Promise.resolve(result);
    }

    getRoles() {
        let listOfRole = new Array<MockRole>();
        let mockRole = new MockRole();
        mockRole.Id = "1";
        mockRole.RoleName = "Employee";
        listOfRole.push(mockRole);
        return Promise.resolve(listOfRole);
    }

    checkOldPasswordIsValid() {
        return Promise.resolve(true);
    }

}



class MockRole extends UserModel {

    constructor() {
        super();
    }
}

class MockUser extends UserModel {

    constructor() {
        super();
    }
}

class MockUsers extends UserModel {

    constructor(id: string) {
        super();
        this.Id = id;
    }

}
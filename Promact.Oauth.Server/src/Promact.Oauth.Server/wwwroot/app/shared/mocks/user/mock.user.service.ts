import { Injectable } from '@angular/core';
import { ResponseOptions, Response } from "@angular/http";
import { UserModel } from "../../../users/user.model";
import { PasswordModel } from "../../../users/user-password.model";
import { ErrorModel } from "../../../shared/error.model";

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
        if (newUser.FirstName === "") {
            let error = new MockError(400);
            return Promise.reject(error);
        }
        else {
            return Promise.resolve(newUser);
        }
    }

    getUserById(userId: string) {
        let mockUser = new MockUsers(userId);
        if (userId === "1") {
            mockUser.FirstName = "First Name";
            mockUser.LastName = "Last Name";
            mockUser.Email = "test@promactinfo.com";
            return Promise.resolve(mockUser);
        }
        else {
            return Promise.reject(mockUser)
        };
    }


    userDelete(userId: string) {
        if (userId === "1") {
            return Promise.resolve("");
        }
        else {
            return Promise.resolve("User could not be deleted");
        }
    }


    reSendMail(user: UserModel) {
        return Promise.resolve();
    }


    editUser(editedUser: UserModel) {

        if (editedUser.FirstName === "") {
            let error = new MockError(404);
            return Promise.reject(error);
        }
        else {
            return Promise.resolve(editedUser);
        }
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


    checkEmailIsExists() {
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


class MockError extends ErrorModel {

    constructor(status: number) {
        super();
        this.status = status;
    }
}

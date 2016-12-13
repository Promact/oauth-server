
import { Injectable } from '@angular/core';
import { ResponseOptions, Response } from "@angular/http";
import { UserModel } from "../../../users/user.model";
import { PasswordModel } from "../../../users/user-password.model";
import { Subject } from 'rxjs/Rx';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { StringConstant } from '../../stringconstant';


@Injectable()
export class MockUserService {
    stringConstant: StringConstant = new StringConstant();
    constructor() { }

    getUsers() {
        let mockUser = new MockUser();
        mockUser.FirstName = this.stringConstant.testfirstName;
        mockUser.LastName = this.stringConstant.lastName;
        mockUser.Email = this.stringConstant.email;

        return new BehaviorSubject(mockUser).asObservable();

    }

    registerUser(newUser: UserModel) {
        if (newUser.Email === this.stringConstant.email)
            return new BehaviorSubject(newUser.Email).asObservable();
        else
            return new BehaviorSubject(newUser.FirstName).asObservable();
    }

    getUserById(userId: string) {
        let mockUser = new MockUsers(userId);
        if (userId === this.stringConstant.id) {
            mockUser.FirstName = this.stringConstant.testfirstName;
            mockUser.LastName = this.stringConstant.lastName;
            mockUser.Email = this.stringConstant.email;
        }
        return new BehaviorSubject(mockUser).asObservable();
    }

    editUser(editedUser: UserModel) {
        return new BehaviorSubject(editedUser).asObservable();
    }

    changePassword(newPassword: PasswordModel) {
        let result = newPassword.NewPassword;
        return new BehaviorSubject(result).asObservable();
    }
    getRoles() {
        let listOfRole = new Array<MockRole>();
        let mockRole = new MockRole();
        mockRole.Id = this.stringConstant.id;
        mockRole.RoleName = this.stringConstant.employee;
        listOfRole.push(mockRole);
        return new BehaviorSubject(listOfRole).asObservable();
    }

    checkOldPasswordIsValid() {
        return new BehaviorSubject(true).asObservable();
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
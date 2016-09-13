import {TestConnection} from "../test.connection";
import {Injectable} from '@angular/core';
import {ResponseOptions, Response} from "@angular/http";
import {UserModel} from '../../../users/user.model';
import {PasswordModel} from "../../../users/user-password.model";
import {Md2Toast} from 'md2/toast';
import {Subject} from 'rxjs/Rx';
import {MockBaseService} from '../mock.base';



@Injectable()
export class MockUserService {
    private UserUrl = 'api/user';
    constructor(private mockBaseService: MockBaseService) { }

    getUsers() {
        let mockUser = new MockUser();
        mockUser.FirstName = "First Name";
        mockUser.LastName = "Last Name";
        mockUser.Email = "test@promactinfo.com";
        let connection = this.mockBaseService.getMockResponse(this.UserUrl, mockUser);
        return connection;
        
    }

    registerUser(newUser: UserModel) {
        let connection = this.mockBaseService.getMockResponse(this.UserUrl, newUser);
        return connection;
    }

    getUserById(userId: string) {
        let mockUser = new MockUsers(userId);
        if (userId === "1") {
            mockUser.FirstName = "First Name";
            mockUser.LastName = "Last Name";
            mockUser.Email = "test@promactinfo.com";
        }
        let connection = this.mockBaseService.getMockResponse(this.UserUrl + userId, mockUser);
        return connection;
        //return this.mockBaseService.get(this.UserUrl + "/" + userId);
    }

    editUser(editedUser: UserModel) {
        let connection = this.mockBaseService.getMockResponse(this.UserUrl, editedUser);
        return connection;
    }

    changePassword(newPassword: PasswordModel) {
        let result = true;
        let connection = this.mockBaseService.getMockResponse(this.UserUrl, result);
        return connection;
        //return this.mockBaseService.post(this.UserUrl + "/changePassword", newPassword);
    }

    //findUserByUserName(userName: string) {

    //    return this.mockBaseService.get(this.UserUrl + "/findbyusername/" + userName);
    //}

    findUserByEmail(email: string) {
        let isEmailExist = true;
        let connection = this.mockBaseService.getMockResponse(this.UserUrl, isEmailExist);
        return connection;
    }

    getRoles() {
        let listOfRole = new Array<MockRole>();
        let mockRole = new MockRole();
        mockRole.Id = "1";
        mockRole.RoleName = "Employee";
        listOfRole.push(mockRole);
        let connection = this.mockBaseService.getMockResponse(this.UserUrl, listOfRole);
        return connection;
    }
    

}

class MockRole extends UserModel
{
    constructor() {
        super();
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
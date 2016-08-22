import {TestConnection} from "../test.connection";
import {Injectable} from '@angular/core';
import {ResponseOptions, Response} from "@angular/http";
import {projectModel} from "../../../project/project.model";
import {UserModel} from "../../../users/User.model";
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

    //changePassword(newPassword: PasswordModel) {
    //    return this.mockBaseService.post(this.UserUrl + "/changePassword", newPassword);
    //}

    //findUserByUserName(userName: string) {
    //    return this.mockBaseService.get(this.UserUrl + "/findbyusername/" + userName);
    //}

    //findUserByEmail(email: string) {
    //    return this.mockBaseService.get(this.UserUrl + "/findbyemail/" + email);
    //}


    

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
import { Injectable } from '@angular/core';

@Injectable()
export class StringConstant {
    constructor() { }
    admin = 'Admin';
    employee = 'Employee';
    projectUrl = 'api/project';
    userUrl = 'api/users';
    consumerAppUrl = 'api/consumerapp';
    paramsId = 'id';
    id = '1';
    emailExtension = '@promactinfo.com';
    projectName = 'test project';
    slackChannelName = 'Test Slack Name';
    firstName = 'test';
    testfirstName = "First Name";
    lastName = 'shah';
    email = 'test@abc.com';
    teamLeaderId = '2';
    consumerappname = 'slack';
    description = 'slack description';
    callbackUrl = 'www.google.com';
    loginUrl = 'www.google.com';
    authSecret = 'dsdsdsdsdsdsd';
    authId = 'ASASs5454545455';
    consumerAppExpectedValue = 'SFDASFADSFSAD';
    newPassword = 'test123';
    oldPassword = 'test';
    confirmPassword = 'test1234';
    medium = "medium";
    navigate = "navigate";
    dateFormat = "dd-MM-yyyy";
    userRole = "employee";
    testString = "test";
    changePassword = "changePassword";
    checkOldPasswordIsValid = "checkOldPasswordIsValid";
}
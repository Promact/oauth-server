import { Injectable } from '@angular/core';


@Injectable()
export class StringConstant {
    constructor() { }
    admin = 'Admin';
    employee = 'Employee';
    projectUrl = 'api/project';
    userUrl = 'api/user';
    slash = '/';
    getEmployees = '/getEmployees';
    checkDuplicate = '/checkDuplicate';
    paramsId = 'id';
    id = '1';
    projectEdit = '/project/edit';
    projectView = '/project/view';
    projectList = '/project/list';
    userList = 'user/list';
    userDetail = '/user/details/';
    userEdit = '/user/edit';
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
    authSecret = 'dsdsdsdsdsdsd';
    authId = 'ASASs5454545455';
    consumerapp = '/consumerapp';
    consumerappEdit = '/consumerapp/edit';
    consumerappAdd = '/consumerapp/add';
    newPassword = 'test123';
    oldPassword = 'test';
    confirmPassword = 'test1234';
    consumerAppUrl = 'api/consumerapp';
}
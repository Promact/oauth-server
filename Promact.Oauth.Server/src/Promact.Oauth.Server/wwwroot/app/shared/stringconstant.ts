import { Injectable } from '@angular/core';


@Injectable()
export class StringConstant {
    constructor() { }
    admin = 'Admin';
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
    userEdit ='/user/edit'
    emailExtension = '@promactinfo.com';
}
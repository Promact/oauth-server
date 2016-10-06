import { Injectable } from '@angular/core';
import { projectModel } from "../../../project/project.model";
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { UserModel } from "../../../users/user.model";

@Injectable()
export class MockProjectService {
    projects: Array<projectModel> = new Array<projectModel>();
    constructor() {
        let mockProject = new projectModel();
        mockProject.name = "slack1";
        mockProject.SlackChannelName = "slack.test";
        this.projects.push(mockProject);
    }
    getProjects() {
        return new BehaviorSubject(this.projects).asObservable();
    }
    addProject(projectModel: projectModel)
    {
        
        //this.projects.push(projectModel);
        return new BehaviorSubject(projectModel).asObservable();
    }
    editProject(projectModel: projectModel)
    {
        //let connection = this.mockBaseService.getMockResponse(this.projectUrl, projectModel);
        //return connection;
        return new BehaviorSubject(projectModel).asObservable();
    }
    getUsers() {
        let mockUser = new UserModel();
        mockUser.FirstName = "Ronak";
        mockUser.LastName = "Shah";
        mockUser.Email = "rshah@Promactinfo.com";
        mockUser.IsActive = true;
        let mockList = new Array<UserModel>();
        //let connection = this.mockBaseService.getMockResponse(this.userUrl, mockList.push(mockUser));
        return new BehaviorSubject(mockUser).asObservable();
    }

        getProject(Id: number) {
        let mockProject = new MockProjects(Id);
        if (Id === 1) {
           mockProject.name = "Project";
           mockProject.SlackChannelName = "Slack Channel"
           let mockUser = new UserModel();
            mockUser.FirstName = "Ronakfdfas";
            mockUser.LastName = "Shahfdsaf";
            mockUser.Email = "rshah@Promactinfofdsfs.com";
            mockUser.IsActive = true;
            mockUser.Id = "3";
            let mockList = new Array<UserModel>();
            mockList.push(mockUser);
            mockProject.applicationUsers = mockList;
            mockProject.teamLeaderId = "2";
            return new BehaviorSubject(mockProject).asObservable();
        }
        //let connection = this.mockBaseService.getMockResponse(this.projectUrl + Id, mockProject);
      // return connection;
   }

}



//import {TestConnection} from "../test.connection";
//import {Injectable} from '@angular/core';
//import {ResponseOptions, Response} from "@angular/http";
//import {projectModel} from "../../../project/project.model";
//import { UserModel } from "../../../users/user.model";
////import {Md2Toast} from 'md2/toast';
//import {Subject} from 'rxjs/Rx';
////import {MockBaseService} from '../mock.base';



//@Injectable()
//export class MockProjectService {
//    private projectUrl = "api/project";
//    private userUrl="api/users"
//    constructor() { }
    
//    getProjects() {
//        let mockProject = new MockProject();
//        let mockProjectList = new Array<MockProject>();
//        mockProject.name = "slack";
//        mockProject.SlackChannelName = "slack.test";
//        mockProjectList.push(mockProject);
//        //let connection = this.mockBaseService.getMockResponse(this.projectUrl, mockProjectList);
//        //return connection;
//    }
//    addProject(projectModel: projectModel)
//    {
//        //let connection = this.mockBaseService.getMockResponse(this.projectUrl, projectModel);
//        //return connection;
//    }
//    editProject(projectModel: projectModel)
//    {
//        //let connection = this.mockBaseService.getMockResponse(this.projectUrl, projectModel);
//        //return connection;
//    }
//    getProject(Id: number) {
//        let mockProject = new MockProjects(Id);
//        if (Id === 1) {
//           mockProject.name = "Project";
//           mockProject.SlackChannelName="Slack Channel"
//        }
//        //let connection = this.mockBaseService.getMockResponse(this.projectUrl + Id, mockProject);
//      // return connection;
//   }


//    getUsers() {
//        let mockProject = new MockProject();
//        let mockUser = new UserModel();
//        mockUser.FirstName = "Ronak";
//        mockUser.LastName = "Shah";
//        mockUser.Email = "rshah@Promactinfo.com";
//        mockUser.IsActive = true;
//        let mockList = new Array<UserModel>();
//        //let connection = this.mockBaseService.getMockResponse(this.userUrl, mockList.push(mockUser));
//        //return connection;
//    }
//}
    class MockProjects extends projectModel {
        
    constructor(id: number) {
        super();
        this.Id = id;
        }
        
}
class MockProject extends projectModel {

    constructor() {
        super();
        //this.Id = id;
    }
}


import { Injectable } from '@angular/core';
import { ProjectModel } from "../../../project/project.model";
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { UserModel } from "../../../users/user.model";

@Injectable()
export class MockProjectService {
    projects: Array<ProjectModel> = new Array<ProjectModel>();
    constructor() {
        let mockProject = new ProjectModel();
        mockProject.name = "slack1";
        mockProject.slackChannelName = "slack.test";
        this.projects.push(mockProject);
    }
    getProjects() {
        return new BehaviorSubject(this.projects).asObservable();
    }
    addProject(projectModel: ProjectModel) {
        projectModel.slackChannelName = null;
        return new BehaviorSubject(projectModel).asObservable();
    }
    editProject(projectModel: ProjectModel) {
        projectModel.slackChannelName = null;
        return new BehaviorSubject(projectModel).asObservable();
    }
    getUsers() {
        let mockUser = new UserModel();
        mockUser.FirstName = "Ronak";
        mockUser.LastName = "Shah";
        mockUser.Email = "rshah@Promactinfo.com";
        mockUser.IsActive = true;
        let mockList = new Array<UserModel>();
        return new BehaviorSubject(mockUser).asObservable();
    }

        getProject(Id: number) {
        let mockProject = new MockProjects(Id);
        if (Id === 1) {
           mockProject.name = "Project";
           mockProject.slackChannelName = "Slack Channel";
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
            mockProject.teamLeader = mockUser;
            return new BehaviorSubject(mockProject).asObservable();
        }
   }

}




    class MockProjects extends ProjectModel {
        
    constructor(id: number) {
        super();
        this.id = id;
        }
        
}
class MockProject extends ProjectModel {

    constructor() {
        super();
        
    }
}


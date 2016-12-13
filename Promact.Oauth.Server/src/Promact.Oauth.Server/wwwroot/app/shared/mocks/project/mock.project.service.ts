import { Injectable } from '@angular/core';
import { ProjectModel } from "../../../project/project.model";
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { UserModel } from "../../../users/user.model";
import { StringConstant } from '../../stringconstant';

@Injectable()
export class MockProjectService {
    projects: Array<ProjectModel> = new Array<ProjectModel>();
    stringConstant: StringConstant = new StringConstant();
    constructor() {
        let mockProject = new ProjectModel();
        mockProject.name = this.stringConstant.projectName;
        mockProject.slackChannelName = this.stringConstant.slackChannelName;
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
        mockUser.FirstName = this.stringConstant.firstName;
        mockUser.LastName = this.stringConstant.lastName;
        mockUser.Email = this.stringConstant.email;
        mockUser.IsActive = true;
        let mockList = new Array<UserModel>();
        return new BehaviorSubject(mockUser).asObservable();
    }

        getProject(Id: number) {
        let mockProject = new MockProjects(Id);
        if (Id === 1) {
            mockProject.name = this.stringConstant.projectName;
            mockProject.slackChannelName = this.stringConstant.slackChannelName;
            let mockUser = new UserModel();
            mockUser.FirstName = this.stringConstant.firstName;
            mockUser.LastName = this.stringConstant.lastName;
            mockUser.Email = this.stringConstant.email;
            mockUser.IsActive = true;
            mockUser.Id = this.stringConstant.id;
            let mockList = new Array<UserModel>();
            mockList.push(mockUser);
            mockProject.applicationUsers = mockList;
            mockProject.teamLeaderId = this.stringConstant.paramsId;
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


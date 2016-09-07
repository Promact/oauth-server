import {TestConnection} from "../test.connection";
import {Injectable} from '@angular/core';
import {ResponseOptions, Response} from "@angular/http";
import {projectModel} from "../../../project/project.model";
import {UserModel} from "../../../users/User.model";
import {Md2Toast} from 'md2/toast';
import {Subject} from 'rxjs/Rx';
import {MockBaseService} from '../mock.base';



@Injectable()
export class MockProjectService {
    private projectUrl = "api/project";
    private userUrl="api/users"
    constructor(private mockBaseService: MockBaseService) { }
    
    getPros() {
        let mockProject = new MockProject();
        let mockProjectList = new Array<MockProject>();
        mockProject.Name = "slack";
        mockProject.SlackChannelName = "slack.test";
        mockProjectList.push(mockProject);
        let connection = this.mockBaseService.getMockResponse(this.projectUrl, mockProjectList);
        return connection;
    }
    addProject(projectModel: projectModel)
    {
        let connection = this.mockBaseService.getMockResponse(this.projectUrl, projectModel);
        return connection;
    }
    editProject(projectModel: projectModel)
    {
        let connection = this.mockBaseService.getMockResponse(this.projectUrl, projectModel);
        return connection;
    }
    getProject(Id: number) {
        let mockProject = new MockProjects(Id);
        if (Id === 1) {
           mockProject.Name = "Project";
           mockProject.SlackChannelName="Slack Channel"
        }
        let connection = this.mockBaseService.getMockResponse(this.projectUrl + Id, mockProject);
       return connection;
   }


    getUsers() {
        let mockProject = new MockProject();
        let mockUser = new UserModel();
        mockUser.FirstName = "Ronak";
        mockUser.LastName = "Shah";
        mockUser.Email = "rshah@Promactinfo.com";
        mockUser.IsActive = true;
        let mockList = new Array<UserModel>();
        let connection = this.mockBaseService.getMockResponse(this.userUrl, mockList.push(mockUser));
        return connection;
    }
}
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


import { Injectable } from '@angular/core';
import {HttpService} from "../http.service";
import 'rxjs/add/operator/toPromise';
import { projectModel } from './project.model';
@Injectable()
export class ProjectService {
    private ProjectUrl = 'api/project';  // URL to web api
    constructor(private httpService: HttpService<projectModel>) { }
    //check duplicate
    checkDuplicate(project: projectModel) {
        return this.httpService.post(this.ProjectUrl + "/checkDuplicate", project);
    }

    //list of users
    getUsers() {
        return this.httpService.get("api/user" + "/getEmployees");
    }
    //
    getProjects() {
        return this.httpService.get(this.ProjectUrl + "/");
    }
    getProject(id: number) {
        return this.httpService.get(this.ProjectUrl+ "/" +id);
    }
    addProject(project: projectModel) {
        return this.httpService.post(this.ProjectUrl + "/", project);
    }

    deleteProject(projectId: number) {
        return this.httpService.delete(this.ProjectUrl + "/deleteProject/" + projectId);
    }
    editProject(project: projectModel)
    {
        return this.httpService.put(this.ProjectUrl + "/", project);
    }
}

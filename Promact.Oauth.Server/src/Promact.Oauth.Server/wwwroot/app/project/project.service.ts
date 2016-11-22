import { Injectable } from '@angular/core';
import {HttpService} from "../http.service";
import 'rxjs/add/operator/toPromise';
import { ProjectModel } from './project.model';
@Injectable()
export class ProjectService {
    private ProjectUrl = 'api/project';  // URL to web api
    constructor(private httpService: HttpService<ProjectModel>) { }
    //check duplicate
    checkDuplicate(project: ProjectModel) {
        return this.httpService.post(this.ProjectUrl + "/checkDuplicate", project);
    }

    //list of users
    getUsers() {
        return this.httpService.get("api/user" + "/getEmployees");
    }
    //
    getProjects() {
        return this.httpService.get(this.ProjectUrl + "/getAllProjects");
    }
    getProject(id: number) {
        return this.httpService.get(this.ProjectUrl + "/getProjects/"+ id);
    }
    addProject(project: ProjectModel) {
        return this.httpService.post(this.ProjectUrl + "/addProject", project);
    }

    deleteProject(projectId: number) {
        return this.httpService.delete(this.ProjectUrl + "/deleteProject/" + projectId);
    }
    editProject(project: ProjectModel) {
        return this.httpService.put(this.ProjectUrl + "/editProject/", project);
    }
}

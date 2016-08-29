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
        return this.httpService.get("api/user" + "/users");
    }
    //
    getPros() {
        return this.httpService.get(this.ProjectUrl + "/projects");
    }
    getProject(id: number) {
        return this.httpService.get(this.ProjectUrl + "/getProjects/"+ id);
    }
    addProject(project: projectModel) {
        return this.httpService.post(this.ProjectUrl + "/addProject", project);
    }

    deleteProject(projectId: number) {
        return this.httpService.delete(this.ProjectUrl + "/deleteProject/" + projectId);
    }
    editProject(project: projectModel)
    {
        return this.httpService.put(this.ProjectUrl + "/editProject/", project);
    }
}

import { Injectable } from '@angular/core';
import {HttpService} from "../http.service";
import 'rxjs/add/operator/toPromise';
import { ProjectModel } from './project.model';
@Injectable()
export class ProjectService {
    private ProjectUrl = 'api/project';  // URL to web api
    private UserUrl = 'api/users';
    constructor(private httpService: HttpService<ProjectModel>) { }

    getUsers() {
        return this.httpService.get(this.UserUrl + "/orderby/name");
    }
    getProjects() {
        return this.httpService.get(this.ProjectUrl);
    }
    getProject(id: number) {
        return this.httpService.get(this.ProjectUrl+ "/" +id);
    }
    addProject(project: ProjectModel) {
        return this.httpService.post(this.ProjectUrl, project);
    }
    editProject(project: ProjectModel) {
        return this.httpService.put(this.ProjectUrl+"/"+ project.id, project);
    }
}

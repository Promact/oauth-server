import { Injectable } from '@angular/core';
import {HttpService} from "../http.service";
import 'rxjs/add/operator/toPromise';
import { ProjectModel } from './project.model';
import { StringConstant } from '../shared/stringconstant';

@Injectable()
export class ProjectService {
    private ProjectUrl = 'api/project';  // URL to web api
    private UserUrl = 'api/users';
    constructor(private httpService: HttpService<ProjectModel>) { }
    getUsers() {
        return this.httpService.get(this.UserUrl + "/orderby/name");
    }
    getProjects() {
        return this.httpService.get(this.stringConstant.projectUrl);
    }
    getProject(id: number) {
        return this.httpService.get(this.stringConstant.projectUrl + this.stringConstant.slash + id);
    }
    addProject(project: ProjectModel) {
        return this.httpService.post(this.stringConstant.projectUrl, project);
    }
    editProject(project: ProjectModel) {
        return this.httpService.put(this.ProjectUrl+"/"+ project.id, project);
    }
}

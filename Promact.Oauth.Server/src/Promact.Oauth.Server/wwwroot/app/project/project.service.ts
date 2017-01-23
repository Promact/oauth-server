import { Injectable } from '@angular/core';
import { HttpService } from "../http.service";
import 'rxjs/add/operator/toPromise';
import { ProjectModel } from './project.model';
import { StringConstant } from '../shared/stringconstant';

@Injectable()
export class ProjectService {

    constructor(private httpService: HttpService<ProjectModel>, private stringConstant: StringConstant) { }
    getUsers() {
        return this.httpService.get(this.stringConstant.userUrl + "/orderby/name");
    }
    getProjects() {
        return this.httpService.get(this.stringConstant.projectUrl);
    }
    getProject(id: number) {
        return this.httpService.get(this.stringConstant.projectUrl + "/" + id);
    }
    addProject(project: ProjectModel) {
        return this.httpService.post(this.stringConstant.projectUrl, project);
    }
    editProject(project: ProjectModel) {
        return this.httpService.put(this.stringConstant.projectUrl + "/" + project.id, project);
    }
}

import { Injectable } from '@angular/core';
import {HttpService} from "../http.service";
import 'rxjs/add/operator/toPromise';
import { ProjectModel } from './project.model';
import { StringConstant } from '../shared/stringconstant';

@Injectable()
export class ProjectService {
   
    constructor(private httpService: HttpService<ProjectModel>, private stringConstant: StringConstant) { }
   
    checkDuplicate(project: ProjectModel) {
        return this.httpService.post(this.stringConstant.projectUrl + this.stringConstant.checkDuplicate, project);
    }

    getUsers() {
        return this.httpService.get(this.stringConstant.userUrl + this.stringConstant.getEmployees);
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
    editProject(project: ProjectModel)
    {
        return this.httpService.put(this.stringConstant.projectUrl + this.stringConstant.slash+ project.id, project);
    }
}

import { Injectable } from '@angular/core';
import {HttpService} from "./http.service";
import 'rxjs/add/operator/toPromise';
import { Pro } from './Pro';
import { PROS } from './mock-project';
@Injectable()
export class ProService {
    private ProjectUrl = 'api/project';  // URL to web api
    constructor(private httpService: HttpService<Pro>) { }
    getPros() {
        return this.httpService.get(this.ProjectUrl+"/projects");
    }

    addProject(project: Pro) {
        return this.httpService.post(this.ProjectUrl +"/addProject", project);
    }

    deleteProject(projectId: number) {
        return this.httpService.delete(this.ProjectUrl +"/deleteProject/" + projectId);
    }
    //deleteProject(id) {
    //    return this.httpService.delete("api/project/deleteProject/id");
    //}

}

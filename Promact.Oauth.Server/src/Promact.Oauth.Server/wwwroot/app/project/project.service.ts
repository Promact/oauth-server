import { Injectable } from '@angular/core';
import { Http, Headers, RequestOptions, Response } from "@angular/http";
import 'rxjs/add/operator/toPromise';
import { ProjectModel } from './project.model';


@Injectable()
export class ProjectService {
    private headers = new Headers({ 'Content-Type': 'application/json' });
    private ProjectUrl = 'api/project';  // URL to web api
    private UserUrl = 'api/user';
    constructor(private http: Http) { }

    /*This service used for get users*
    * /
    * @param no
    */
    getUsers() {
        return this.http.get(this.UserUrl + "/orderby/name")
            .map(res => res.json())
            .toPromise();
    }

    /*This service used for get projects*
     * /
     * @param no
     */
    getProjects(): Promise<ProjectModel[]> {
        return this.http.get(this.ProjectUrl)
            .map(res => res.json())
            .toPromise();
    }

   
    /*This service get project by id.*
      *
      * @param id
      */
    getProject(id: number) {
        return this.http.get(this.ProjectUrl + "/" + id)
            .map(res => res.json())
            .toPromise();
    }

    /*This service used for add new peoject*
    *
    * @param project
    */
    addProject(project: ProjectModel) {
        return this.http.post(this.ProjectUrl, JSON.stringify(project), { headers: this.headers })
            .map(res => res.json())
            .toPromise();
    }

    /*This service used for update project*
     *
     * @param project
     */
    editProject(project: ProjectModel) {
        return this.http.put(this.ProjectUrl + "/" + project.id, JSON.stringify(project), { headers: this.headers })
            .map(res => res.json())
            .toPromise();
    }
}

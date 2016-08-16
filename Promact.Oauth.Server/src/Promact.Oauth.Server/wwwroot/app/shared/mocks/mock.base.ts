import {TestConnection} from "./test.connection";
import {Injectable} from '@angular/core';
import {ResponseOptions, Response} from "@angular/http";
import {projectModel} from "../../project/project.model";

@Injectable()

export class MockBaseService {
    constructor(private connection: TestConnection) { }
    //This is used to get the mock response
    getMockResponse(api: string, mockBody: boolean | string | number | projectModel | Array<projectModel>) {
        let connection = this.connection.mockConnection(api);
        let response = new Response(new ResponseOptions({ body: mockBody }));
        //sends mock response to connection
        connection.mockRespond(response.json());
        return connection.response;
    }
}
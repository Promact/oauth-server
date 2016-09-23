
import {MockBackend, MockConnection} from "@angular/http/testing";
import {ReflectiveInjector} from "@angular/core";
import { Http, XHRBackend} from "@angular/http";
import { Injectable } from '@angular/core';
declare var HTTP_BINDINGS, bind;

@Injectable()
export class TestConnection {
    mockConnection(url: string) {
        let example: any;
        let connection: MockConnection;
        let injector = ReflectiveInjector.resolveAndCreate([
            HTTP_BINDINGS,
            MockBackend,
            bind(XHRBackend).toAlias(MockBackend)
        ]);

        let backend = injector.get(MockBackend);
        let http = injector.get(Http);
        //Assign any newly-created connection to local variable
        backend.connections.subscribe((c: MockConnection) => {
            connection = c;
        });
        http.request(url);
        return connection;
        
    }
}


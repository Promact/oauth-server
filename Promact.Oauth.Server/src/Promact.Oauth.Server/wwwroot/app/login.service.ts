//import {Http, Headers, RequestOptions, Response} from "@angular/http";
import {Injectable} from "@angular/core";
import {Observable} from 'rxjs/Observable';
import {HttpService} from "./http.service";
import { LoginModel } from './LoginModel';


@Injectable()
export class LoginService {

    constructor(private http: HttpService<LoginModel>) {

    }
    getRoleAsync() {
        return this.http.get("Account/IsInRole");
    }
}
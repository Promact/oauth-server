import {Injectable} from "@angular/core";
import {Observable} from 'rxjs/Observable';
import {HttpService} from "./http.service";
import { LoginModel } from './login.model';


@Injectable()
export class LoginService {

    constructor(private http: HttpService<LoginModel>) {

    }
    getRoleAsync() {
        return this.http.get("Account/IsInRole");
    }
}
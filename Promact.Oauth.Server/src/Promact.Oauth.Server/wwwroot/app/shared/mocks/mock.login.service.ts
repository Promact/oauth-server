import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { LoginModel } from '../../login.model';

@Injectable()
export class MockLoginService {

    getRoleAsync() {
        let loginModel = new LoginModel();
        loginModel.userId = "1";
        loginModel.role = "Admin";
        return new BehaviorSubject(loginModel).asObservable();
    }
}
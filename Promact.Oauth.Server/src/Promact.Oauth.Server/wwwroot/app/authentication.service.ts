import { Injectable } from "@angular/core";
import { CanActivate} from "@angular/router";
import { UserRole } from "./shared/userrole.model";
import { StringConstant } from './shared/stringconstant';
import { Md2Toast } from 'md2';

@Injectable()
export class AuthenticationService implements CanActivate {
    constructor(private userRole: UserRole, private stringconstant: StringConstant, private toast : Md2Toast) {

    }
    canActivate(): boolean {
        if (this.userRole.Role === this.stringconstant.admin) {
            return true;
        }
        else {
            this.toast.show("You are not authorized to view this page");
            return false;
        }
    }
}





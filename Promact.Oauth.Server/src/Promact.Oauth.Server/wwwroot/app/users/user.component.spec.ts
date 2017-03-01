declare var describe, it, beforeEach, expect;
import { async, TestBed } from '@angular/core/testing';
import { Provider } from "@angular/core";
import { Router, RouterModule, Routes } from '@angular/router';
import { MockRouter } from '../shared/mocks/mock.router';
import { UserRole } from "../shared/userrole.model";
import { UserComponent } from './user.component';
import { UserModule } from './user.module';
import { StringConstant } from '../shared/stringconstant';

describe('App Component Test', () => {
    const routes: Routes = [];

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            imports: [UserModule, RouterModule.forRoot(routes, { useHash: true })],
            providers: [
                { provide: Router, useClass: MockRouter },
                { provide: UserRole, useClass: UserRole },
                { provide: StringConstant, useClass: StringConstant }]
        }).compileComponents();
    }));

    it("Load app Component", () => {
        let fixture = TestBed.createComponent(UserComponent);
        let comp = fixture.componentInstance;
    });
}); 
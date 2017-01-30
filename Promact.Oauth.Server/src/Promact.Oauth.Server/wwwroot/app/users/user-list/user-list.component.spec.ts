declare var describe, it, beforeEach, expect;
import {async,  TestBed, ComponentFixture} from '@angular/core/testing';
import { UserService }   from '../user.service';
import {UserModel} from '../user.model';
import { RouterModule, Routes } from '@angular/router';
import { Md2Toast } from 'md2';
import {UserListComponent} from '../user-list/user-list.component';
import {MockToast} from "../../shared/mocks/mock.toast";
import {MockUserService} from "../../shared/mocks/user/mock.user.service";
import { UserModule } from '../user.module';
import { LoaderService } from '../../shared/loader.service';

let fixture: ComponentFixture<UserListComponent>;

describe("User List Test", () => {
     
    const routes: Routes = [];
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            imports: [UserModule, RouterModule.forRoot(routes, { useHash: true }) //Set LocationStrategy for component. 
            ],
            providers: [
                { provide: UserService, useClass: MockUserService },
                { provide: Md2Toast, useClass: MockToast },
                { provide: LoaderService, useClass: LoaderService }]
        }).compileComponents();
    }));

    it("should be defined userListComponent", () => {
        let fixture = TestBed.createComponent(UserListComponent);
        let userListComponent = fixture.componentInstance;
        expect(userListComponent).toBeDefined();
    });

    it("should get default Users for company", () => {
            let fixture = TestBed.createComponent(UserListComponent); //Create instance of component            
            let userListComponent = fixture.componentInstance;
            expect(userListComponent.getUsers());
      });
});





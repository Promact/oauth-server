declare var describe, it, beforeEach, expect;
import { async, TestBed } from '@angular/core/testing';
import { Provider } from "@angular/core";
import { Router, RouterModule, Routes } from '@angular/router';
import { MockRouter } from './shared/mocks/mock.router';
import { UserRole } from "./shared/userrole.model";
import { LoaderService } from './shared/loader.service';
import { RouterLinkStubDirective } from './shared/mocks/mock.routerLink';
import { AppComponent } from './app.component';
import { AppModule } from './app.module';
import { StringConstant } from './shared/stringconstant';

describe('App Component Test', () => {
    const routes: Routes = [];

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [RouterLinkStubDirective],
            imports: [AppModule, RouterModule.forRoot(routes, { useHash: true })],
            providers: [
                { provide: Router, useClass: MockRouter },
                { provide: UserRole, useClass: UserRole },
                { provide: LoaderService, useClass: LoaderService },
                { provide: StringConstant, useClass: StringConstant }]
        }).compileComponents();
    }));

    it("Load app Component", () => {
        let fixture = TestBed.createComponent(AppComponent);
        let comp = fixture.componentInstance;
        expect(comp).toBeDefined();
    });
});
declare var describe, it, beforeEach, expect;
import { async, inject, TestBed, ComponentFixture } from '@angular/core/testing';
import { Provider } from "@angular/core";
import { Router, ActivatedRoute, RouterModule, Routes } from '@angular/router';
import { Md2Toast } from 'md2';
import { MockRouter } from './shared/mocks/mock.router';
import { UserRole } from "./shared/userrole.model";
import { LoaderService } from './shared/loader.service';
import { RouterLinkStubDirective } from './shared/mocks/mock.routerLink';
import { AppComponent } from './app.component';
import { AppModule } from './app.module';

describe('User Add Test', () => {
    class MockLoaderService { }
    const routes: Routes = [];

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [RouterLinkStubDirective],
            imports: [AppModule, RouterModule.forRoot(routes, { useHash: true })],
            providers: [
                { provide: Router, useClass: MockRouter },
                { provide: UserRole, useClass: UserRole },
                { provide: LoaderService, useClass: MockLoaderService }]
        }).compileComponents();
    }));

    it("Load app Component", () => {
        let fixture = TestBed.createComponent(AppComponent);
        let comp = fixture.componentInstance;
    });
});


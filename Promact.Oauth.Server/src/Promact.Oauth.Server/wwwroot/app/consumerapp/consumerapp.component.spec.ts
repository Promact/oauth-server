declare var describe, it, beforeEach, expect;
import { async, TestBed } from '@angular/core/testing';
import { Router, RouterModule, Routes } from '@angular/router';
import { MockRouter } from '../shared/mocks/mock.router';
import { RouterLinkStubDirective } from '../shared/mocks/mock.routerLink';
import { ConsumerAppModule } from './consumerapp.module';
import { ConsumerAppComponent } from './consumerapp.component';

describe('Project Component Test', () => {
    const routes: Routes = [];

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [RouterLinkStubDirective],
            imports: [ConsumerAppModule, RouterModule.forRoot(routes, { useHash: true })],
            providers: [
                { provide: Router, useClass: MockRouter }]
        }).compileComponents();
    }));

    it("Load consumerApp Component", () => {
        let fixture = TestBed.createComponent(ConsumerAppComponent);
        let consumerAppComponent = fixture.componentInstance;
        expect(consumerAppComponent).toBeDefined();
    });
});
declare var describe, it, beforeEach, expect;
import { async, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { ConsumerAppModel } from "../consumerapp-model";
import { ConsumerappListComponent } from "../consumerapp-list/consumerapp-list.component";
import { ConsumerAppService } from "../consumerapp.service";
import { Router, RouterModule, Routes } from '@angular/router';
import { MockToast } from "../../shared/mocks/mock.toast";
import { MockConsumerappService } from "../../shared/mocks/consumerapp/mock.consumerapp.service";
import { ConsumerAppModule } from '../consumerapp.module';
import { LoaderService } from '../../shared/loader.service';
import { Md2Toast } from 'md2';
import { MockRouter } from '../../shared/mocks/mock.router';


describe('Consumer List Test', () => {
    const routes: Routes = [];

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            imports: [ConsumerAppModule, RouterModule.forRoot(routes, { useHash: true }) //Set LocationStrategy for component. 
            ],
            providers: [
                { provide: Router, useClass: MockRouter },
                { provide: ConsumerAppService, useClass: MockConsumerappService },
                { provide: Md2Toast, useClass: MockToast },
                { provide: ConsumerAppModel, useClass: ConsumerAppModel },
                { provide: LoaderService, useClass: LoaderService }
            ]
        }).compileComponents();
    }));

    it("Consumerapp list Component", () => {
        expect(ConsumerappListComponent).toBeDefined();
    });

    it("Get Consumer Apps", fakeAsync(() => {
        let fixture = TestBed.createComponent(ConsumerappListComponent); //Create instance of component            
        let consumerappListComponent = fixture.componentInstance;
        consumerappListComponent.getConsumerApps();
        tick();
        expect(consumerappListComponent.listOfConsumerApps.length).toEqual(1);
    }));

    it("Edit page call Details", fakeAsync(() => {
        let fixture = TestBed.createComponent(ConsumerappListComponent); //Create instance of component            
        let consumerappListComponent = fixture.componentInstance;
        consumerappListComponent.editDetails(1);
        tick();
    }));

    it("Add New App page call", fakeAsync(() => {
        let fixture = TestBed.createComponent(ConsumerappListComponent); //Create instance of component            
        let consumerappListComponent = fixture.componentInstance;
        consumerappListComponent.addNewApp();
        tick();
    }));

    it("OnInit", fakeAsync(() => {
        let fixture = TestBed.createComponent(ConsumerappListComponent); //Create instance of component            
        let consumerappListComponent = fixture.componentInstance;
        consumerappListComponent.ngOnInit();
        tick();
        expect(consumerappListComponent.listOfConsumerApps.length).toEqual(1);
    }));
});






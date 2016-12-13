declare var describe, it, beforeEach, expect;
import { async, inject, TestBed, ComponentFixture } from '@angular/core/testing';
import { Provider } from "@angular/core";
import { ConsumerAppModel } from "../consumerapp-model";
import { ConsumerappListComponent } from "../consumerapp-list/consumerapp-list.component";
import { ConsumerAppService } from "../consumerapp.service";
import { Router, ActivatedRoute, RouterModule, Routes } from '@angular/router';
import { MockToast } from "../../shared/mocks/mock.toast";
import { MockConsumerappService } from "../../shared/mocks/consumerapp/mock.consumerapp.service";
import { MockRouter } from '../../shared/mocks/mock.router';
import { Observable } from 'rxjs/Observable';
import { ConsumerAppModule } from '../consumerapp.module';
import { LoaderService } from '../../shared/loader.service';
import { Md2Toast } from 'md2';
import { StringConstant } from '../../shared/stringconstant';

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
                { provide: LoaderService, useClass: LoaderService },
                { provide: StringConstant, useClass: StringConstant }
            ]
        }).compileComponents();
    }));

    it("Consumerapp list Component", () => {
        expect(ConsumerappListComponent).toBeDefined();
    });

    it("Get Consumer Apps", () => {
            let fixture = TestBed.createComponent(ConsumerappListComponent); //Create instance of component            
            let consumerappListComponent = fixture.componentInstance;
            consumerappListComponent.getConsumerApps();
            expect(consumerappListComponent.listOfConsumerApps.length).toEqual(1);
    });
});






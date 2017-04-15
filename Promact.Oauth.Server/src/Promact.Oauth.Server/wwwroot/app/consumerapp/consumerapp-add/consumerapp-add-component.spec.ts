//declare var describe, it, beforeEach, expect;
//import { async, TestBed, fakeAsync, tick } from '@angular/core/testing';
//import { ConsumerAppModel, consumerappallowedscopes } from "../consumerapp-model";
//import { ConsumerappAddComponent } from "../consumerapp-add/consumerapp-add.component";
//import { ConsumerAppService } from "../consumerapp.service";
//import { Router, RouterModule, Routes } from '@angular/router';
//import { Md2Toast } from 'md2';
//import { MockToast } from "../../shared/mocks/mock.toast";
//import { MockConsumerappService } from "../../shared/mocks/consumerapp/mock.consumerapp.service";
//import { MockRouter } from '../../shared/mocks/mock.router';
//import { ConsumerAppModule } from '../consumerapp.module';
//import { LoaderService } from '../../shared/loader.service';
//import { StringConstant } from '../../shared/stringconstant';

//let stringConstant = new StringConstant();

//describe('Consumer Add Test', () => {
//    const routes: Routes = [];

//    beforeEach(async(() => {
//        TestBed.configureTestingModule({
//            imports: [ConsumerAppModule, RouterModule.forRoot(routes, { useHash: true }) //Set LocationStrategy for component. 
//            ],
//            providers: [
//                { provide: Router, useClass: MockRouter },
//                { provide: ConsumerAppService, useClass: MockConsumerappService },
//                { provide: Md2Toast, useClass: MockToast },
//                { provide: ConsumerAppModel, useClass: ConsumerAppModel },
//                { provide: LoaderService, useClass: LoaderService },
//                { provide: consumerappallowedscopes, useClas: consumerappallowedscopes }
//            ]
//        }).compileComponents();
//    }));

//    it("Added consumer app", fakeAsync(() => {
//        let fixture = TestBed.createComponent(ConsumerappAddComponent); //Create instance of component            
//        let consumerappAddComponent = fixture.componentInstance;
//        let toast = fixture.debugElement.injector.get(Md2Toast);
//        let consumerAppModel = new ConsumerAppModel();
//        consumerAppModel.Name = stringConstant.consumerappname;
//        consumerAppModel.LogoutUrl = stringConstant.callbackUrl;
//        consumerAppModel.CallbackUrl = stringConstant.callbackUrl;
//        consumerAppModel.AuthSecret = stringConstant.authSecret;
//        consumerAppModel.AuthId = stringConstant.authId;
//        consumerAppModel.Scopes = [consumerappallowedscopes.email, consumerappallowedscopes.openid];
//        consumerappAddComponent.submitApps(consumerAppModel);
//        tick();
//        expect(consumerAppModel.Id).toBe(1);
//    }));

//    it("Random number consumer app AuthId", fakeAsync(() => {
//        let fixture = TestBed.createComponent(ConsumerappAddComponent); //Create instance of component            
//        let consumerappAddComponent = fixture.componentInstance;
//        let toast = fixture.debugElement.injector.get(Md2Toast);
//        let expectedValue = stringConstant.consumerAppExpectedValue;
//        let consumerAppModel = new ConsumerAppModel();
//        consumerappAddComponent.getRandomNumber(true);
//        tick();
//        expect(consumerappAddComponent.consumerModel.AuthId).toBe(expectedValue);
//    }));

//    it("Random number consumer app AuthSecret", fakeAsync(() => {
//        let fixture = TestBed.createComponent(ConsumerappAddComponent); //Create instance of component            
//        let consumerappAddComponent = fixture.componentInstance;
//        let toast = fixture.debugElement.injector.get(Md2Toast);
//        let expectedValue = stringConstant.consumerAppExpectedValue;
//        let consumerAppModel = new ConsumerAppModel();
//        consumerappAddComponent.getRandomNumber(false);
//        tick();
//        expect(consumerappAddComponent.consumerModel.AuthSecret).toBe(expectedValue);
//    }));

//    it("Consumer app OnInit", fakeAsync(() => {
//        let fixture = TestBed.createComponent(ConsumerappAddComponent); //Create instance of component            
//        let consumerappAddComponent = fixture.componentInstance;
//        let toast = fixture.debugElement.injector.get(Md2Toast);
//        let expectedValue = "SFDASFADSFSAD";
//        let consumerAppModel = new ConsumerAppModel();
//        consumerappAddComponent.ngOnInit();
//        tick();
//        expect(consumerappAddComponent.consumerModel.AuthSecret).toBe(expectedValue);
//    }));

//    it("Consumer app ScopeRequiredValidator", fakeAsync(() => {
//        let fixture = TestBed.createComponent(ConsumerappAddComponent); //Create instance of component            
//        let consumerappAddComponent = fixture.componentInstance;
//        let toast = fixture.debugElement.injector.get(Md2Toast);
//        let expectedValue = "SFDASFADSFSAD";
//        let consumerAppModel = new ConsumerAppModel();
//        let scopes = new Array<consumerappallowedscopes>();
//        scopes.push(consumerappallowedscopes.email);
//        consumerappAddComponent.scopeOnChange(scopes);
//        tick();
//        expect(consumerappAddComponent.clientScopeIndicator).toBe(false);
//    }));

//    it("Consumer app ScopeRequiredValidatorForEmptyScope", fakeAsync(() => {
//        let fixture = TestBed.createComponent(ConsumerappAddComponent); //Create instance of component            
//        let consumerappAddComponent = fixture.componentInstance;
//        let toast = fixture.debugElement.injector.get(Md2Toast);
//        let expectedValue = "SFDASFADSFSAD";
//        let consumerAppModel = new ConsumerAppModel();
//        let scopes = new Array<consumerappallowedscopes>();
//        consumerappAddComponent.scopeOnChange(scopes);
//        tick();
//        expect(consumerappAddComponent.clientScopeIndicator).toBe(true);
//    }));
//});
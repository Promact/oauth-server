import {async, inject, TestBed, ComponentFixture, TestComponentBuilder} from "@angular/core/testing";
import {provide} from "@angular/core";
import { ROUTER_DIRECTIVES, Router} from "@angular/router";
import {Md2Toast} from "md2/toast";
import { DeprecatedFormsModule } from '@angular/common';
import { ConsumerappAddComponent } from "../consumerapp-add/consumerapp-add.component";
import { ConsumerAppService} from "../consumerapp.service";
import {ConsumerAppModel} from "../consumerapp-model";
import {TestConnection} from "../../shared/mocks/test.connection";
import {MockToast} from "../../shared/mocks/mock.toast";
import { MockConsumerappService } from "../../shared/mocks/consumerapp/mock.consumerapp.service";
import {MockBaseService} from "../../shared/mocks/mock.base";
import {MockRouter} from "../../shared/mocks/mock.router";
declare var describe, it, beforeEach, expect;

describe("Consumerapp Test Case", function () {
    it("This is a spec with expectations", function () {
        var expectedValue = 12;
        var actualValue = 12;
        expect(expectedValue).toBe(actualValue);
    });
});


describe("Consumerapp Test Case", function () {
    it("This is a spec with expectations", function () {
        var expectedValue = 12;
        var actualValue = 12;
        expect(expectedValue).toBe(actualValue);
    });
});


describe("Consumerapp Test Case", function () {
    it("This is a spec with expectations", function () {
        var expectedValue = 12;
        var actualValue = 12;
        expect(expectedValue).toBe(actualValue);
    });
});


//    beforeEach(inject([ConsumerAppService, Router, Md2Toast], (consumerAppService: ConsumerAppService, router: Router, toast: Md2Toast) => {
//        consumerappAddComponent = new ConsumerappAddComponent(consumerAppService, router, toast);
//    }));
    
    it("consumerapp add method test", inject([ConsumerAppModel], (consumerAppModel: ConsumerAppModel) => {
        let expectedconsumerappname = "slack";
        let result = true;
        consumerAppModel.Name = expectedconsumerappname;
        consumerAppModel.Description = "slack description";
        consumerAppModel.CallbackUrl = "www.google.com";
        consumerAppModel.AuthSecret = "dsdsdsdsdsdsd";
        consumerAppModel.AuthId = "ASASs5454545455";
        consumerappAddComponent.submitApps(consumerAppModel);
        expect(consumerAppModel.Name).toBe(expectedconsumerappname);
    }));
});



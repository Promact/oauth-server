import {async, inject, TestBed, ComponentFixture, TestComponentBuilder} from "@angular/core/testing";
import {provide} from "@angular/core";
import { DeprecatedFormsModule } from "@angular/common";
import { ROUTER_DIRECTIVES, Router} from "@angular/router";
import {Md2Toast} from "md2/toast";
import { ConsumerappAddComponent } from "../consumerapp-add/consumerapp-add.component";
import { ConsumerAppService} from "../consumerapp.service";
import {ConsumerAppModel} from "../consumerapp-model";
import {TestConnection} from "../../shared/mocks/test.connection";
import {MockToast} from "../../shared/mocks/mock.toast";
import { MockConsumerappService } from "../../shared/mocks/consumerapp/mock.consumerapp.service";
import {MockBaseService} from "../../shared/mocks/mock.base";
import {MockRouter} from "../../shared/mocks/mock.router";





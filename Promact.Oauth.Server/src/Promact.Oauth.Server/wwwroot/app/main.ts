import { NgModule } from '@angular/core';
import {bootstrap} from "@angular/platform-browser-dynamic";
import {disableDeprecatedForms, provideForms, FormsModule} from '@angular/forms'; 
import {enableProdMode} from '@angular/core';
import { AppComponent } from "./app.component";
import { HTTP_PROVIDERS } from '@angular/http';
import { ROUTER_DIRECTIVES} from '@angular/router';
import {appRouterProviders} from "./app.routes";
enableProdMode();



bootstrap(AppComponent, [HTTP_PROVIDERS, appRouterProviders, disableDeprecatedForms(), provideForms()]);

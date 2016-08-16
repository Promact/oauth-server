import { NgModule } from '@angular/core';
import {bootstrap} from "@angular/platform-browser-dynamic";
import {enableProdMode} from '@angular/core';
import { AppComponent } from "./app.component";
enableProdMode();
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';
import { AppModule }              from '../app/app.module';

platformBrowserDynamic().bootstrapModule(AppModule);

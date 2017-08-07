import { platformBrowser } from '@angular/platform-browser';
import { AppModuleNgFactory } from './aot/wwwroot/app/app.module.ngfactory';
import {enableProdMode} from '@angular/core';

enableProdMode();

platformBrowser().bootstrapModuleFactory(AppModuleNgFactory);

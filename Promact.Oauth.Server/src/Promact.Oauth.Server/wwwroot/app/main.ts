import { bootstrap }    from "@angular/platform-browser-dynamic";
import {enableProdMode} from '@angular/core';
import { AppComponent } from "./app.component";
import { HTTP_PROVIDERS } from '@angular/http';
enableProdMode();
bootstrap(AppComponent, [HTTP_PROVIDERS]);

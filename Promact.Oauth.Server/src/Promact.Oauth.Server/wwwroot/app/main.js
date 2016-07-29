"use strict";
const platform_browser_dynamic_1 = require("@angular/platform-browser-dynamic");
const core_1 = require('@angular/core');
const app_component_1 = require("./app.component");
const http_1 = require('@angular/http');
const app_routes_1 = require("./app.routes");
core_1.enableProdMode();
platform_browser_dynamic_1.bootstrap(app_component_1.AppComponent, [http_1.HTTP_PROVIDERS, app_routes_1.appRouterProviders]);
//# sourceMappingURL=main.js.map
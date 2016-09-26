/*global jasmine, __karma__, window*/
Error.stackTraceLimit = Infinity;
jasmine.DEFAULT_TIMEOUT_INTERVAL = 1000;

__karma__.loaded = function () {
};


function isJsFile(path) {
    return path.slice(-3) === '.js';
}

function isSpecFile(path) {
    return path.slice(-8) === '.spec.js';
}

function isBuiltFile(path) {
    var builtPath = '/base/wwwroot/app/';
    return isJsFile(path) && (path.substr(0, builtPath.length) === builtPath);
}

var allSpecFiles = Object.keys(window.__karma__.files)
  .filter(isSpecFile)
  .filter(isBuiltFile);

// Load our SystemJS configuration.
System.config({
    baseURL: '/base'
});

System.config({
    packages: {
        'app': {
            main: 'main.js',
            defaultExtension: 'js'
        },
        'rxjs': {
            defaultExtension: 'js'
        },
        '@angular/router': {
            main: 'index.js',
            defaultExtension: 'js'
        },
        '@angular/http': {
            main: 'index.js',
            defaultExtension: 'js'
        },
      
        '@angular2-material/button': {
            format: 'cjs',
            defaultExtension: 'js',
            main: 'button.js'
        },
        '@angular2-material/card': {
            format: 'cjs',
            defaultExtension: 'js',
            main: 'card.js'
        },
        '@angular2-material/checkbox': {
            format: 'cjs',
            defaultExtension: 'js',
            main: 'checkbox.js'
        },
        '@angular2-material/input': {
            format: 'cjs',
            defaultExtension: 'js',
            main: 'input.js'
        },
        '@angular2-material/list': {
            format: 'cjs',
            defaultExtension: 'js',
            main: 'list.js'
        },
        '@angular2-material/progress-bar': {
            format: 'cjs',
            defaultExtension: 'js',
            main: 'progress-bar.js'
        },
        '@angular2-material/progress-circle': {
            format: 'cjs',
            defaultExtension: 'js',
            main: 'progress-circle.js'
        },
        '@angular2-material/radio': {
            format: 'cjs',
            defaultExtension: 'js',
            main: 'radio.js'
        },
        '@angular2-material/sidenav': {
            format: 'cjs',
            defaultExtension: 'js',
            main: 'sidenav.js'
        },
        '@angular2-material/slide-toggle': {
            format: 'cjs',
            defaultExtension: 'js',
            main: 'slide-toggle.js'
        },
        '@angular2-material/tabs': {
            format: 'cjs',
            defaultExtension: 'js',
            main: 'tabs.js'
        },
        '@angular2-material/toolbar': {
            format: 'cjs',
            defaultExtension: 'js',
            main: 'toolbar.js'
        },
        'md2/accordion': {
            format: 'cjs',
            defaultExtension: 'js',
            main: 'accordion.js'
        },
        'md2/autocomplete': {
            format: 'cjs',
            defaultExtension: 'js',
            main: 'autocomplete.js'
        },
        'md2/collapse': {
            format: 'cjs',
            defaultExtension: 'js',
            main: 'collapse.js'
        },
        'md2/colorpicker': {
            format: 'cjs',
            defaultExtension: 'js',
            main: 'colorpicker.js'
        },
        'md2/dialog': {
            format: 'cjs',
            defaultExtension: 'js',
            main: 'dialog.js'
        },
        'md2/menu': {
            format: 'cjs',
            defaultExtension: 'js',
            main: 'menu.js'
        },
        'md2/multiselect': {
            format: 'cjs',
            defaultExtension: 'js',
            main: 'multiselect.js'
        },
        'md2/select': {
            format: 'cjs',
            defaultExtension: 'js',
            main: 'select.js'
        },
        'md2/switch': {
            format: 'cjs',
            defaultExtension: 'js',
            main: 'switch.js'
        },
        'md2/tabs': {
            format: 'cjs',
            defaultExtension: 'js',
            main: 'tabs.js'
        },
        'md2/toast': {
            format: 'cjs',
            defaultExtension: 'js',
            main: 'toast.js'
        },
        'md2/tooltip': {
            format: 'cjs',
            defaultExtension: 'js',
            main: 'tooltip.js'
        },
    },

    map: {
        'app': 'wwwroot/app',
        '@angular/core': 'node_modules/@angular/core/bundles/core.umd.js',
        '@angular/common': 'node_modules/@angular/common/bundles/common.umd.js',
        '@angular/compiler': 'node_modules/@angular/compiler/bundles/compiler.umd.js',
        '@angular/platform-browser': 'node_modules/@angular/platform-browser/bundles/platform-browser.umd.js',
        '@angular/platform-browser-dynamic': 'node_modules/@angular/platform-browser-dynamic/bundles/platform-browser-dynamic.umd.js',
        '@angular2-material': 'node_modules/@angular2-material',
        // angular testing umd bundles
        '@angular/core/testing': 'node_modules/@angular/core/bundles/core-testing.umd.js',
        '@angular/common/testing': 'node_modules/@angular/common/bundles/common-testing.umd.js',
        '@angular/compiler/testing': 'node_modules/@angular/compiler/bundles/compiler-testing.umd.js',
        '@angular/platform-browser/testing': 'node_modules/@angular/platform-browser/bundles/platform-browser-testing.umd.js',
        '@angular/platform-browser-dynamic/testing': 'node_modules/@angular/platform-browser-dynamic/bundles/platform-browser-dynamic-testing.umd.js',
        '@angular/router/testing': 'npm:@angular/router/bundles/router-testing.umd.js',
        '@angular/forms/testing': 'npm:@angular/forms/bundles/forms-testing.umd.js',
        '@angular/http/testing': 'npm:@angular/http/bundles/http-testing.umd.js',
        'rxjs': 'node_modules/rxjs',
        '@angular': 'node_modules/@angular',

        'lodash': 'node_modules/lodash',
        'md2': 'node_modules/md2/src/components'
    }
});


Promise.all([
 System.import('@angular/core/testing'),
 System.import('@angular/platform-browser-dynamic/testing')
]).then(function (providers) {
    var testing = providers[0];
    var testingBrowser = providers[1];

    testing.TestBed.initTestEnvironment(testingBrowser.BrowserDynamicTestingModule,
      testingBrowser.platformBrowserDynamicTesting());

}).then(function () {
    // Finally, load all spec files.
    // This will run the tests directly.
    return Promise.all(
      allSpecFiles.map(function (moduleName) {
          return System.import(moduleName);
      }));
}).then(__karma__.start, __karma__.error);

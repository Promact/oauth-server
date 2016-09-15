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

let allSpecFiles = Object.keys(window.__karma__.files)
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
        '@angular/core': {
            main: 'index.js',
            defaultExtension: 'js'
        },
        '@angular/compiler': {
            main: 'index.js',
            defaultExtension: 'js'
        },
        '@angular/forms': {
            main: 'index.js',
            defaultExtension: 'js'
        },
        '@angular/common': {
            main: 'index.js',
            defaultExtension: 'js'
        },
        '@angular/platform-browser': {
            main: 'index.js',
            defaultExtension: 'js'
        },
        '@angular/platform-browser-dynamic': {
            main: 'index.js',
            defaultExtension: 'js'
        },
        '@angular/http': {
            main: 'index.js',
            defaultExtension: 'js'
        },
        '@angular/router-deprecated': {
            main: 'index.js',
            defaultExtension: 'js'
        },
        '@angular/router': {
            main: 'index.js',
            defaultExtension: 'js'
        },
        '@angular2-material/core': {
            format: 'cjs',
            defaultExtension: 'js',
            main: 'core.js'
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

        'ng2-translate': {
            format: 'cjs',
            defaultExtension: 'js',
            main: 'ng2-translate.js'
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
        'ng2-dragula': {
            format: 'cjs',
            defaultExtension: 'js',
            main: 'ng2-dragula.js'
        },
        'angular2-datatable': {
            format: 'cjs',
            defaultExtension: 'js',
            main: 'datatable.js'
        },
        'lodash': {
            format: 'cjs',
            defaultExtension: 'js',
            main: 'lodash.js'
        }
    },

    map: {
        'rxjs': 'node_modules/rxjs',
        '@angular': 'node_modules/@angular',
        '@angular2-material': 'node_modules/@angular2-material',
        'ng2-translate': 'node_modules/ng2-translate',
        'md2': 'node_modules/md2/src/components',
        'app': 'wwwroot/app',
        'ng2-dragula': 'node_modules/ng2-dragula',
        'angular2-datatable': 'node_modules/angular2-datatable',
        'lodash': 'node_modules/lodash'
    },
    paths: {
        'dragula': 'node_modules/dragula/dist/dragula.js'
    },
});

Promise.all([
  System.import('@angular/core/testing'),
  System.import('@angular/platform-browser-dynamic/testing')
]).then(function (providers) {
    let testing = providers[0];
    let testingBrowser = providers[1];

    testing.setBaseTestProviders(testingBrowser.TEST_BROWSER_DYNAMIC_PLATFORM_PROVIDERS,
      testingBrowser.TEST_BROWSER_DYNAMIC_APPLICATION_PROVIDERS);

}).then(function () {
    // Finally, load all spec files.
    // This will run the tests directly.
    return Promise.all(
      allSpecFiles.map(function (moduleName) {
          return System.import(moduleName);
      }));
}).then(__karma__.start, __karma__.error);

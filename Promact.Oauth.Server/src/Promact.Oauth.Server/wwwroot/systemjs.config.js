/**
 * System configuration for Angular 2 samples
 * Adjust as necessary for your application needs.
 */
(function (global) {
    // map tells the System loader where to look for things
    var map = {
        'app': 'app', // 'dist',
        '@angular': 'lib/@angular',
        'angular2-in-memory-web-api': 'lib/angular2-in-memory-web-api',
        '@angular2-material': 'lib/@angular2-material',
        'rxjs': 'lib/rxjs',
        'md2': 'lib/md2',
        'angular2-datatable': 'lib/angular2-datatable',
        'lodash': 'lib/lodash'
    };
    // packages tells the System loader how to load when no filename and/or no extension
    var packages = {
        'app': { main: 'main.js', defaultExtension: 'js' },
        'rxjs': { defaultExtension: 'js' },
        'angular2-in-memory-web-api': { main: 'index.js', defaultExtension: 'js' },

        '@angular2-material/core': {
            format: 'cjs',
            defaultExtension: 'js',
            main: 'core.umd.js'
        },

        '@angular2-material/button': {
            format: 'cjs',
            defaultExtension: 'js',
            main: 'button.umd.js'
        },

        '@angular2-material/toolbar': {
            format: 'cjs',
            defaultExtension: 'js',
            main: 'toolbar.umd.js'
        },

        '@angular2-material/sidenav': {
            format: 'cjs',
            defaultExtension: 'js',
            main: 'sidenav.umd.js'
        },

        '@angular2-material/input': {
            format: 'cjs',
            defaultExtension: 'js',
            main: 'input.umd.js'
        },

        '@angular2-material/card': {
            format: 'cjs',
            defaultExtension: 'js',
            main: 'card.umd.js'
        },
        '@angular2-material/checkbox': {
            format: 'cjs',
            defaultExtension: 'js',
            main: 'checkbox.umd.js'
        },
        '@angular2-material/progress-bar': {
            format: 'cjs',
            defaultExtension: 'js',
            main: 'progress-bar.umd.js'
        },

        '@angular2-material/progress-circle': {
            format: 'cjs',
            defaultExtension: 'js',
            main: 'progress-circle.umd.js'
        },
        'md2': {
            format: 'cjs',
            main: 'md2.umd.js',
            defaultExtension: 'js'
        },
        'md2/toast': {
            format: 'cjs',
            defaultExtension: 'js',
            main: 'toast.js'
        },
    };

    var ngPackageNames = [
      'common',
      'compiler',
      'core',
      'forms',
      'http',
      'platform-browser',
      'platform-browser-dynamic',
      'router',
      'upgrade'
    ];

    var mdPackages = [
      'button',
      'toolbar'

    ];

    // Individual files (~300 requests):
    function packIndex(pkgName) {
        packages['@angular/' + pkgName] = { main: 'index.js', defaultExtension: 'js' };
    }


    // Bundled (~40 requests):
    function packUmd(pkgName) {
        packages['@angular/' + pkgName] = { main: '/bundles/' + pkgName + '.umd.js', defaultExtension: 'js' };
    }
    // Most environments should use UMD; some (Karma) need the individual index files
    var setPackageConfig = System.packageWithIndex ? packIndex : packUmd;
    // Add package entries for angular packages

    ngPackageNames.forEach(setPackageConfig);

    var config = {
        map: map,
        packages: packages
    };

    System.config(config);
})(this);
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
        '@angular/material': 'lib/@angular/material/bundles',
        'rxjs': 'lib/rxjs',
        'md2': 'lib/md2/bundles',
        '@angular/animations': 'lib/@angular/animations/bundles/animations.umd.js',
        '@angular/platform-browser/animations': 'lib/@angular/platform-browser/bundles/platform-browser-animations.umd.js',
        '@angular/animations/browser': 'lib/@angular/animations/bundles/animations-browser.umd.js'
    };
    // packages tells the System loader how to load when no filename and/or no extension
    var packages = {
        'app': { main: 'main.js', defaultExtension: 'js' },
        'rxjs': { defaultExtension: 'js' },
        'angular2-in-memory-web-api': { main: 'index.js', defaultExtension: 'js' },
        '@angular/material': { main: 'material.umd.js', defaultExtension: 'js' },
        'md2': {
            format: 'cjs',
            main: 'md2.umd.js',
            defaultExtension: 'js'
        }
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
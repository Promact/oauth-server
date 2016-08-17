/**
 * System configuration for Angular 2 samples
 * Adjust as necessary for your application needs.
 */
(function (global) {
    // map tells the System loader where to look for things
    var map = {
        'app': 'app', // 'dist',
        '@angular': '/lib/@angular',
        'angular2-in-memory-web-api': '/lib/angular2-in-memory-web-api',
        'rxjs': '/lib/rxjs',
        'md2': '/lib/md2',
        '@angular2-material' : '/lib/@angular2-material'
       
    };
    // packages tells the System loader how to load when no filename and/or no extension
    var packages = {
        'app': { main: 'main.js', defaultExtension: 'js' },
        'rxjs': { defaultExtension: 'js' },
        'angular2-in-memory-web-api': { main: 'index.js', defaultExtension: 'js' },


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
        'md2/toast': {
            format: 'cjs',
            defaultExtension: 'js',
            main: 'toast.js'
        },
        'md2/multiselect': {
            format: 'cjs',
            defaultExtension: 'js',
            main: 'multiselect.js'
        },
        //...

        //--- or ---

        //'md2/all': {
        //    format: 'cjs',
        //    defaultExtension: 'js',
        //    main: 'all.js'
        //}
    

        'angular2-in-memory-web-api': { main: 'index.js', defaultExtension: 'js' }
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
      'router-deprecated',
      'upgrade',
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


    packages['@angular2-material/core'] = {
        format: 'cjs',
        defaultExtension: 'js',
        main: 'core.js'
    };
    
    packages['@angular2-material/button'] = {
        format: 'cjs',
        defaultExtension: 'js',
        main: 'button.js'
    };

    packages['@angular2-material/toolbar'] = {
        format: 'cjs',
        defaultExtension: 'js',
        main: 'toolbar.js'
    };

    packages['@angular2-material/sidenav'] = {
        format: 'cjs',
        defaultExtension: 'js',
        main: 'sidenav.js'
    }

    packages['@angular2-material/input'] = {
        format: 'cjs',
        defaultExtension: 'js',
        main: 'input.js'
    }

    var config = {
        map: map,
        packages: packages
    };

    //// add package entries for angular packages in the form '@angular/common': { main: 'index.js', defaultExtension: 'js' }
    //ngPackageNames.forEach(function (pkgName) {
    //    packages[pkgName] = { main: 'index.js', defaultExtension: 'js' };
    //});

    //var config = {
    //    map: map,
    //    packages: packages
    //};

    //// filterSystemConfig - index.html's chance to modify config before we register it.
    //if (global.filterSystemConfig) {
    //    global.filterSystemConfig(config);
    //}
    System.config(config);
})(this);

/// <binding BeforeBuild='copytowwwroot' />
"use strict";

var gulp = require("gulp"),
    rimraf = require("rimraf"),
    concat = require("gulp-concat"),
    cssmin = require("gulp-cssmin"),
    uglify = require("gulp-uglify"),
    Server = require('karma').Server;

var paths = {
    webroot: "./wwwroot/"
};


//var Server = require('karma').Server;

paths.js = paths.webroot + "js/**/*.js";
paths.minJs = paths.webroot + "js/**/*.min.js";
paths.css = paths.webroot + "css/**/*.css";
paths.minCss = paths.webroot + "css/**/*.min.css";
paths.concatJsDest = paths.webroot + "js/site.min.js";
paths.concatCssDest = paths.webroot + "css/site.min.css";

gulp.task("copytowwwroot", function () {
    gulp.src([
         'node_modules/zone.js/dist/zone.js',
         'node_modules/reflect-metadata/Reflect.js',
         'node_modules/systemjs/dist/system.src.js',
         'node_modules/core-js/client/shim.min.js'
    ]).pipe(gulp.dest('./wwwroot/lib/'));

    gulp.src([
      'node_modules/@angular/**/*.js'
    ]).pipe(gulp.dest('./wwwroot/lib/@angular'));

    gulp.src([
    'node_modules/@angular2-material/**/*.js'
    ]).pipe(gulp.dest('./wwwroot/lib/@angular2-material'));


    gulp.src([
      'node_modules/rxjs/**/*.js'
    ]).pipe(gulp.dest('./wwwroot/lib/rxjs'));

    gulp.src([
        'node_modules/md2/src/components/**/*.js',
        'node_modules/md2/src/components/**/*.js.map'
    ]).pipe(gulp.dest('./wwwroot/lib/md2'));

});

gulp.task("clean:js", function (cb) {
    rimraf(paths.concatJsDest, cb);
});

gulp.task("clean:css", function (cb) {
    rimraf(paths.concatCssDest, cb);
});

gulp.task("clean", ["clean:js", "clean:css"]);

gulp.task("min:js", function () {
    return gulp.src([paths.js, "!" + paths.minJs], { base: "." })
        .pipe(concat(paths.concatJsDest))
        .pipe(uglify())
        .pipe(gulp.dest("."));
});

gulp.task("min:css", function () {
    return gulp.src([paths.css, "!" + paths.minCss])
        .pipe(concat(paths.concatCssDest))
        .pipe(cssmin())
        .pipe(gulp.dest("."));
});

gulp.task("min", ["min:js", "min:css"]);

//// Run test once and exit
//gulp.task('test', function (done) {
//    new Server({
//        configFile: __dirname + '/karma.conf.js',
//        singleRun: true
//    }, done).start();
//});

//Runs karma test
gulp.task('test', function (done) {
    new Server({
        configFile: __dirname + '/karma.conf.js',
        singleRun: true,
    }, function () { done(); }).start();
});

//Generates coverage reports in coverage folder
gulp.task('coverage', function () {
    return gulp.src('coverage/coverage-final.json')
    .pipe(remapIstanbul({
        reports: {
            'html': 'coverage'
        }
    }))
    .pipe(gulp.dest('./coverage'));
});
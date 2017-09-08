var gulp = require('gulp'),
    watch = require('gulp-watch');

var src = [
    './Aubergine.StyledTextbox/App_Plugins',
    './Aubergine.Comments/App_Plugins',
    './Aubergine.Forums/App_Plugins',
    './Aubergine.Auth/App_Plugins',
    './Aubergine.Blog/App_Plugins',
    './Aubergine.Helpers/App_Plugins'],
    dest = './Aubergine.Web/App_Plugins';


var css_src = [
    './Aubergine.Blog/css',
    './Aubergine.Forums/css',
    './Aubergine.Comments/css'],
    css_dest = './Aubergine.Web/css';

gulp.task('monitor', function () {

    for (var i = 0; i < src.length; i++) {
        watch(src[i] + '/**/*', { ignoreInitial: false, verbose: true })
            .pipe(gulp.dest(dest));
    }

});

gulp.task('css', function () {

    for (var i = 0; i < css_src.length; i++) {
        watch(css_src[i] + '/**/*', { ignoreInitial: false, verbose: true })
            .pipe(gulp.dest(css_dest));
    }

});


gulp.task('default', ['monitor', 'css']);
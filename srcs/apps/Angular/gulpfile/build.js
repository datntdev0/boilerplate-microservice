import path from 'path';
import gulp from 'gulp';
import dartSass from 'sass';
import gulpSass from 'gulp-sass';
import concat from 'gulp-concat';

import config from '../gulpfile.config.js';
import cleanTask from './clean.js';

const sass = gulpSass(dartSass);

function outputPipe(pipe, dirname) {
    const bundleDest = config.options.dist.map(x => path.join(x, dirname));
    bundleDest.forEach(destPath => {
        pipe = pipe.pipe(gulp.dest(destPath)) 
    });
    return pipe;
}

export function buildAssets(cb) {
    const bundleDir = config.bundle.assets;
    
    let assetsPipe = gulp
        .src(config.source.assets, { encoding: false })

    assetsPipe = outputPipe(assetsPipe, bundleDir);

    return assetsPipe;
}

export function buildStyles(cb) {
    const bundleName = path.basename(config.bundle.styles);
    const bundleDir = path.dirname(config.bundle.styles);

    let bundlePipe = gulp
        .src(config.source.styles)
        .pipe(sass().on('error', sass.logError))
        .pipe(concat(bundleName));

    bundlePipe = outputPipe(bundlePipe, bundleDir);

    return bundlePipe;
}

export function buildScripts(cb) {
    const bundleName = path.basename(config.bundle.scripts);
    const bundleDir = path.dirname(config.bundle.scripts);

    let bundlePipe = gulp
        .src(config.source.scripts)
        .pipe(concat(bundleName));

    bundlePipe = outputPipe(bundlePipe, bundleDir);

    return bundlePipe;
}

export default gulp.series(cleanTask, 
    gulp.parallel(buildAssets, buildStyles, buildScripts));

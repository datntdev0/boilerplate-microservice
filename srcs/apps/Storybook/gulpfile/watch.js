import gulp from 'gulp';

import { buildStyles, buildScripts } from './build.js';

// Watch paths configuration
const WATCH_PATHS = {
    styles: "./src/**/*.scss",
    scripts: "./src/**/*.js"
};

/**
 * Watch task for SCSS files
 * Monitors SCSS/SASS files and recompiles on changes
 */
function watchStyles() {
    return gulp.watch(
        WATCH_PATHS.styles,
        { ignoreInitial: false },
        buildStyles
    );
}

/**
 * Watch task for asset files
 * Monitors static assets and copies them on changes
 */
function watchScripts() {
    return gulp.watch(
        WATCH_PATHS.scripts,
        { ignoreInitial: false },
        buildScripts
    );
}

/**
 * Main watch task
 * Runs all watch tasks in parallel
 */
export default function watchTask() {
    watchStyles();
    watchScripts();
    console.log('👀 Watching for file changes...');
}
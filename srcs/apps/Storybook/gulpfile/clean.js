import path from 'path';
import gulp from 'gulp';
import clean from 'gulp-clean';

import config from '../gulpfile.config.js';

function getDistPaths() {
    const paths = [];

    if (config.bundle.styles) {
        const bundleDir = path.dirname(config.bundle.styles);
        paths.push(...config.options.dist.map(x => path.join(x, bundleDir)));
    }

    return paths;
}

export default function cleanTask(cb) {
    const paths = getDistPaths();
    return gulp.src(paths, { read: false, allowEmpty: true })
        .pipe(clean({ force: true }));
}
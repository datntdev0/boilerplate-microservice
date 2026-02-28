import buildTask from "./gulpfile/build.js";
import cleanTask from "./gulpfile/clean.js";
import watchTask from "./gulpfile/watch.js";

export {
    buildTask as build,
    cleanTask as clean,
    watchTask as watch,
};
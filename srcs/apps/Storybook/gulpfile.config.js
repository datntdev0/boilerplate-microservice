const config = {
    options: {
        dist: ["./dist"],
    },
    source: {
        styles: ["./src/styles/index.scss"],
        scripts: ["./src/scripts/index.js"],
    }, 
    bundle: {
        styles: "css/style.css",
        scripts: "js/bundle.js",
    }
};

export default config;
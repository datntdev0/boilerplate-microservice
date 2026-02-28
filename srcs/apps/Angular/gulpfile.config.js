const config = {
    options: {
        dist: [
            "./dist/cdn/",
            "../Identity/wwwroot/",
        ],
    },
    source: {
        styles: ["./src/styles/index.scss"],
        scripts: ["./src/scripts/index.js"],
        assets: ["./public/**/*"],
    }, 
    bundle: {
        styles: "css/bundle.css",
        scripts: "js/bundle.js",
        assets: "",
    }
};

export default config;
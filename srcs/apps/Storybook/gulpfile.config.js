const config = {
    options: {
        dist: [
            "./src/dist",
            "../Angular/public/dist",
            "../Identity/wwwroot/dist",
        ],
    },
    source: {
        styles: ["./src/styles/index.scss"],
        scripts: ["./src/scripts/index.js"],
        assets: ["./src/assets/**/*"],
    }, 
    bundle: {
        styles: "css/bundle.css",
        scripts: "js/bundle.js",
        assets: "media",
    }
};

export default config;
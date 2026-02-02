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
    }, 
    bundle: {
        styles: "css/bundle.css",
        scripts: "js/bundle.js",
    }
};

export default config;
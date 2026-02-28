const config = {
    options: {
        dist: [
            "./dist/assets/",
            "../Identity/wwwroot/",
        ],
    },
    source: {
        styles: ["./src/styles/index.scss"],
        scripts: ["./src/scripts/index.js"],
        assets: ["./public/**/*"],
    }, 
    bundle: {
        styles: "styles/bundle.css",
        scripts: "scripts/bundle.js",
        assets: "",
    }
};

export default config;
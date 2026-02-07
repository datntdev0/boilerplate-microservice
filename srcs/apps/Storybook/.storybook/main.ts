import type { StorybookConfig } from '@storybook/web-components-vite';

const config: StorybookConfig = {
  "stories": [
    "../src/**/*.mdx",
    "../src/**/*.stories.@(js|jsx|mjs|ts|tsx)"
  ],
  "addons": ["@storybook/addon-docs", "@storybook/addon-themes"],
  "staticDirs": [{ from: "../src/dist/media", to: "/media" }],
  "framework": "@storybook/web-components-vite"
};
export default config;
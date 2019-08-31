const merge = require('webpack-merge');
const baseConfig = require('./webpack/webpack.base.js');
const serverConfig = require('./webpack/webpack.server.js');
const staticConfig = require('./webpack/webpack.static.js');
const devConfig = require('./webpack/webpack.dev.js');
const prodConfig = require('./webpack/webpack.prod.js');

module.exports = (env, argv) => {
    const isDev = argv.mode === 'development';
    const isAnalyze = env && env.anlz;
    const isDevServer = env && env.server;

    if (isDevServer) {
        return merge.smart(baseConfig(), serverConfig());
    }

    if (isDev) {
        return merge.smart(
            baseConfig(),
            staticConfig(isDev, isAnalyze),
            devConfig()
        );
    }

    return merge.smart(baseConfig(), staticConfig(isDev, isAnalyze), prodConfig());
};

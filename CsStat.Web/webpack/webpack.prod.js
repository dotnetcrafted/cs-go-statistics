const UglifyJsPlugin = require('uglifyjs-webpack-plugin');
const OptimizeCssAssetsPlugin = require('optimize-css-assets-webpack-plugin');
const utils = require('./utils');

// pass env variable to babel https://github.com/webpack/webpack/issues/2121
process.env.NODE_ENV = 'production';

module.exports = () => {
  utils.log('\nPRODUCTION BUILD\n');

  return {
    optimization: {
      minimizer: [
        new UglifyJsPlugin({
          uglifyOptions: {
            parse: {},
            compress: {
              comparisons: false
            },
            output: {
              comments: false,
              ascii_only: true
            }
          },
          parallel: true,
          cache: true,
          sourceMap: true
        })
      ]
    },
    plugins: [
      new OptimizeCssAssetsPlugin({
        cssProcessorPluginOptions: {
          preset: ['default', { discardComments: { removeAll: true } }],
        },
        canPrint: !utils.isJsonOutput()
      })
    ]
  };
};

const MiniCssExtractPlugin = require('mini-css-extract-plugin');
const ManifestPlugin = require('webpack-manifest-plugin');
const BundleAnalyzerPlugin = require('webpack-bundle-analyzer')
    .BundleAnalyzerPlugin;

module.exports = (isDev, isAnlz) => ({
    output: {
        filename: '[name].[chunkhash].js'
    },
    module: {
        rules: [
            {
                test: /\.css$/,
                use: [
                    MiniCssExtractPlugin.loader,
                    {
                        loader: 'css-loader',
                        options: {
                            sourceMap: true,
                            importLoaders: 1
                        }
                    },
                    {
                        loader: 'postcss-loader',
                        options: {
                            sourceMap: true
                        }
                    }
                ]
            },
            {
                test: /\.scss$/,
                use: [
                    MiniCssExtractPlugin.loader,
                    {
                        loader: 'css-loader',
                        options: {
                            sourceMap: true,
                            importLoaders: 2
                        }
                    },
                    {
                        loader: 'postcss-loader',
                        options: {
                            sourceMap: true
                        }
                    },
                    {
                        loader: 'sass-loader',
                        options: {
                            sourceMap: true
                        }
                    },
                    // Reads Sass vars from files or inlined in the options property
                    {
                        loader: '@epegzz/sass-vars-loader',
                        options: {
                            syntax: 'scss',
                            vars: {
                                viewports: require('./config').viewports,
                                containerMaxWidth: require('./config')
                                    .containerMaxWidth,
                                defaultTransitionDuration: require('./config')
                                    .defaultTransitionDuration
                            }
                        }
                    }
                ]
            }
        ]
    },
    plugins: [
        new ManifestPlugin({
            filter: ({ name }) => !name.endsWith('.map'),
            // https://github.com/danethurber/webpack-manifest-plugin/issues/144#issuecomment-382779618
            seed: {}
        }),
        new MiniCssExtractPlugin({
            filename: '[name].[contenthash].css'
        })
    ].concat(
        isAnlz
            ? new BundleAnalyzerPlugin({
                  analyzerPort: 8889
              })
            : []
    )
});

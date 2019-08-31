const webpack = require('webpack');
const utils = require('./utils');

module.exports = () => {
    utils.log('\nDEV SERVER BUILD\n');

    return {
        devServer: {
            headers: {
                'Access-Control-Allow-Origin': '*'
            },
            hot: true,
            stats: {
                children: false
            }
        },
        module: {
            rules: [
                {
                    test: /\.s?css$/,
                    use: [
                        'style-loader',
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
                        }
                    ]
                }
            ]
        },
        plugins: [new webpack.HotModuleReplacementPlugin()]
    };
};

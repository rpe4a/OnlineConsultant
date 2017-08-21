"use strict";

const path = require('path');
const webpack = require('webpack');
const ExtractTextPlugin = require("extract-text-webpack-plugin");
const CleanWebpackPlugin = require('clean-webpack-plugin');

const extractCSS = new ExtractTextPlugin({
    filename: "bundle.css"
});

module.exports = {
    entry: {
        bundle: ["babel-polyfill", "./src/file.js"],
        chat: ["babel-polyfill", "./src/chat/"],
        vendor: ['jquery', 'bootstrap-sass']
    },
    output: {
        path: path.join(__dirname, 'dist'),
        filename: '[name].js',
        publicPath: '/dist/'
    },

    resolve: {
        modules: ['node_modules', 'src', 'bower_components'],
        extensions: ['.js', '.scss'],
        alias: {
            'bootstrap-css$': "bootstrap/dist/css/bootstrap",
            'bootstrap-js$': "bootstrap/dist/js/bootstrap",
            'font-awesome$': 'font-awesome/sccs/font-awesome',
        }
    },
    plugins: [
        extractCSS,
        new webpack.DefinePlugin({
            'process.env.NODE_ENV': JSON.stringify(process.env.NODE_ENV)
        }),
        new CleanWebpackPlugin(['dist']),
        new webpack.optimize.CommonsChunkPlugin({
            names: ['vendor']
        }),
        new webpack.ProvidePlugin({
            '$': "jquery",
            "jQuery": "jquery",
            'window.jQuery': "jquery",
        }),
    ],
    devtool: '#eval-source-map',
    module: {
        rules: [{
                test: /\.scss$/,
                use: extractCSS.extract({
                    use: [{
                            loader: "css-loader",
                            options: {
                                importLoaders: 3,
                                sourceMap: true,
                                //minimize: true
                            },
                        },
                        {
                            loader: "group-css-media-queries-loader"
                        },
                        {
                            loader: 'postcss-loader',
                            options: {
                                sourceMap: true,
                                plugins: function () {
                                    return [
                                        require('autoprefixer')({
                                            browsers: ['last 2 versions'],
                                            cascade: false,
                                        })
                                    ]
                                }
                            }
                        },
                        {
                            loader: "sass-loader"
                        }
                    ],
                    // use style-loader in development 
                    fallback: "style-loader"
                })
            },

            {
                test: /\.(png|jpg|gif)$/,
                loader: 'file-loader',
                options: {
                    name: '[name].[ext]' //?[hash]' 
                }
            },
            {
                test: /\.woff(\?v=\d+\.\d+\.\d+)?$|\.woff2(\?v=\d+\.\d+\.\d+)?$|\.ttf(\?v=\d+\.\d+\.\d+)?$|\.otf(\?v=\d+\.\d+\.\d+)?$|\.eot(\?v=\d+\.\d+\.\d+)?$|\.svg(\?v=\d+\.\d+\.\d+)?$/,
                loader: 'file-loader',
                options: {
                    name: '[name].[ext]' //?[hash]' 
                }
            },
            {
                test: /\.js$/,
                exclude: /(node_modules)/,
                use: {
                    loader: 'babel-loader',
                    options: {
                        presets: ['env'],
                    }
                }
            },
            {
                enforce: 'pre',
                test: /.vue$|.js$/,
                loader: 'eslint-loader',
                exclude: /node_modules/
            }
        ],
    }

};

if (process.env.NODE_ENV === 'production') {
    module.exports.devtool = '#source-map'
    /*  module.exports.output.publicPath = '',*/
  
    module.exports.plugins = (module.exports.plugins || []).concat([
        new webpack.DefinePlugin({
            'process.env': {
                NODE_ENV: '"production"'
            }
        }),
        new webpack.optimize.UglifyJsPlugin({
            sourceMap: true,
            compress: {
                warnings: false
            }
        }),
        new webpack.LoaderOptionsPlugin({
            minimize: true
        }),
    ])
}
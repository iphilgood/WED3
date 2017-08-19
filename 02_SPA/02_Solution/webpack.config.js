const HtmlWebpackPlugin = require('html-webpack-plugin');
const webpack = require('webpack');

const rootDir = __dirname;
const srcDir = rootDir + "/src";
const distDir = rootDir + "/public";
const scriptsDir = "/scripts";

module.exports = {
  context: rootDir, // a base directory to resolve the "entry"
  entry: {
    main: [
      `${srcDir}${scriptsDir}/main.ts`,
      `${srcDir}${scriptsDir}/bl.ts`,
      `${srcDir}${scriptsDir}/dl.ts`,
    ]
  },
  resolve: {
    extensions: ['.ts', '.js']
  },
  output: {
    path: distDir + scriptsDir,
    filename: "[name].js" // [name] means we are going to use the "key" value of each entry as the bundle file name
  },
  devtool: "source-map", // Enable sourcemaps for debugging webpack's output.
  module: {
    rules: [
      { // all files with a '.ts' extension will be handled by 'awesome-typescript-loader'.
        test: /\.ts$/,
        exclude: /(node_modules)/,
        loader: "awesome-typescript-loader",
        options: {
          configFileName: rootDir + "/tsconfig.json"
        }
      }, { // all output '.js' files will have any sourcemaps re-processed by 'source-map-loader'.
        test: /\.js$/,
        enforce: "pre",
        loader: "source-map-loader"
      }
    ]
  },
  plugins: [

    new HtmlWebpackPlugin({
      title: 'Index',
      filename: '../index.html', // relative path from "output" directory
      template: srcDir + '/index.html' // source file
    })
    /*new webpack.optimize.UglifyJsPlugin({
        compress: { warnings: false }
    })*/
  ]
};

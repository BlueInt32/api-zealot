/* eslint-env node*/
const path = require('path');
const webpack = require('webpack');
const HtmlWebpackHarddiskPlugin = require('html-webpack-harddisk-plugin');

// this is server path, e.g. http://localhost/boilerplate-vuejs
// warning : this path must also be set in
const serverAppName = '/boilerplate-vuejs/'; // start and end slashes are important !
let apiHost = '';
let webpackOutputFolder = '';
let webpackPublicPath = '';
const HtmlWebpackPlugin = require('html-webpack-plugin');

const setupConfigs = function () {
  switch (process.env.NODE_ENV) {
    case 'production':
      apiHost = '"http://localhost/path-to-api"';
      webpackOutputFolder = path.resolve(__dirname, '../dist');
      webpackPublicPath = serverAppName;
      break;
    default:
      apiHost = '"https://localhost:5001/api"';
      webpackOutputFolder = path.resolve(__dirname, '../dist');
      console.log('Webpack output folder', webpackOutputFolder);
      webpackPublicPath = '/';
  }
};

setupConfigs();

const config = {
  mode: 'development',
  entry: {
    app: path.resolve(__dirname, '../src/client-entry.js')
  },
  devtool: 'source-map',
  module: {
    rules: [
      {
        enforce: 'pre',
        test: /(\.js$)|(\.vue$)/u,
        loader: 'eslint-loader',
        exclude: /node_modules/u
      },
      {
         test: /\.css$/u,
         use: [
           'style-loader', 'css-loader'
         ]
      },
      {
        test: /\.vue$/u,
        loader: 'vue-loader',
        options: {
          loaders: {
            scss: 'vue-style-loader!css-loader!sass-loader', // <style lang="scss">
            sass: 'vue-style-loader!css-loader!sass-loader?indentedSyntax' // <style lang="sass">
          }
        }
      },
      {
        test: /\.js$/u,
        loader: 'babel-loader',
        exclude: /node_modules/u
      },
      {
        test: /\.(png|jpg|gif|config)$/u,
        use: [
          {
            loader: 'file-loader'
          }
        ]
      },
      {
        test: /favicon\.png$/u,
        loader: 'file-loader?name=static/[name].[ext]' // <-- retain original file name
      }
    ]
  },
  output: {
    path: path.resolve(__dirname, webpackOutputFolder),
    publicPath: webpackPublicPath,
    filename: 'assets/js/[name].js'
  },
  plugins: [
    new HtmlWebpackPlugin({
      title: 'boilerPlate',
      template: 'index.prod.html',
      serverAppName,
      alwaysWriteToDisk: true
    }),
    new HtmlWebpackHarddiskPlugin(),
    new webpack.DefinePlugin({
      __API__: apiHost,
      __SERVER_APP_NAME__: serverAppName,
      'process.env': {
        NODE_ENV: `"${process.env.NODE_ENV}"`
      }
    })
  ]
};

module.exports = config;

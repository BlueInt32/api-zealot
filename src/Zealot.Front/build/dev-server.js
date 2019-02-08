/* eslint-env node*/
const webpack = require('webpack');
const webpackDevMiddleWare = require('webpack-dev-middleware');
const webpackHotMiddleWare = require('webpack-hot-middleware');
const clientConfig = require('./webpack.client.config');

module.exports = function setupDevServer(app) {
  clientConfig.entry.app = [
    'webpack-hot-middleware/client', clientConfig.entry.app
  ];
  clientConfig.plugins.push(
    new webpack.HotModuleReplacementPlugin(),
    new webpack.NoEmitOnErrorsPlugin()
  );
  const clientCompiler = webpack(clientConfig);
  app.use(webpackDevMiddleWare(clientCompiler, {
      stats: {
        colors: true
      }
    }));
  app.use(webpackHotMiddleWare(clientCompiler));
};

process.env.VUE_APP_VERSION = require('./package.json').version;
const { basename } = require('path');

// look at https://cli.vuejs.org/config/#global-cli-config for how to customize this

module.exports = {
  configureWebpack: {
    devtool: 'source-map'
  },

  outputDir: '../deploy/wwwroot',
  productionSourceMap: false,
}


/* eslint-env node*/
const base = require('./webpack.base.config');

const config = { ...base, plugins: base.plugins || [] };

config.module.rules
  .filter(rule => rule.loader === 'vue-loader')
  .forEach(rule => {
    rule.options.extractCSS = false;
  });

config.plugins.push();

module.exports = config;

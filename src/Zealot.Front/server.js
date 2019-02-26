/* eslint-env node*/
const express = require('express');

const app = express();
const fs = require('fs');
const path = require('path');

// const acceptedVideoRegex = /^.*\.((mp4)|(m4v)|(avi)|(mkv))$/i

console.log('Serving from ', __dirname);

app.use('/dist', express.static(path.resolve(__dirname, './dist')));

require('./build/dev-server')(app);

app.get('*', (req, res) => {
  console.log('catch *', req.originalUrl);
  fs.readFile(path.resolve(__dirname, 'dist\\index.html'), (err, data) => {
    if (err) {
      throw err;
    }
    console.log(data);
    res.send(data.toString());
  });
  // res.write(stuff.resolve());
  // res.end();
});
const defaultPort = 3000;
const port = process.env.PORT || defaultPort;
app.listen(port, () => {
  console.log(`server started at http://localhost:${port}`);
});

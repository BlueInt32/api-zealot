import axios from 'axios';

axios.defaults.baseURL = 'https://api.fullstackweekly.com';
const serviceRootUrl = __API__; // eslint-disable-line no-undef

axios.interceptors.request.use(function (config) {
  if (typeof window === 'undefined') {
    return config;
  }
  const token = window.localStorage.getItem('token');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

const appService = {
  getBlogInfo() {
    return new Promise(resolve => {
      axios.get(`${serviceRootUrl}/posts/info`).then(
        response => {
          resolve(response.data);
        },
        () => {
          resolve('DAYUM !');
        }
      );
    });
  },
  sendWiz() {
    return new Promise(resolve => {
      axios.post(`${serviceRootUrl}/proxy`, {
        verb: 'POST',
        endpoint: 'http://there',
        body: '{content}'
      }).then(
        response => {
          resolve(response.data);
        },
        () => {
          resolve('DAYUM !');
        }
      );
    });
  }
};

export default appService;

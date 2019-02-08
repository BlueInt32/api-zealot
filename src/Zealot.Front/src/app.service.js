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
  sendWiz({ verb, endpointUrl, body }) {
    return new Promise(resolve => {
      axios.post(`${serviceRootUrl}/proxy`, {
        verb,
        endpointUrl,
        body
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

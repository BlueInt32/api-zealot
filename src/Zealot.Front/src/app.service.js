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
  getProjectsList() {
    return new Promise(resolve => {
      axios.get(`${serviceRootUrl}/projects`).then(
        response => {
          resolve(response.data);
        },
        () => {
          resolve('DAYUM !');
        }
      );
    });
  },
  sendWiz({ httpMethod, endpointUrl, body }) {
    return new Promise(resolve => {
      axios.post(`${serviceRootUrl}/proxy`, {
        httpMethod,
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
  },
  saveProject({ projectName, projectPath }) {
    return new Promise(resolve => {
      axios.post(`${serviceRootUrl}/projects`, {
        name: projectName,
        path: projectPath
      })
      .then(response => {
        resolve(response.data);
      },
      () => {

      });
    });
  },
  getProjectDetails({ projectId }) {
    return new Promise(resolve => {
      axios.get(`${serviceRootUrl}/projects/${projectId}`)
      .then(response => {
        resolve(response.data);
      },
      () => {

      });
    });
  }
};

export default appService;

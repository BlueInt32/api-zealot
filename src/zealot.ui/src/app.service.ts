import axios, { AxiosRequestConfig } from 'axios';
import { log } from './helpers/consoleHelpers';
import { prepareTreeBeforeSave } from './helpers/treeHelper';
import ProjectConfig from './domain/Project';

export default class AppService {
  private serviceRootUrl: string;
  constructor() {
    axios.defaults.baseURL = '/';
    axios.interceptors.request.use(this.axiosInterceptor);
    this.serviceRootUrl = process.env.VUE_APP_APIURL;
  }
  public axiosInterceptor = (config: AxiosRequestConfig) => {
    if (typeof window === 'undefined') {
      return config;
    }
    const token = window.localStorage.getItem('token');
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  };

  public getProjectsConfigsList(): Promise<ProjectConfig[]> {
    return new Promise((resolve, reject) => {
      axios
        .get(`${this.serviceRootUrl}/projects`)
        .then(response => {
          resolve(response.data);
        })
        .catch(error => {
          reject('DAYUM !');
        });
    });
  }
  // sendRequest({ httpMethod, endpointUrl, body }) {
  //   return new Promise(resolve => {
  //     axios
  //       .post(`${serviceRootUrl}/proxy`, {
  //         httpMethod,
  //         endpointUrl,
  //         body
  //       })
  //       .then(
  //         response => {
  //           resolve(response.data);
  //         },
  //         () => {
  //           resolve('DAYUM !');
  //         }
  //       );
  //   });
  // }
  // createProject({ projectName, projectPath }) {
  //   return new Promise(resolve => {
  //     axios
  //       .post(`${serviceRootUrl}/projects`, {
  //         name: projectName,
  //         path: projectPath
  //       })
  //       .then(
  //         response => {
  //           resolve(response.data);
  //         },
  //         () => {}
  //       );
  //   });
  // }
  // updateProject({ projectId, projectName, tree }) {
  //   return new Promise(resolve => {
  //     prepareTreeBeforeSave(tree);
  //     axios
  //       .put(`${serviceRootUrl}/projects/${projectId}`, {
  //         name: projectName,
  //         tree
  //       })
  //       .then(
  //         response => {
  //           resolve(response.data);
  //         },
  //         error => {
  //           log(error);
  //         }
  //       );
  //   });
  // }
  // getProjectDetails({ projectId }) {
  //   return new Promise(resolve => {
  //     axios.get(`${serviceRootUrl}/projects/${projectId}`).then(
  //       response => {
  //         resolve(response.data);
  //       },
  //       () => {}
  //     );
  //   });
  // }
}

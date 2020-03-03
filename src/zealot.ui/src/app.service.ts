import axios, { AxiosRequestConfig } from 'axios';
import { log } from './helpers/consoleHelpers';
import { prepareTreeBeforeSave } from './helpers/treeHelper';
import ProjectConfig from './domain/ProjectConfig';
import Guid from './helpers/Guid';
import Project from './domain/Project';
import CreateProjectParams from './domain/actionParams/CreateProjectParams';

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
          reject(error);
        });
    });
  }
  public getProjectDetails(projectId: Guid): Promise<Project> {
    return new Promise((resolve, reject) => {
      axios
        .get(`${this.serviceRootUrl}/projects/${projectId}`)
        .then(response => {
          resolve(response.data);
        })
        .catch(error => {
          reject(error);
        });
    });
  }
  createProject(params: CreateProjectParams) {
    return new Promise((resolve, reject) => {
      axios
        .post(`${this.serviceRootUrl}/projects`, {
          name: params.name,
          path: params.path
        })
        .then(response => {
          resolve(response.data);
        })
        .catch(error => {
          reject(error);
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
}

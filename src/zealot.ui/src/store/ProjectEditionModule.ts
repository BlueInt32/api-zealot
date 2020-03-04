import store from '.';
import {
  VuexModule,
  Module,
  Action,
  getModule,
  Mutation
} from 'vuex-module-decorators';
import AppService from '@/app.service';
import {
  logAction,
  log,
  logArrow,
  logMutation
} from '../helpers/consoleHelpers';
import Project from '@/domain/Project';
import Guid from '@/helpers/Guid';
import TreeModule from './TreeModule';
import CreateProjectParams from '@/domain/stateChangeParams/CreateProjectParams';

const appService = new AppService();

@Module({
  dynamic: true,
  namespaced: true,
  name: 'ProjectEditionModule',
  store
})
export default class ProjectEditionModule extends VuexModule {
  public currentProject: Project | null = null;

  @Action({ rawError: true })
  public async getProjectDetails(projectId: Guid) {
    logAction('Load Project', projectId);
    const projectDetails = await appService.getProjectDetails(projectId);
    logArrow('Received project details', projectDetails);
    const treeModule = getModule(TreeModule);
    treeModule.setRawTree(projectDetails.tree);
    this.context.commit('setProject', projectDetails);
  }

  @Action({ rawError: true })
  public async createProject(params: CreateProjectParams) {
    logAction('Create Project', params);
    const creationResult = await appService.createProject(params);
    logArrow('Created', creationResult);
  }

  // @Action({ rawError: true })
  // updateProject() {
  //   logAction('updateProject');
  //   const { id, name } = this.context.state;
  //   const { tree } = this.context.rootState.treeModule;
  //   appService
  //     .updateProject({
  //       projectId: id,
  //       projectName: name,
  //       tree
  //     })
  //     .then(data => {
  //       log(data);
  //     });
  // }

  @Mutation
  public setProject(project: Project) {
    logMutation('Set project', project);
    this.currentProject = project;
  }
}

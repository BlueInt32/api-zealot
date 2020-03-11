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
import UpdateProjectParams from '@/domain/stateChangeParams/UpdateProjectParam';

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

  @Action({ rawError: true })
  async updateProject() {
    logAction('updateProject');
    const treeModule = getModule(TreeModule);

    if (!this.currentProject) {
      return;
    }

    const updateParams = new UpdateProjectParams();
    updateParams.id = this.currentProject.id;
    updateParams.name = this.currentProject.name;
    updateParams.tree = this.currentProject.tree;

    const result = await appService.updateProject(updateParams)
    log(result);
  }

  @Mutation
  public setProject(project: Project) {
    logMutation('Set project', project);
    this.currentProject = project;
  }
}

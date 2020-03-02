import store from '.';
import {
  VuexModule,
  Module,
  Action,
  getModule,
  Mutation
} from 'vuex-module-decorators';
import AppService from '@/app.service';
import { logAction } from '../helpers/consoleHelpers';
import ProjectConfig from '@/domain/Project';

const appService = new AppService();

@Module({ dynamic: true, namespaced: true, name: 'projectsListModule', store })
export default class ProjectsListModule extends VuexModule {
  public projectsConfigs: ProjectConfig[] = [];

  @Action({ rawError: true })
  public async getProjectsConfigsList() {
    logAction('load projects');
    let projectsConfigs = await appService.getProjectsConfigsList();
    this.context.commit('setProjectsConfigs', projectsConfigs);
    return projectsConfigs;
  }

  @Mutation
  setProjectsConfigs(projectsConfigs: ProjectConfig[]) {
    this.projectsConfigs = projectsConfigs;
  }
}

import store from '.';
import {
  VuexModule,
  Module,
  Action,
  getModule,
  Mutation
} from 'vuex-module-decorators';
import AppService from '@/app.service';
import { logAction, logArrow } from '../helpers/consoleHelpers';
import ProjectConfig from '@/domain/ProjectConfig';

const appService = new AppService();

@Module({ dynamic: true, namespaced: true, name: 'ProjectsListModule', store })
export default class ProjectsListModule extends VuexModule {
  public projectsConfigs: ProjectConfig[] = [];

  @Action({ rawError: true })
  public async getProjectsConfigsList() {
    logAction('Load Projects');
    let projectsConfigs = await appService.getProjectsConfigsList();
    this.context.commit('setProjectsConfigs', projectsConfigs);
    logArrow('Projects', projectsConfigs);
    return projectsConfigs;
  }

  @Mutation
  setProjectsConfigs(projectsConfigs: ProjectConfig[]) {
    this.projectsConfigs = projectsConfigs;
  }
}

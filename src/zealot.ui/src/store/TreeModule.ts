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
import Project from '@/domain/Project';
import Guid from '@/helpers/Guid';

const appService = new AppService();

@Module({
  dynamic: true,
  namespaced: true,
  name: 'TreeModule',
  store
})
export default class TreeModule extends VuexModule {
  @Action({ rawError: true })
  public setRawTree(tree: Node) {
    // TODO couldn't this be a Mutation instead ?
  }
}

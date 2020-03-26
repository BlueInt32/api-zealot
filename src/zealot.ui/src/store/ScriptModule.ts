import store from '.';
import {
  VuexModule,
  Module,
  Action,
  getModule,
  Mutation
} from 'vuex-module-decorators';
import AppService from '@/app.service';
import { logAction, logMutation } from '../helpers/consoleHelpers';
import { RequestNode, ScriptNode } from '@/domain/Node';
import * as treeHelper from '@/helpers/treeHelper';
import * as packContextService from '@/domain/packContextService';
import { HttpMethodEnum } from '@/domain/HttpMethodEnum';
import { ZealotError } from '@/domain/Errors/ZealotError';
import TreeModule from './TreeModule';

const appService = new AppService();

@Module({
  dynamic: true,
  namespaced: true,
  name: 'ScriptModule',
  store
})
export default class ScriptModule extends VuexModule {
  public currentNode: ScriptNode | null = null;
  public requestResult: string = '';

  @Action({ rawError: true })
  public async executeScript() {}

  @Mutation
  public setCurrentNode(scriptNode: ScriptNode) {
    this.currentNode = scriptNode;
  }

  @Mutation
  public setCode(code: string) {
    logMutation('setCode', code);
    if (!this.currentNode) {
      throw new ZealotError('[RequestModule] CURRENT_NODE_NOT_SET');
    }
    this.currentNode.code = code;
    const treeModule = getModule(TreeModule);
    treeModule.setNodeInTree(this.currentNode);
  }
}

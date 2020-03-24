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
import Project from '@/domain/Project';
import Guid from '@/helpers/Guid';
import { Node, RequestNode } from '@/domain/Node';
import { NodeType } from '@/domain/NodeType';
import * as treeHelper from '@/helpers/treeHelper';
import * as packContextService from '@/domain/packContextService';
import { HttpMethodEnum } from '@/domain/HttpMethodEnum';
import { ZealotError } from '@/domain/Errors/ZealotError';
import TreeModule from './TreeModule';

const appService = new AppService();

@Module({
  dynamic: true,
  namespaced: true,
  name: 'RequestModule',
  store
})
export default class RequestModule extends VuexModule {
  public currentNode: RequestNode | null = null;
  public requestResult: string = '';

  @Action({ rawError: true })
  public async sendRequest() {
    // TODO maybe could be a Mutation
    if (!this.currentNode) {
      throw new ZealotError('[RequestModule] CURRENT_NODE_NOT_SET');
    }
    logAction(
      'Send Request',
      this.currentNode.httpMethod,
      this.currentNode.endpointUrl
    );
    const resultData = await appService.sendRequest({
      httpMethod: this.currentNode.httpMethod,
      endpointUrl: this.currentNode.endpointUrl,
      body: ''
    });
    this.context.commit('setRequestResult', { resultData });
    packContextService.setPackContext('a', { lastResult: resultData });
  }

  @Mutation
  public setcurrentNode(requestNode: RequestNode) {
    this.currentNode = requestNode;
  }

  @Mutation
  public setHttpMethod(httpMethod: HttpMethodEnum) {
    logMutation('setHttpMethod', httpMethod);
    if (!this.currentNode) {
      throw new ZealotError('[RequestModule] CURRENT_NODE_NOT_SET');
    }
    this.currentNode.httpMethod = httpMethod;
    const treeModule = getModule(TreeModule);
    treeModule.setNodeInTree(this.currentNode);
  }

  @Mutation
  public setRequestResult(data: any) {
    this.requestResult = data;
  }

  @Mutation
  public setEndpointUrl(endpointUrl: string) {
    logMutation('requestModule/endpointUrl', endpointUrl);
    if (!this.currentNode) {
      throw new ZealotError('[RequestModule] CURRENT_NODE_NOT_SET');
    }
    this.currentNode.endpointUrl = endpointUrl;
    const treeModule = getModule(TreeModule);
    treeModule.setNodeInTree(this.currentNode);
  }
}

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

const appService = new AppService();

@Module({
  dynamic: true,
  namespaced: true,
  name: 'RequestModule',
  store
})
export default class RequestModule extends VuexModule {
  public endpointUrl: string = '';
  public httpMethod: HttpMethodEnum = HttpMethodEnum.GET;
  public requestResult: string = '';

  @Action({ rawError: true })
  public async sendRequest() {
    // TODO maybe could be a Mutation
    logAction('Send Request', this.httpMethod, this.endpointUrl);
    const resultData = await appService.sendRequest({
      httpMethod: this.httpMethod,
      endpointUrl: this.endpointUrl,
      body: ''
    });
    this.context.commit('setRequestResult', { resultData });
    packContextService.setPackContext('a', { lastResult: resultData });
  }

  @Action({ rawError: true })
  public changeRequestUrl(endpointUrl: string) {
    // TODO maybe could be a Mutation
    logAction('[requestModule/setEndpointUrl]', endpointUrl);
    this.context.commit('setEndpointUrl', endpointUrl);
    this.context.commit(
      'treeModule/setNodeProperties',
      { endpointUrl, isPristine: false },
      { root: true }
    );
  }

  @Action({ rawError: true })
  public changeHttpMethod(httpMethod: string) {
    // TODO maybe could be a Mutation
    logAction('[requestModule/setHttpMethod]', httpMethod);
    this.context.commit('setHttpMethod', httpMethod);
    this.context.commit(
      'treeModule/setNodeProperties',
      { httpMethod, isPristine: false },
      { root: true }
    );
  }

  @Mutation
  public setHttpMethod(httpMethod: HttpMethodEnum) {
    logMutation('setHttpMethod', httpMethod);
    this.httpMethod = httpMethod;
  }

  @Mutation
  public setRequestResult(data: any) {
    this.requestResult = data;
  }

  @Mutation
  public setEndpointUrl(endpointUrl: string) {
    logMutation('requestModule/endpointUrl', endpointUrl);
    this.endpointUrl = endpointUrl;
  }
}

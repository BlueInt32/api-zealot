import appService from '../app.service';
import { logAction, logMutation } from '../helpers/consoleHelpers';
import { setPackContext } from '../services/packContextService';

const state = {
  requestUrl: '',
  httpMethod: '',
  requestResult: ''
};

const getters = {
  httpMethod: currentState => currentState.httpMethod,
  requestResult: currentState => currentState.requestResult,
  requestUrl: currentState => currentState.requestUrl
};

const actions = {
  resetFromTree(context, { requestUrl, httpMethod }) {
    context.commit('setRequestUrl', requestUrl);
    context.commit('setHttpMethod', httpMethod);
  },
  sendRequest(context, { httpMethod, url }) {
    logAction('sendRequest', httpMethod, url);
    appService.sendRequest({
      httpMethod: context.state.selectedHttpMethod,
      endpointUrl: context.state.requestUrl,
      body: '{}'
    }).then((data) => {
      context.commit('setRequestResult', { data });
      setPackContext('a', { lastResult: data });
    });
  },
  setRequestUrl(context, requestUrl) {
    logAction('[requestModule/setRequestUrl]', requestUrl);
    context.commit('setRequestUrl', requestUrl);
    context.commit('treeModule/setNodeProperties', { requestUrl, isPristine: false }, { root: true });
  },
  setHttpMethod(context, httpMethod) {
    logAction('[requestModule/setHttpMethod]', httpMethod);
    context.commit('setHttpMethod', httpMethod);
    context.commit('treeModule/setNodeProperties', { httpMethod, isPristine: false }, { root: true });
  }
};

const mutations = {
  setHttpMethod(currentState, httpMethod) {
    logMutation('setHttpMethod', httpMethod);
    currentState.httpMethod = httpMethod;
  },
  setRequestResult(currentState, { data }) {
    currentState.requestResult = data;
  },
  setRequestUrl(currentState, requestUrl) {
    logMutation('requestModule/requestUrl', requestUrl);
    currentState.requestUrl = requestUrl;
  }
};

export default {
  namespaced: true,
  state,
  getters,
  actions,
  mutations
};

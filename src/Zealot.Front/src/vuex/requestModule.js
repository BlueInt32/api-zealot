import appService from '../app.service';
import { logAction, logMutation } from '../helpers/consoleHelpers';
import { setPackContext } from '../services/packContextService';

const state = {
  selectedHttpMethod: 'GET',
  requestUrl: '',
  requestResult: ''
};

const getters = {
  selectedHttpMethod: currentState => currentState.selectedHttpMethod,
  requestResult: currentState => currentState.requestResult,
  requestUrl: currentState => currentState.requestUrl
};

const actions = {
  sendRequest(context) {
    logAction('sendRequest', context.state.selectedHttpMethod, context.state.requestUrl);
    appService.sendRequest({
      httpMethod: context.state.selectedHttpMethod,
      endpointUrl: context.state.requestUrl,
      body: '{}'
    }).then((data) => {
      context.commit('setRequestResult', { data });
      setPackContext('a', { lastResult: data });
    });
  },
  setHttpMethod(context, { httpMethod }) {
    logAction('setHttpMethod', httpMethod);
    context.commit('setHttpMethod', { httpMethod });
  },
  setRequestUrl(context, { requestUrl }) {
    logAction('setRequestUrl', requestUrl);
    context.commit('setRequestUrl', { requestUrl });
  }
};

const mutations = {
  setHttpMethod(currentState, { httpMethod }) {
    logMutation('selectedHttpMethod', httpMethod);
    currentState.selectedHttpMethod = httpMethod;
  },
  setRequestResult(currentState, { data }) {
    currentState.requestResult = data;
  },
  setRequestUrl(currentState, { requestUrl }) {
    logMutation('requestUrl', requestUrl);
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

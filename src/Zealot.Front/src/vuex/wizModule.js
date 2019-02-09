import appService from '../app.service';
import { logAction, logMutation } from '../helpers/consoleHelpers';

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
  sendWiz(context) {
    logAction('sendWiz', context.state.selectedHttpMethod, context.state.requestUrl);
    appService.sendWiz({
      httpMethod: context.state.selectedHttpMethod,
      endpointUrl: context.state.requestUrl,
      body: '{}'
    }).then((data) => {
      context.commit('setRequestResult', { data });
    });
  },
  selectHttpMethod(context, { httpMethod }) {
    logAction('selectHttpMethod', httpMethod);
    context.commit('selectHttpMethod', { httpMethod });
  },
  setRequestUrl(context, { requestUrl }) {
    logAction('setRequestUrl', requestUrl);
    context.commit('setRequestUrl', { requestUrl });
  }
};

const mutations = {
  selectHttpMethod(currentState, { httpMethod }) {
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

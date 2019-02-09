import appService from '../app.service';
import { logAction, logMutation } from '../helpers/consoleHelpers';

const state = {
  selectedHttpVerb: 'GET',
  requestUrl: '',
  requestResult: ''
};

const getters = {
  selectedHttpVerb: currentState => currentState.selectedHttpVerb,
  requestResult: currentState => currentState.requestResult,
  requestUrl: currentState => currentState.requestUrl
};

const actions = {
  sendWiz(context) {
    logAction('sendWiz to ', context.state.requestUrl);
    appService.sendWiz({
      httpVerb: this.selectedHttpVerb,
      endpointUrl: context.state.requestUrl,
      body: '{}'
    }).then((data) => {
      context.commit('setRequestResult', { data });
    });
  },
  selectHttpVerb(context, { httpVerb }) {
    logAction('selectHttpVerb', httpVerb);
    context.commit('selectHttpVerb', { httpVerb });
  },
  setRequestUrl(context, { requestUrl }) {
    logAction('setRequestUrl', requestUrl);
    context.commit('setRequestUrl', { requestUrl });
  }
};

const mutations = {
  selectHttpVerb(currentState, { httpVerb }) {
    logMutation('selectedHttpVerb', httpVerb);
    currentState.selectedHttpVerb = httpVerb;
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

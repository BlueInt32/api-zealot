const state = {
  httpVerbs: ['GET',
    'POST',
    'PUT',
    'PATCH',
    'DELETE']

};

const getters = {
  httpVerbs: currentState => currentState.httpVerbs
};

const actions = {

};

const mutations = {

};

export default {
  namespaced: true,
  state,
  getters,
  actions,
  mutations
};

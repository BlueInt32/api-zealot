const state = {
  httpMethods: ['GET',
    'POST',
    'PUT',
    'PATCH',
    'DELETE']

};

const getters = {
  httpMethods: currentState => currentState.httpMethods
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

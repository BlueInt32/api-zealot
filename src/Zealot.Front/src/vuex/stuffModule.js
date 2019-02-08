const state = {
  text: '',
  modalVisible: false
};

const getters = {
  text: currentState => currentState.text,
};

const actions = {
  addStuff(context, { text, title }) {
    context.commit('actuallyDo', { text, title });
  }
};

const mutations = {
  actuallyDo(currentState, { text, title }) {
    currentState.text = `${title} - ${text}`;
  }
};

export default {
  namespaced: true,
  state,
  getters,
  actions,
  mutations
};

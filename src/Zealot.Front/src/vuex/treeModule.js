import { logAction } from '../helpers/consoleHelpers';

const state = {
  currentlySelectedUid: null
};

const getters = {
  defaultNewNodeName: () => 'New node',
  currentlySelectedUid: currentState => currentState.currentlySelectedUid
};

const actions = {
  allowDrag(context, model) {
    logAction('allowDrag', model);
    // can be dragged
    return true;
  },
  allowDrop(context, model) {
    logAction('allowDrop', model);
    return true;
  },
  curNodeClicked(context, { model, component }) {
    logAction('curNodeClicked', model, component);
    context.commit('setSelectedUid', { id: component._uid }); // eslint-disable-line
  },
  dragHandler() {
    // console.log('dragHandler: ', model, component, e);
  },
  dragEnterHandler() {
    // console.log('dragEnterHandler: ', model, component, e);
  },
  dragLeaveHandler() {
    // console.log('dragLeaveHandler: ', model, component, e);
  },
  dragOverHandler() {
    // console.log('dragOverHandler: ', model, component, e);
  },
  dragEndHandler() {
    // console.log('dragEndHandler: ', model, component, e);
  },
  dropHandler() {
    // console.log('dropHandler: ', model, component, e);
  }
};

const mutations = {
  setSelectedUid(currentState, { id }) {
    currentState.currentlySelectedUid = id;
  }
};

export default {
  namespaced: true,
  state,
  getters,
  actions,
  mutations
};

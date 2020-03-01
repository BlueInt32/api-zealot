import appService from '../app.service';
import { log, logAction } from '../helpers/consoleHelpers';

const state = {
  id: '',
  name: '',
  path: ''
};

const getters = {
  projectId: currentState => currentState.id,
  name: currentState => currentState.name
};

const actions = {
  getProjectDetails(context, { projectId }) {
    logAction('loadProject', projectId);
    return appService.getProjectDetails({ projectId })
      .then(data => {
        log('Received project details', data);
        context.dispatch('treeModule/setRawTree', data.tree, { root: true });
        context.commit('setProject', {
          id: data.id,
          name: data.name,
        });
      });
  },
  createProject(context, { projectName, projectPath }) {
    logAction('createProject', projectName, projectPath);
    appService.createProject({
      projectName,
      projectPath
    }).then((data) => {
      log(data);
    });
  },
  updateProject(context) {
    logAction('updateProject');
    const { id, name } = context.state;
    const { tree } = context.rootState.treeModule;
    appService.updateProject({
      projectId: id,
      projectName: name,
      tree
    }).then((data) => {
      log(data);
    });
  },
};

const mutations = {
  setProject(currentState, { id, name }) {
    currentState.id = id;
    currentState.name = name;
  }
};

export default {
  namespaced: true,
  state,
  getters,
  actions,
  mutations
};

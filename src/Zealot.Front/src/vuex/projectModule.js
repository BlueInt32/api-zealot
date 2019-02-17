import appService from '../app.service';
import { log, logAction } from '../helpers/consoleHelpers';

const state = {
  id: '',
  name: '',
  path: ''
};

const getters = {
  projectId: currentState => currentState.id,
  name: currentState => currentState.name,
  tree: currentState => currentState.tree
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
  saveProject(context, { projectName, projectPath }) {
    logAction('saveProject', projectName, projectPath);
    appService.saveProject({
      projectName,
      projectPath
    }).then((data) => {
      log(data);
    });
  }
};

const mutations = {
  setProject(currentState, { id, name, tree }) {
    currentState.id = id;
    currentState.name = name;
    currentState.tree = tree;
  }
};

export default {
  namespaced: true,
  state,
  getters,
  actions,
  mutations
};

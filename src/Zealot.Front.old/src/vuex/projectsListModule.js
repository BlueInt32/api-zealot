import appService from '../app.service';
import { logAction } from '../helpers/consoleHelpers';

const state = {
  projects: []
};

const getters = {
  projects: currentState => currentState.projects
};

const actions = {
  getProjectsList(context) {
    logAction('load projects');
    appService.getProjectsList().then((data) => {
      context.commit('setProjects', data);
    });
  }
};

const mutations = {
  setProjects(currentState, projects) {
    currentState.projects = projects;
  }
};

export default {
  namespaced: true,
  state,
  getters,
  actions,
  mutations
};

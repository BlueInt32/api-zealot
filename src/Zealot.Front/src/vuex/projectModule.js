import appService from '../app.service';
import { log, logAction } from '../helpers/consoleHelpers';

const state = {
  projectName: '',
  projectPath: ''
};

const getters = {

};

const actions = {
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

};

export default {
  namespaced: true,
  state,
  getters,
  actions,
  mutations
};

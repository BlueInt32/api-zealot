import appService from '../app.service';
import { log, logAction } from '../helpers/consoleHelpers';

const state = {
  projectName: '',
  projectFolder: ''
};

const getters = {

};

const actions = {
  saveProject(context, { projectName, projectFolder }) {
    logAction('saveProject', projectName, projectFolder);
    appService.saveProject({
      projectName,
      projectFolder
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

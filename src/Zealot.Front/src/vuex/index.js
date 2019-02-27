import Vue from 'vue';
import Vuex from 'vuex';
import configModule from './configModule';
import requestModule from './requestModule';
import projectModule from './projectModule';
import projectsListModule from './projectsListModule';
import treeModule from './treeModule';

Vue.use(Vuex);

const state = {
  isAuthenticated: false,
  currentCatalog: {}
};
const store = new Vuex.Store({
  modules: {
    configModule,
    requestModule,
    projectModule,
    projectsListModule,
    treeModule
  },
  state,
  getters: {},
  actions: {},
  mutations: {}
});

export default store;

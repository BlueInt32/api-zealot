import Vue from 'vue';
import Vuex from 'vuex';
import configModule from './configModule';
import wizModule from './wizModule';
import projectModule from './projectModule';
import projectsListModule from './projectsListModule';

Vue.use(Vuex);

const state = {
  isAuthenticated: false,
  currentCatalog: {}
};
const store = new Vuex.Store({
  modules: {
    configModule,
    wizModule,
    projectModule,
    projectsListModule
  },
  state,
  getters: {},
  actions: {},
  mutations: {}
});

export default store;

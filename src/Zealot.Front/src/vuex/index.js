import Vue from 'vue';
import Vuex from 'vuex';
import configModule from './configModule';
import wizModule from './wizModule';

Vue.use(Vuex);

const state = {
  isAuthenticated: false,
  currentCatalog: {}
};
const store = new Vuex.Store({
  modules: {
    configModule,
    wizModule
  },
  state,
  getters: {},
  actions: {},
  mutations: {}
});

export default store;

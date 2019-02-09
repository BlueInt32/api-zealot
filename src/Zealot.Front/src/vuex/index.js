import Vue from 'vue';
import Vuex from 'vuex';
import stuffModule from './stuffModule';
import configModule from './configModule';

Vue.use(Vuex);

const state = {
  isAuthenticated: false,
  currentCatalog: {}
};
const store = new Vuex.Store({
  modules: {
    stuffModule,
    configModule
  },
  state,
  getters: {},
  actions: {},
  mutations: {}
});

export default store;

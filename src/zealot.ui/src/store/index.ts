import Vue from 'vue';
import Vuex, { StoreOptions } from 'vuex';
import { RootState } from '@/store/states/RootState';

Vue.use(Vuex);

const store: StoreOptions<RootState> = {
  modules: {
    // why is this object empty ? Because modules are loaded dynamically !
    // https://championswimmer.in/vuex-module-decorators/pages/advanced/dynamic.html#step-1-create-the-store
  }
};

export default new Vuex.Store<RootState>(store);

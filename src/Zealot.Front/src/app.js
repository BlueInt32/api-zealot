import Vue from 'vue';
import BootstrapVue from 'bootstrap-vue';
import store from './vuex/index';
import AppLayout from './vueFiles/Layout.vue';
import 'bootstrap-vue/dist/bootstrap-vue.css';
import router from './router';

const app = new Vue({
  router,
  ...AppLayout,
  store
});

Vue.use(BootstrapVue);

export { app, router, store };

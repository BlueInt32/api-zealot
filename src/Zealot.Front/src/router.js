import Vue from 'vue';
import VueRouter from 'vue-router';
import Home from './vueFiles/Home.vue';
import NotFound from './vueFiles/NotFound.vue';

Vue.use(VueRouter);

const router = new VueRouter({
  mode: 'history', // allows url without the /#/ between host and path
  linkExactActiveClass: 'is-active',
  scrollBehavior: (to, from, savedPosition) => {
    if (savedPosition) {
      return savedPosition;
    }
  },
  routes: [
    { path: '/', component: Home },
    { path: '/random', component: Home },
    { path: '*', component: NotFound },
  ]
});

export default router;

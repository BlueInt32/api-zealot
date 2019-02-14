import Vue from 'vue';
import VueRouter from 'vue-router';
import Home from './vueFiles/Home.vue';
import NotFound from './vueFiles/NotFound.vue';
import ProjectsList from './vueFiles/ProjectsList.vue';

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
    { path: '/projects', component: ProjectsList },
    { path: '/random', component: Home },
    { path: '*', component: NotFound },
  ]
});

export default router;

import Vue from 'vue';
import VueRouter from 'vue-router';
import NotFound from './vueFiles/NotFound.vue';
import ProjectsList from './vueFiles/ProjectsList.vue';
import ProjectView from './vueFiles/ProjectView.vue';

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
    { name: 'defaultPage', path: '/', component: ProjectsList },
    { name: 'projectView', path: '/projects/:projectId', component: ProjectView },
    { name: 'projectList', path: '/projects', component: ProjectsList },
    { name: 'notFound', path: '*', component: NotFound },
  ]
});

export default router;

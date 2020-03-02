import Vue from 'vue';
import VueRouter from 'vue-router';
import Home from '@/views/Home.vue';
import ProjectsList from '@/views/ProjectsList.vue';

Vue.use(VueRouter);

const routes = [
  {
    name: 'projects',
    path: '/projects',
    component: () => import('@/views/ProjectsList.vue')
  },
  {
    path: '/',
    name: 'Home',
    component: Home
  },
  {
    path: '/about',
    name: 'About',
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: () => import(/* webpackChunkName: "about" */ '@/views/About.vue')
  }
  // { name: 'defaultPage', path: '/', component: ProjectsList },
  // { name: 'projectView', path: '/projects/:projectId', component: ProjectView },
  // { name: 'notFound', path: '*', component: NotFound }
];

const router = new VueRouter({
  mode: 'history',
  base: process.env.BASE_URL,
  routes
});

export default router;

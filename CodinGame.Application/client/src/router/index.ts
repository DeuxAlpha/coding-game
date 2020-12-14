import {createRouter, createWebHistory, RouteRecordRaw} from 'vue-router'
import Home from '../views/Home.vue'
import MarsLander from "@/views/mars-lander/View.vue";
import GreatEscape from "@/views/great-escape/View.vue";

const routes: Array<RouteRecordRaw> = [{
  path: '/',
  name: 'Home',
  component: Home
}, {
  path: '/mars-lander',
  name: 'MarsLander',
  component: MarsLander
}, {
  path: '/great-escape',
  name: 'GreatEscape',
  component: GreatEscape
}];

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
})

export default router

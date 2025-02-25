import { createRouter, createWebHistory } from 'vue-router'
import ProductList from '@/UI/ProductList.vue'
import AuthModal from '@/UI/AuthModal.vue'
import CreateProduct from '@/UI/CreateProduct.vue'
import CheckOut from '@/UI/CheckOut.vue'


const routes = [
  { path: '/', component: ProductList },
  { path: '/auth', component: AuthModal },
  {  path: '/create',component: CreateProduct},
  {  path: '/checkout',component: CheckOut},

]

const router = createRouter({
  history: createWebHistory(),
  routes,
})

export default router

import { createRouter, createWebHistory } from "vue-router";
import ProductList from "@/UI/ProductList.vue";
import AuthModal from "@/UI/AuthModal.vue";
import CreateProduct from "@/UI/CreateProduct.vue";
import EditProduct from "@/UI/EditProduct.vue";
import CheckOut from "@/UI/CheckOut.vue";
import CallBack from "@/UI/CallBack.vue";


const routes = [
  { path: "/", component: ProductList },
  { path: "/auth", component: AuthModal },
  { path: "/create", component: CreateProduct },
  { path: "/edit", component: EditProduct },
  { path: "/checkout", component: CheckOut },
  { path: '/CallBack',name: 'CallBack', component: CallBack,}
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;

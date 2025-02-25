import { createApp } from "vue";
import App from "./App.vue";
import ProductList from "./UI/ProductList.vue";
import AuthModal from "./UI/AuthModal.vue";
import CreateProduct from "./UI/CreateProduct.vue";
import Checkout from "./UI/Checkout.vue";
import router from "./router";

// Mount main Vue app
const app = createApp(App);
app.use(router);
app.mount("#app");

const productListApp = createApp(ProductList);
productListApp.mount("#product-list");

const authModalApp = createApp(AuthModal);
authModalApp.mount("#auth-modal");

const createProductApp = createApp(CreateProduct);
createProductApp.mount("#create-product");

const appCheckOut = createApp(Checkout);
appCheckOut.use(router);
appCheckOut.mount("#checkout-app");

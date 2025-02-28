<template>
  <div>
    <!-- Button to trigger the offcanvas cart -->
    <ul class="d-flex justify-content-end list-unstyled m-0">
      <div id="auth-modal"></div>
      <!-- Separate Section for Auth Modal -->
      <li></li>
      <li>
        <a href="#" class="p-2 mx-1" @click="toggleCart">
          <svg width="24" height="24">
            <use xlink:href="#shopping-bag"></use>
          </svg>
        </a>
      </li>
    </ul>

    <!-- Offcanvas cart section -->
    <div
      class="offcanvas offcanvas-end"
      :class="{ show: isCartVisible }"
      tabindex="-1"
      id="offcanvasCart"
      aria-labelledby="offcanvasCartLabel"
    >
      <div class="offcanvas-header">
        <h5 id="offcanvasCartLabel">Shopping Cart</h5>
        <button type="button" class="btn-close" @click="toggleCart" aria-label="Close"></button>
      </div>
      <div class="offcanvas-body">
        <div v-for="(item, index) in cart" :key="index" class="cart-item">
          <h6>{{ item.productName }}</h6>
          <p>Quantity: {{ item.quantity }}</p>
          <p>Price: ${{ item.price }}</p>
          <p>Total: ${{ (item.price * item.quantity).toFixed(2) }}</p>
        </div>
        <div class="d-flex justify-content-between">
          <h5>Total:</h5>
          <h5>${{ cartTotal.toFixed(2) }}</h5>
        </div>
        <button class="btn btn-primary w-100 mt-3" @click="checkout">Proceed to Checkout</button>
      </div>
    </div>
    <div class="mb-3">
    <label for="sortOrder" class="form-label">Sort by:</label>
    <select v-model="sortOrder" @change="fetchSortedProducts" class="form-select">
      <option value="default">Default</option>
      <option value="asc">Price: Low to High</option>
      <option value="desc">Price: High to Low</option>
      <option value="oldest">Oldest First</option>
      <option value="newest">Newest First</option>
    </select>
  </div>

  <div class="slider-container">
    <p>Price Range: ${{ minPrice }} - ${{ maxPrice }}</p>

    <div class="slider">
      <input type="range" v-model="minPrice" min="0" max="50" @input="fetchSortedProducts" />
      <input type="range" v-model="maxPrice" min="51" max="100" @input="fetchSortedProducts" />
    </div>
  </div>

    <button v-if="isAuthenticated && isAdmin" @click="showCreateProduct" class="btn btn-primary">
      Create Product
    </button>

    <button v-if="isAuthenticated && isAdmin" @click="showCreateCoupon" class="btn btn-primary">
      Create Coupon
    </button>

    <router-view></router-view>
    <Modal :visible="isModalVisible" @close="hideModal">
      <template v-if="isEditMode">
        <EditProduct :product="currentProduct" />
      </template>
      <template v-else>
        <CreateProduct />
      </template>
      <template v-else>
        <CreateCoupon/>
      </template>
    </Modal>

    <div class="row">
      <div class="col-md-12">
        <div class="swiper">
          <div class="swiper-wrapper">
            <div class="product-item swiper-slide">
              <div
                class="product-grid row row-cols-1 row-cols-sm-2 row-cols-md-3 row-cols-lg-3 row-cols-xl-4 row-cols-xxl-5"
              >
                <div v-for="(product, index) in products" :key="index" class="col">
                  <div class="product-item">
                    <figure>
                      <a href="index.html" title="Product Title">
                        <img
                          :src="imageSrc(product)"
                          :alt="product.productName"
                          class="tab-image"
                        />
                      </a>
                    </figure>
                    <div class="d-flex flex-column text-center">
                      <h3 class="fs-6 fw-normal">{{ product.productName }}</h3>
                      <div class="d-flex justify-content-center align-items-center gap-2">
                        <span class="text-dark fw-semibold">${{ product.price }}</span>
                      </div>
                      <div class="button-area p-3 pt-0">
                        <div class="row g-1 mt-2">
                          <div class="col-3">
                            <input
                              type="number"
                              class="form-control border-dark-subtle input-number quantity"
                              v-model.number="product.quantity"
                              min="0"
                              value="0"
                              placeholder="0"
                            />
                          </div>
                          <div class="col-7">
                            <button
                              @click="addToCart(product)"
                              class="btn btn-primary rounded-1 p-2 fs-7 btn-cart"
                            >
                              <svg width="18" height="18">
                                <use xlink:href="#cart"></use>
                              </svg>
                              Add to Cart
                            </button>
                          </div>
                          <div class="col-2">
                            <div v-if="isAuthenticated && isAdmin">
                              <a
                                href="#"
                                @click="deleteProduct(product.productId)"
                                class="btn btn-danger btn-outline-dark rounded-1 p-2 fs-6"
                              >
                                <svg width="18" height="18">
                                  <use xlink:href="#trash"></use>
                                </svg>
                              </a>

                              <button
                                v-if="isAuthenticated && isAdmin"
                                @click="showEditProduct(product)"
                                class="btn btn-danger"
                              >
                                Edit Product
                              </button>
                            </div>
                          </div>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.btn .btn-danger {
  width: 100%;
}
</style>

<script setup>
import { ref, onMounted, computed, onBeforeUnmount, nextTick } from "vue";
import { useRouter } from "vue-router";
import axios from "axios";
import Modal from "./Modal.vue";
import CreateProduct from "./CreateProduct.vue";
import EditProduct from "./EditProduct.vue";
import CreateCoupon from "./CreateCoupon.vue";

const minPrice = ref(0);
const maxPrice = ref(100);
const sortOrder = ref("default");
const products = ref([]);
const isAuthenticated = ref(!!localStorage.getItem("token"));
const isAdmin = ref(false);
const userId = ref(null);
const isModalVisible = ref(false);
const isCartVisible = ref(false);
const isEditMode = ref(false);
const currentProduct = ref(null);
const api = "https://localhost:7229/api/FishProductAPI";
const cart = ref([]);


// Import and initialize useRouter correctly
const router = useRouter();

const loadAuthState = () => {
  const token = localStorage.getItem("token");
  const user = JSON.parse(localStorage.getItem("user"));
  const role = localStorage.getItem("role");

  if (token && user && role) {
    isAuthenticated.value = true;
    isAdmin.value = role === "admin";
    userId.value = user.id;
  }
};

const addToCart = (product) => {
  if (product.quantity === 0 || product.quantity === "" || product.quantity == null) {
    alert("Product quantity have not selected yet!");
    return;
  }
  const existingProduct = cart.value.find((item) => item.productId === product.productId);

  if (existingProduct) {
    existingProduct.quantity += product.quantity;
  } else {
    cart.value.push({ ...product, quantity: product.quantity });
    alert("Product have been selected ! You can check your cart now");
  }
};

const cartTotal = computed(() => {
  return cart.value.reduce((total, item) => total + item.price * item.quantity, 0);
});

const toggleCart = () => {
  isCartVisible.value = !isCartVisible.value;
};

const imageSrc = (product) => {
  if (product && product.imageURl) {
    if (product.imageURl.startsWith("http")) {
      return product.imageURl;
    } else {
      return `src/assets/images/${product.imageURl}`;
    }
  }
  return "path/to/default-image.png";
};

const fetchSortedProducts = () => {
  let apiUrl = `https://localhost:7229/api/FishProductAPI/GetProductInRange?min=${minPrice.value}&max=${maxPrice.value}`;
  
    switch (sortOrder.value) {
      case "asc":
        apiUrl = `https://localhost:7229/api/FishProductAPI/GetProductAsc?min=${minPrice.value}&max=${maxPrice.value}`;
        break;
      case "desc":
        apiUrl = `https://localhost:7229/api/FishProductAPI/GetProductDesc?min=${minPrice.value}&max=${maxPrice.value}`;
        break;
      case "oldest":
        apiUrl = `https://localhost:7229/api/FishProductAPI/GetProductOldest?min=${minPrice.value}&max=${maxPrice.value}`;
        break;
      case "newest":
        apiUrl = `https://localhost:7229/api/FishProductAPI/GetProductNewest?min=${minPrice.value}&max=${maxPrice.value}`;
        break;
    }
  

  console.log("Fetching data from:", apiUrl); // Debugging log

  axios
    .get(apiUrl)
    .then((response) => {
      if (response.data.isSuccess) {
        products.value = response.data.result;
      } else {
        console.error("Error fetching data:", response.data.errorMessages);
      }
    })
    .catch((error) => {
      console.error("Error fetching data:", error);
    });
};


// Fetch products on load
const getProductData = () => {
  fetchSortedProducts();
};



const deleteProduct = (productId) => {
  const token = localStorage.getItem("token");
  axios
    .delete(`${api}/${productId}`, {
      headers: { Authorization: `Bearer ${token}` },
    })
    .then(() => {
      getProductData();
      alert("Product deleted successfully!");
    })
    .catch((error) => {
      console.error("Error deleting product:", error);
      alert("Error deleting product. Please try again.");
    });
};

// const checkout = () => {   using for push in the router and rechive in the same html
//   router.push({ path: '/checkout', query: { cart: JSON.stringify(cart.value), total: cartTotal.value } });
// };

const checkout = () => {
  const cartData = JSON.stringify(cart.value);
  const totalData = cartTotal.value;

  localStorage.setItem("cart", cartData);
  localStorage.setItem("total", totalData);

  window.location.href = "checkout.html";
};
const trackView = async () => {
  try {
    await axios.post("https://localhost:7229/api/Analytics/TrackView");
  } catch (error) {
    console.error("Error tracking view:", error);
  }
};

const startSession = async () => {
  try {
    await axios.post("https://localhost:7229/api/Analytics/StartSession");
  } catch (error) {
    console.error("Error starting session:", error);
  }
};

const endSession = async () => {
  try {
    await axios.post("https://localhost:7229/api/Analytics/EndSessions");
  } catch (error) {
    console.error("Error ending session:", error);
  }
};

onBeforeUnmount(() => {
  window.removeEventListener("beforeunload", endSession);
});

const showCreateProduct = () => {
  isEditMode.value = false;
  currentProduct.value = null; 
  isModalVisible.value = true;
  document.querySelector(".swiper").style.display = "none";
};

const showCreateCoupon = () => {
  isEditMode.value = false;
  currentProduct.value = null; 
  isModalVisible.value = true;
  document.querySelector(".swiper").style.display = "none";
};


const showEditProduct = (product) => {
  isEditMode.value = true;
  console.log(product);
  currentProduct.value = product; // Set current product for editing
  isModalVisible.value = true;
  document.querySelector(".swiper").style.display = "none";
};

const hideModal = () => {
  isModalVisible.value = false;
  document.querySelector(".swiper").style.display = "block";
};

onMounted(() => {
  trackView();
  loadAuthState();
  getProductData();
  startSession();

  window.addEventListener("beforeunload", endSession);
});

onMounted(() => {
  trackView();
  loadAuthState();
  getProductData();
  startSession();

  window.addEventListener("beforeunload", endSession);
});
</script>

<style src="@/assets/css/style.css"></style>
<style src="@/assets/css/vendor.css"></style>
<style>
@import url("https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css");
</style>

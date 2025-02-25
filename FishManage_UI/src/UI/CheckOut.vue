<template>
  <div>
    <div v-for="(item, index) in cart" :key="index">
      <h6>{{ item.productName }}</h6>
      <p>Quantity: {{ item.quantity }}</p>
      <p>Price: ${{ item.price }}</p>
      <p>Total: ${{ (item.price * item.quantity).toFixed(2) }}</p>
    </div>
    <div class="d-flex justify-content-between">
      <h5>Total:</h5>
      <h5>${{ total.toFixed(2) }}</h5>
    </div>
  </div>
</template>

<script>
export default {
  name: "Checkout",
  data() {
    return {
      cart: [],
      total: 0,
    };
  },
  mounted() {
    const cartData = localStorage.getItem("cart");
    const totalData = localStorage.getItem("total");
    if (cartData && totalData) {
      this.cart = JSON.parse(cartData);
      this.total = parseFloat(totalData);
    }
  },
};
</script>

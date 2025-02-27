<template>
  <div class="row">
    <form @submit.prevent="createPayment" class="col-6">
      <div class="mb-3">
        <label for="name" class="form-label">Name</label>
        <input type="text" class="form-control" v-model="userInfo.FullName" required />
      </div>
      <div class="mb-3">
        <label for="phoneNumber" class="form-label">Phone Number</label>
        <input type="text" class="form-control" v-model="userInfo.phoneNumber" required />
      </div>
      <div class="mb-3">
        <label for="address" class="form-label">Address</label>
        <input type="text" class="form-control" v-model="userInfo.address" required />
      </div>
      <div class="mb-3">
        <label for="city" class="form-label">City</label>
        <input type="text" class="form-control" v-model="userInfo.city" required />
      </div>
      <div class="mb-3">
        <label for="zipCode" class="form-label">Zip Code</label>
        <input type="text" class="form-control" v-model="userInfo.zipCode" required />
      </div>
      <div class="mb-3">
        <label for="email" class="form-label">Email</label>
        <input type="email" class="form-control" v-model="userInfo.email" required />
      </div>
      <button
        type="submit"
        class="btn btn-primary btn-block btn-lg"
        style="background-color: palevioletred"
      >
        Pay By MoMo
      </button>
    </form>

    <!-- Order Recap -->
    <div class="col-6">
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
      <button
        type="submit"
        class="btn btn-primary btn-block btn-lg"
        style="background-color: green"
      >
        Pay By COD
      </button>
      <button type="submit" class="btn btn-primary btn-block btn-lg">Pay By VNPay</button>
      <h6>
        <a href="http://localhost:5173/">Cancel and return to website</a>
      </h6>
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
      userInfo: {
        FullName: "",
        phoneNumber: "",
        address: "",
        city: "",
        zipCode: "",
        email: "",
      },
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
  methods: {
    async handlePaymentCallback() {
      const urlParams = new URLSearchParams(window.location.search);
      const resultCode = urlParams.get("resultCode");
      const orderId = urlParams.get("orderId");

      if (!resultCode) return;

      if (resultCode === "0") {
        alert("Payment successful! Your order has been placed.");
        this.$router.push({ path: '/CallBack' });
      } else {
        alert("Payment canceled or failed. Please try again.");
      }
      window.history.replaceState(null, null, "/checkout");
    },

    async createPayment() {
      try {
        const response = await fetch("https://localhost:7229/MoMoPaymentAPI/CreatePaymentMomo", {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify({
            FullName: this.userInfo.FullName,
            OrderId: new Date().getTime().toString(),
            Orderinformation: "Your order",
            Amount: Math.round(this.total * 25545),
          }),
        });

        const textResponse = await response.text();
        console.log("Server response:", textResponse);

        if (!response.ok) {
          throw new Error(textResponse);
        }

        const data = JSON.parse(textResponse);
        window.location.href = data.payUrl;
      } catch (error) {
        console.error("Error creating payment:", error);
      }
    },
  },
};
</script>

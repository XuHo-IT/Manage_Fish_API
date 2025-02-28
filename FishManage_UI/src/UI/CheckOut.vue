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
      <div class="mb-3">
        <label for="province" class="form-label">Province</label>
        <select id="province" class="form-control" v-model="selectedProvince">
          <option v-for="province in provinces" :key="province.id" :value="province.id">
            {{ province.full_name }}
          </option>
        </select>
      </div>
      <div class="mb-3">
        <label for="district" class="form-label">District</label>
        <select id="district" class="form-control" v-model="selectedDistrict">
          <option v-for="district in districts" :key="district.id" :value="district.id">
            {{ district.full_name }}
          </option>
        </select>
      </div>
      <div class="mb-3">
        <label for="ward" class="form-label">Ward</label>
        <select id="ward" class="form-control" v-model="selectedWard">
          <option v-for="ward in wards" :key="ward.id" :value="ward.id">
            {{ ward.full_name }}
          </option>
        </select>
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
      <div class="d-flex justify-content-between">
        <h5>Shipping:</h5>
        <h5 v-if="shippingPrice !== null">{{ shippingPrice }} $</h5>
      </div>
      <div class="mb-3">
        <label for="couponCode" class="form-label">Coupon Code</label>
        <input type="text" class="form-control" v-model="selectedCouponCode" required />
        <div v-if="selectedCoupon">
        <p>{{ selectedCoupon.couponDescription }}</p>
        </div>
        <div v-else>
        <p>No Coupon Code Available</p>
        </div>

        <button
          type="button"
          class="btn btn-primary btn-block btn-lg"
          style="background-color: green"
          @click="checkCoupon"
        >
          Check Coupon
        </button>
      </div>

  

      <button
        type="button"
        class="btn btn-primary btn-block btn-lg"
        style="background-color: green"
        @click="createPaymentByCOD"
      >
        Pay By COD
      </button>

      <button type="submit" class="btn btn-primary btn-block btn-lg">Pay By VNPay</button>
      <h6>
        <a href="http://localhost:5173/">Cancel and return to website</a>
      </h6>
      <router-view></router-view>
    </div>
  </div>
</template>

<style>
.css_select_div {
  text-align: center;
}
.css_select {
  display: inline-table;
  width: 25%;
  padding: 5px;
  margin: 5px 2%;
  border: solid 1px #686868;
  border-radius: 5px;
}
</style>

<script>
import { ref, onMounted, watch } from "vue";
import { useRouter } from "vue-router";

export default {
  name: "Checkout",
  setup() {
    const cart = ref([]);
    const total = ref(0);
    const userInfo = ref({
      FullName: "",
      phoneNumber: "",
      address: "",
      city: "",
      zipCode: "",
      email: "",
    });
    const shippingPrice = ref(0);

    const router = useRouter();
    // Location data
    const provinces = ref([]);
    const districts = ref([]);
    const wards = ref([]);
    const selectedProvince = ref("0");
    const selectedDistrict = ref("0");
    const selectedWard = ref("0");

    const coupons = ref([]);
    const selectedCouponCode = ref("");
    const selectedCoupon = ref(null);

    const fetchCoupons = async () => {
      try {
        const response = await fetch("https://localhost:7229/api/CouponModel");
        const data = await response.json();
        if (data.isSuccess) {
          coupons.value = data.result;
          console.log(coupons.value);
        }
      } catch (error) {
        console.error("Error fetching coupons:", error);
      }
    };
    const checkCoupon = () => {
      selectedCoupon.value =
        coupons.value.find((c) => c.couponCode === selectedCouponCode.value) || null;
    };
    // Fetch locations
    const fetchProvinces = async () => {
      try {
        const response = await fetch("https://esgoo.net/api-tinhthanh/1/0.htm");
        const data = await response.json();
        if (data.error === 0) {
          provinces.value = data.data;
        }
      } catch (error) {
        console.error("Error fetching provinces:", error);
      }
    };

    watch(selectedProvince, async (newVal) => {
      if (newVal !== "0") {
        try {
          const response = await fetch(`https://esgoo.net/api-tinhthanh/2/${newVal}.htm`);
          const data = await response.json();
          if (data.error === 0) {
            districts.value = data.data;
            wards.value = []; // Reset wards
          }
        } catch (error) {
          console.error("Error fetching districts:", error);
        }
      }
    });

    watch(selectedDistrict, async (newVal) => {
      if (newVal !== "0") {
        try {
          const response = await fetch(`https://esgoo.net/api-tinhthanh/3/${newVal}.htm`);
          const data = await response.json();
          if (data.error === 0) {
            wards.value = data.data;
          }
        } catch (error) {
          console.error("Error fetching wards:", error);
        }
      }
    });

    watch([selectedProvince, selectedDistrict, selectedWard], () => {
      const province =
        provinces.value.find((p) => p.id === selectedProvince.value)?.full_name || "";
      const district =
        districts.value.find((d) => d.id === selectedDistrict.value)?.full_name || "";
      const ward = wards.value.find((w) => w.id === selectedWard.value)?.full_name || "";

      userInfo.value.address = [ward, district, province].filter(Boolean).join(", ");
    });

    watch(
      userInfo,
      (newVal) => {
        if (Object.values(newVal).every((field) => field.trim() !== "")) {
          shippingPrice.value = (Math.random() * (2 - 0.25) + 0.25).toFixed(2);
        }
      },
      { deep: true },
    );

    onMounted(() => {
      const cartData = localStorage.getItem("cart");
      const totalData = localStorage.getItem("total");
      if (cartData && totalData) {
        cart.value = JSON.parse(cartData);
        total.value = parseFloat(totalData);
      }
      fetchCoupons();
      fetchProvinces();
    });
    const createPayment = async () => {
      try {
        const response = await fetch("https://localhost:7229/PaymentAPI/CreatePaymentMomo", {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify({
            FullName: userInfo.value.FullName,
            OrderId: new Date().getTime().toString(),
            Orderinformation: "Your order",
            Amount: Math.round(total.value * 25545) + +shippingPrice.value,
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
    };
    const createPaymentByCOD = async () => {
      try {
        const response = await fetch("https://localhost:7229/PaymentAPI/CreatePaymentCOD", {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify({
            userId: new Date().getTime().toString(),
            orderDate: new Date().toISOString(),
            totalAmount: Math.round(total.value * 25545).toString() + +shippingPrice.value,
            paymentMethod: "COD",
            productId: cart.value.map((item) => item.productId).join(","),
            quantity: cart.value.reduce((sum, item) => sum + item.quantity, 0),
            unitPrice: cart.value.reduce((sum, item) => sum + item.price, 0),
          }),
        });
        const textResponse = await response.json();
        console.log("Server response:", textResponse);
        router.push({
          name: "CallBack",
          query: {
            orderId: textResponse.result.orderId,
            userId: textResponse.result.userId,
            amount: textResponse.result.totalAmount + shippingPrice.value,
            payType: textResponse.result.paymentMethod,
            status: textResponse.isSuccess ? "success" : "failed",
          },
        });

        if (!response.ok) {
          throw new Error(textResponse);
        }
      } catch (error) {
        console.error("Error creating payment:", error);
      }
    };

    return {
      selectedCouponCode,
      selectedCoupon,
      checkCoupon,
      cart,
      total,
      userInfo,
      provinces,
      districts,
      wards,
      selectedProvince,
      selectedDistrict,
      selectedWard,
      createPayment,
      createPaymentByCOD,
      shippingPrice,
    };
  },
};
</script>

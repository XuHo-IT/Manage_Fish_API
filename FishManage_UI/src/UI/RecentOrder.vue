<template>
  <table>
    <thead>
      <tr>
        <th>ORDER ID</th>
        <th>PRDUCT ID</th>
       <th>QUANTITY</th>
        <th>PAYMENT</th>
        <th>ORDER DATE</th>
        <th class="text-end">TOTAL AMOUNT</th>
      </tr>
    </thead>
    <tbody>
      <tr v-for="(order, index) in orders" :key="index">
        <td>
          <a href="#" class="text-muted">{{ order.orderId }}</a>
        </td>
        <td>{{ order.productId }}</td>
        <td>{{ order.quantity }}</td>
        <td>{{ order.paymentMethod }}</td>
        <td>{{ order.orderDate }}</td>
        <td class="text-end">${{ order.totalAmount }}</td>
      </tr>
    </tbody>
  </table>
</template>

<script>
import axios from "axios";
import { ref, onMounted } from "vue";

axios.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem("token");
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => Promise.reject(error),
);

export default {
  name: "TotalReport",
  setup() {
    const orders = ref([]);
    const api = "https://localhost:7229/api/FishOrderAPI";

    const getTotalOrders = async () => {
      try {
        const response = await axios.get(api);
        if (response.data.isSuccess) {
          orders.value = response.data.result;
        } else {
          console.error("Error: " + response.data.errorMessage);
        }
      } catch (error) {
        console.error("Failed to fetch orders:", error);
      }
    };

    onMounted(getTotalOrders);

    return {
      orders,
    };
  },
};
</script>

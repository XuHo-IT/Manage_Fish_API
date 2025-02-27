<template>
  <div class="row">
    <div class="col-md-6 col-xl-4">
      <div class="card">
        <div class="card-body">
          <h6 class="mb-2 f-w-400 text-muted">Total Users</h6>
          <h4 class="mb-3">
            {{ totalUsers }}
            <span class="badge bg-light-success border border-success"
              ><i class="ti ti-trending-up"></i> 70.5%</span
            >
          </h4>
          <p class="mb-0 text-muted text-sm">
            You made an extra <span class="text-success">8,900</span> this year
          </p>
        </div>
      </div>
    </div>
    <div class="col-md-6 col-xl-4">
      <div class="card">
        <div class="card-body">
          <h6 class="mb-2 f-w-400 text-muted">Total Product</h6>
          <h4 class="mb-3">
            {{ totalProducts }}
            <span class="badge bg-light-warning border border-warning"
              ><i class="ti ti-trending-down"></i> 27.4%</span
            >
          </h4>
          <p class="mb-0 text-muted text-sm">
            You made an extra <span class="text-warning">1,943</span> this year
          </p>
        </div>
      </div>
    </div>
    <div class="col-md-6 col-xl-4">
      <div class="card">
        <div class="card-body">
          <h6 class="mb-2 f-w-400 text-muted">Total Sales</h6>
          <h4 class="mb-3">
            {{totalOrders}}
            <span class="badge bg-light-danger border border-danger"
              ><i class="ti ti-trending-down"></i> 27.4%</span
            >
          </h4>
          <p class="mb-0 text-muted text-sm">
            You made an extra
            <span class="text-danger">$20,395</span> this year
          </p>
        </div>
      </div>
    </div>
  </div>
</template>
<script>
import axios from "axios";

const apiUser = "https://localhost:7229/api/UserAuth";
const apiProduct = "https://localhost:7229/api/FishProductAPI";
const apiOrder = "https://localhost:7229/api/FishOrderAPI";
export default {
  name: "TotalReport",
  data() {
    return {
      totalUsers: 0,
      totalProducts: 0,
      totalOrders: 0,
      totalViews: 0,
      activeSessions: 0
    };
  },
  methods: {
    async getTotalUsers() {
      try {
        const response = await axios.get(apiUser);
        this.totalUsers = response.data.result.length;
      } catch (error) {
        console.error("Error fetching total users:", error);
      }
    },
    async getTotalProducts() {
      try {
        const response = await axios.get(apiProduct);
        this.totalProducts = response.data.result.length;
      } catch (error) {
        console.error("Error fetching total products:", error);
      }
    },
    async getTotalOrders() {
      try {
        const response = await axios.get(apiOrder);
        this.totalOrders = response.data.result.length;
      } catch (error) {
        console.error("Error fetching total orders:", error);
      }
    },
  },
  mounted() {
    this.getTotalUsers();
    this.getTotalProducts();
    this.getTotalOrders();
  },
};
</script>

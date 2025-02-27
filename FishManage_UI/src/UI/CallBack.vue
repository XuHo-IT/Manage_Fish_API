<template>
    <div class="payment-status">
      <div v-if="order">
        <h1 :class="{'text-green-500': isSuccess, 'text-red-500': !isSuccess}">
          {{ isSuccess ? 'Payment Successful!' : 'Payment Failed' }}
        </h1>
        <div class="order-details">
          <p><strong>Order ID:</strong> {{ order.orderId }}</p>
          <p><strong>User ID:</strong> {{ order.userId }}</p>
          <p><strong>Order Date:</strong> {{ order.orderDate }}</p>
          <p><strong>Total Amount:</strong> ${{ order.totalAmount }}</p>
          <p><strong>Payment Method:</strong> {{ order.paymentMethod }}</p>
        </div>
      </div>
      <div v-else>
        <p>Loading order details...</p>
      </div>
    </div>
  </template>
  
  <script>
  export default {
    data() {
      return {
        order: null,
        isSuccess: false,
      };
    },
    created() {
      const query = this.$route.query;
      this.order = {
        orderId: query.orderId,
        userId: query.userId,
        orderDate: new Date().toLocaleString(),
        totalAmount: query.amount,
        paymentMethod: query.payType,
      };
      this.isSuccess = query.status === 'success';
    },
  };
  </script>
  
  <style scoped>
  .payment-status {
    max-width: 600px;
    margin: 50px auto;
    padding: 20px;
    border: 1px solid #ddd;
    border-radius: 10px;
    background: #f9f9f9;
    text-align: center;
  }
  .order-details {
    margin-top: 20px;
    text-align: left;
  }
  </style>
  
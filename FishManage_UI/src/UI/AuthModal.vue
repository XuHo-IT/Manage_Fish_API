<template>
  <div>
    <li v-if="!isAuthenticated">
      <a href="#" @click="openModal" class="p-2 mx-1">
        <svg width="30" height="30" class="text-gray-600 hover:text-blue-500">
          <use xlink:href="#user"></use>
        </svg>
      </a>
    </li>
    <li v-if="isAuthenticated">
      <a href="#" @click="dashboard" class="p-2 mx-1">
        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 576 512">
          <path
            d="M151.6 42.4C145.5 35.8 137 32 128 32s-17.5 3.8-23.6 10.4l-88 96c-11.9 13-11.1 33.3 2 45.2s33.3 11.1 45.2-2L96 146.3 96 448c0 17.7 14.3 32 32 32s32-14.3 32-32l0-301.7 32.4 35.4c11.9 13 32.2 13.9 45.2 2s13.9-32.2 2-45.2l-88-96zM320 480l32 0c17.7 0 32-14.3 32-32s-14.3-32-32-32l-32 0c-17.7 0-32 14.3-32 32s14.3 32 32 32zm0-128l96 0c17.7 0 32-14.3 32-32s-14.3-32-32-32l-96 0c-17.7 0-32 14.3-32 32s14.3 32 32 32zm0-128l160 0c17.7 0 32-14.3 32-32s-14.3-32-32-32l-160 0c-17.7 0-32 14.3-32 32s14.3 32 32 32zm0-128l224 0c17.7 0 32-14.3 32-32s-14.3-32-32-32L320 32c-17.7 0-32 14.3-32 32s14.3 32 32 32z"
          />
        </svg>
      </a>
    </li>
    <li v-if="isAuthenticated" class="p-2 mx-1">
      <a href="#" @click="logout" class="p-2 mx-1">
        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 512 512">
          <path
            d="M377.9 105.9L500.7 228.7c7.2 7.2 11.3 17.1 11.3 27.3s-4.1 20.1-11.3 27.3L377.9 406.1c-6.4 6.4-15 9.9-24 9.9c-18.7 0-33.9-15.2-33.9-33.9l0-62.1-128 0c-17.7 0-32-14.3-32-32l0-64c0-17.7 14.3-32 32-32l128 0 0-62.1c0-18.7 15.2-33.9 33.9-33.9c9 0 17.6 3.6 24 9.9zM160 96L96 96c-17.7 0-32 14.3-32 32l0 256c0 17.7 14.3 32 32 32l64 0c17.7 0 32 14.3 32 32s-14.3 32-32 32l-64 0c-53 0-96-43-96-96L0 128C0 75 43 32 96 32l64 0c17.7 0 32 14.3 32 32s-14.3 32-32 32z"
          />
        </svg>
      </a>
    </li>

    <div
      v-if="showModal"
      class="fixed inset-0 flex items-center justify-center bg-black bg-opacity-50"
    >
      <div class="bg-white p-6 rounded-lg shadow-lg w-96 relative">
        <button
          @click="closeModal"
          class="absolute top-2 right-2 text-gray-500 hover:text-red-500 text-2xl"
        >
          &times;
        </button>

        <!-- Tabs -->
        <div class="flex mb-4">
          <button
            @click="isLogin = true"
            :class="{ 'border-blue-500': isLogin }"
            class="w-1/2 py-2 text-center border-b-2 font-semibold"
          >
            Login
          </button>
          <button
            @click="isLogin = false"
            :class="{ 'border-blue-500': !isLogin }"
            class="w-1/2 py-2 text-center border-b-2 font-semibold"
          >
            Register
          </button>
        </div>

        <!-- Display Errors -->
        <p v-if="errorMessage" class="text-red-500 text-sm mb-2">{{ errorMessage }}</p>

        <!-- Login Form -->
        <form v-if="isLogin" @submit.prevent="handleLogin">
          <input
            v-model="loginData.userName"
            type="text"
            placeholder="Username"
            class="w-full px-3 py-2 mb-2 border rounded"
            required
          />
          <input
            v-model="loginData.password"
            type="password"
            placeholder="Password"
            class="w-full px-3 py-2 mb-2 border rounded"
            required
          />
          <button
            type="submit"
            class="w-full bg-blue-500 text-white py-2 rounded hover:bg-blue-600"
          >
            Login
          </button>
        </form>

        <!-- Register Form -->
        <form v-else @submit.prevent="handleRegister">
          <input
            v-model="registerData.fullName"
            type="text"
            placeholder="Full Name"
            class="w-full px-3 py-2 mb-2 border rounded"
            required
          />
          <input
            v-model="registerData.userName"
            type="text"
            placeholder="Name"
            class="w-full px-3 py-2 mb-2 border rounded"
            required
          />
          <input
            v-model="registerData.password"
            type="password"
            placeholder="Password"
            class="w-full px-3 py-2 mb-2 border rounded"
            required
          />
          <button
            type="submit"
            class="w-full bg-green-500 text-white py-2 rounded hover:bg-green-600"
          >
            Register
          </button>
        </form>
      </div>
    </div>
  </div>
</template>

<script>
import api from "@/js/api_auth.js"; // Axios instance
import { ref } from "vue";

export default {
  data() {
    return {
      showModal: false,
      isLogin: true, // Toggle between login and register
      isAuthenticated: !!localStorage.getItem("token"),
      isAdmin: false,
      userId: null,
      loginData: { userName: "", password: "" },
      registerData: { fullName: "", userName: "", password: "" },
      errorMessage: "",
    };
  },
  methods: {
    openModal() {
      this.showModal = true;
    },
    closeModal() {
      this.showModal = false;
      this.clearForm();
      this.errorMessage = "";
    },
    clearForm() {
      this.loginData = { userName: "", password: "" };
      this.registerData = { fullName: "", userName: "", password: "" };
    },
    async handleLogin() {
      try {
        const response = await api.post("/login", this.loginData);
        localStorage.setItem("token", response.data.result.token);
        localStorage.setItem("user", JSON.stringify(response.data.result.user)); // Store user information
        localStorage.setItem("role", response.data.result.role); // Store role information
        this.isAdmin = response.data.result.role === "admin";
        this.isAuthenticated = true;
        this.userId = response.data.result.user.id;

        // Print isAdmin and userId to the console
        console.log("isAdmin:", this.isAdmin);
        console.log("userId:", this.userId);

        window.history.pushState({}, "", `?userId=${this.userId}`);
        this.closeModal();
        window.location.reload();
      } catch (error) {
        this.errorMessage = error.response?.data?.errorMessages?.[0] || "An error occurred";
      }
    },

    async handleRegister() {
      try {
        await api.post("/register", this.registerData);
        alert("Registration successful! Please log in.");
        this.isLogin = true;
        this.clearForm();
      } catch (error) {
        this.errorMessage = error.response?.data?.errorMessages?.[0] || "An error occurred";
      }
    },
  dashboard(){
    window.location.href = "/src/dist/dashboard/index.html";
    },
    logout() {
      localStorage.removeItem("token");
      this.isAuthenticated = false;
      this.isAdmin = false;
      this.userId = null;
      window.history.pushState({}, "", "/");
      window.location.reload();
    },
  },
};
</script>

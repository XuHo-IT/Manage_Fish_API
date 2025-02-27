<template>
  <div class="formbold-main-wrapper">
    <div class="formbold-form-wrapper">
      <form @submit.prevent="editProduct">
        <h2 class="formbold-form-title">Edit product</h2>

        <div class="formbold-input-flex">
          <div>
            <label for="productName" class="formbold-form-label">Product Name</label>
            <input
              type="text"
              name="productName"
              id="productName"
              class="formbold-form-input"
              v-model="product.productName"
              required
            />
          </div>
          <div>
            <label for="price" class="formbold-form-label">Price</label>
            <input
              type="number"
              name="price"
              id="price"
              class="formbold-form-input"
              v-model="product.price"
              required
            />
          </div>
        </div>

        <div class="formbold-input-flex">
          <div>
            <label for="category" class="formbold-form-label">Category</label>
            <input
              type="text"
              name="category"
              id="category"
              class="formbold-form-input"
              v-model="product.category"
              required
            />
          </div>
        </div>

        <div class="formbold-input-flex">
          <div>
            <label for="description" class="formbold-form-label">Description</label>
            <input
              type="text"
              name="description"
              id="description"
              class="formbold-form-input"
              v-model="product.description"
              required
            />
          </div>
        </div>

        <div class="formbold-input-flex">
          <div>
            <label for="supplier" class="formbold-form-label">Supplier</label>
            <input
              type="text"
              name="supplier"
              id="supplier"
              class="formbold-form-input"
              v-model="product.supplier"
              required
            />
          </div>
        </div>

        <div>
          <label for="imageURl" class="formbold-form-label">Image URL</label>
          <input
            type="file"
            name="imageURl"
            id="imageURl"
            class="formbold-form-input"
            @change="handleFileUploadUpdate"
          />
        </div>

        <button class="formbold-btn btn-danger">Edit</button>
      </form>
    </div>
  </div>
</template>

<script>
import axios from "axios";

axios.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem("token");
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => {
    return Promise.reject(error);
  },
);

export default {
  name: "EditProduct",
  props: {
    product: {
      type: Object,
      required: true,
    },
  },
  data() {
    return {
      imageFile: null,
    };
  },
  methods: {
    handleFileUploadUpdate(event) {
      this.imageFile = event.target.files[0];
      console.log(this.imageFile);
    },

    async editProduct() {
      if (!this.product.productId) {
        alert("Product ID is missing!");
        return;
      }

      const formData = new FormData();
      formData.append("ProductId", this.product.productId);
      formData.append("ProductName", this.product.productName);
      formData.append("Price", this.product.price);
      formData.append("Category", this.product.category);
      formData.append("Description", this.product.description);
      formData.append("Supplier", this.product.supplier);

      // Always append imageFile to avoid validation errors
      if (this.imageFile) {
        formData.append("imageFile", this.imageFile);
      }
      try {
        const response = await axios.put(
          `https://localhost:7229/api/FishProductAPI/${this.product.productId}`,
          formData,
          {
            headers: {
              "Content-Type": "multipart/form-data", // ✅ Fixes 415 error
            },
          },
        );

        alert("Product updated successfully!");
        console.log("Product updated successfully:", response.data);
      } catch (error) {
        alert("Error updating product!");
        console.error("Error updating product:", error.response ? error.response.data : error);
      }
    },
  },
};
</script>

<style src="@/assets/css/style.css"></style>
<style src="@/assets/css/vendor.css"></style>
<style scoped>
@import url("https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700&display=swap");
* {
  margin: 0;
  padding: 0;
  box-sizing: border-box;
  width: 100%;
}
body {
  font-family: "Inter", sans-serif;
}
.formbold-main-wrapper {
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 48px;
}

.formbold-form-wrapper {
  margin: 0 auto;
  max-width: 550px;
  width: 100%;
  background: white;
}

.formbold-event-wrapper span {
  font-weight: 500;
  font-size: 16px;
  line-height: 24px;
  letter-spacing: 2.5px;
  color: #6a64f1;
  display: inline-block;
  margin-bottom: 12px;
}
.formbold-event-wrapper h3 {
  font-weight: 700;
  font-size: 28px;
  line-height: 34px;
  color: #07074d;
  width: 60%;
  margin-bottom: 15px;
}
.formbold-event-wrapper h4 {
  font-weight: 600;
  font-size: 20px;
  line-height: 24px;
  color: #07074d;
  width: 60%;
  margin: 25px 0 15px;
}
.formbold-event-wrapper p {
  font-size: 16px;
  line-height: 24px;
  color: #536387;
}

.formbold-event-details {
  background: #fafafa;
  border: 1px solid #dde3ec;
  border-radius: 5px;
  margin: 25px 0 30px;
}
.formbold-event-details h5 {
  color: #07074d;
  font-weight: 600;
  font-size: 18px;
  line-height: 24px;
  padding: 15px 25px;
}
.formbold-event-details ul {
  border-top: 1px solid #edeef2;
  padding: 25px;
  margin: 0;
  list-style: none;
  display: flex;
  flex-wrap: wrap;
  row-gap: 14px;
}
.formbold-event-details ul li {
  color: #536387;
  font-size: 16px;
  line-height: 24px;
  width: 50%;
  display: flex;
  align-items: center;
  gap: 10px;
}

.formbold-form-title {
  color: #07074d;
  font-weight: 600;
  font-size: 28px;
  line-height: 35px;
  width: 60%;
  margin-bottom: 30px;
}

.formbold-input-flex {
  display: flex;
  gap: 20px;
  margin-bottom: 15px;
}
.formbold-input-flex > div {
  width: 50%;
}
.formbold-form-input {
  text-align: center;
  width: 100%;
  padding: 13px 22px;
  border-radius: 5px;
  border: 1px solid #dde3ec;
  background: #ffffff;
  font-weight: 500;
  font-size: 16px;
  color: #536387;
  outline: none;
  resize: none;
}
.formbold-form-input:focus {
  border-color: #6a64f1;
  box-shadow: 0px 3px 8px rgba(0, 0, 0, 0.05);
}
.formbold-form-label {
  color: #536387;
  font-size: 14px;
  line-height: 24px;
  display: block;
  margin-bottom: 10px;
}
.formbold-policy {
  font-size: 14px;
  line-height: 24px;
  color: #536387;
  width: 70%;
  margin-top: 22px;
}
.formbold-policy a {
  color: #6a64f1;
}
.formbold-btn {
  text-align: center;
  width: 100%;
  font-size: 16px;
  border-radius: 5px;
  padding: 14px 25px;
  border: none;
  font-weight: 500;
  background-color: #6a64f1;
  color: white;
  cursor: pointer;
  margin-top: 25px;
}
.formbold-btn:hover {
  box-shadow: 0px 3px 8px rgba(0, 0, 0, 0.05);
}
</style>

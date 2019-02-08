<template>
  <div id="app">
    <section class="section">
      <navbar></navbar>
      <div class="content">
        <router-view></router-view>

      </div>
    </section>
  </div>
</template>

<script>
import Navbar from './Navbar.vue';

export default {
  components: {
    'navbar': Navbar
  },
  data() {
    return {
    };
  }
};
</script>

<style lang="scss">
@import "./src/css/custom.scss";
@import "./node_modules/bootstrap/scss/bootstrap.scss";

// .content {
//   margin-top: 57px; // matches custom.scss @navbar-height
// }
</style>

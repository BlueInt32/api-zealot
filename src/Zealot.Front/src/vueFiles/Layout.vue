<template>
  <div id="app">
    <section class="section">
      <navbar></navbar>
      <div class="content">
        <router-view></router-view>
      </div>
      <project-edition-modal></project-edition-modal>
      <project-load-modal></project-load-modal>
    </section>
  </div>
</template>

<script>
import Navbar from './Navbar.vue';
import ProjectEditionModal from './ProjectEdition.Modal.vue';
import ProjectLoadModal from './ProjectLoad.Modal.vue';

export default {
  components: {
    'navbar': Navbar,
    'project-edition-modal': ProjectEditionModal,
    'project-load-modal': ProjectLoadModal
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

.content {
  margin: 65px 1em;
  height: 100%;
}
// .content {
//   margin-top: 57px; // matches custom.scss @navbar-height
// }
</style>

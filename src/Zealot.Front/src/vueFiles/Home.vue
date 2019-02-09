<template>
<div class="homeContainer">
  <div class="row">
    <div class="col-2"></div>
    <div class="col-10">
      <div class="request-panel">
        <div class="row mb-2">
          <request-url-bar class="col-12"></request-url-bar>
        </div>
        <div class="row">
          <request-panel class="col-6"></request-panel>
          <response-panel class="col-6"></response-panel>
        </div>
      </div>
    </div>
  </div>
</div>
</template>

<script>
import RequestUrlBar from './RequestUrlBar.vue';
import RequestPanel from './RequestPanel.vue';
import ResponsePanel from './ResponsePanel.vue';

export default {
  components: {
    RequestUrlBar,
    RequestPanel,
    ResponsePanel
  },
  filters: {
  },
  data() {
    return {
    };
  },
  computed: {
  },
  created() {
  },
  mounted() {
  },
  methods: {
  },
  watch: {
    '$route': function (to) {
      if (to.path === '/random') {
        console.log('random');
      }
    }
  }
};
</script>

<style lang="scss">
body {
  margin: 0;
}
.homeContainer {
  margin: 65px 1em;
  height: 100%;
}
#text-img {
  width: 70px;
}
</style>

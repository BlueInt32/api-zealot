<template>
  <div class="request-panel">
    <div class="row mb-2">
      <request-url-bar class="col-12"></request-url-bar>
    </div>
    <div class="row">
      <request-panel class="col-6"></request-panel>
      <response-panel class="col-6"></response-panel>
    </div>
  </div>
</template>

<script>
// import { mapGetters, mapActions } from "vuex";
import RequestUrlBar from './RequestUrlBar.vue';
import RequestPanel from './RequestPanel.vue';
import ResponsePanel from './ResponsePanel.vue';

export default {
  components: {
    RequestUrlBar,
    RequestPanel,
    ResponsePanel
  },
  computed: {
    // ...mapGetters('someModule', ['someGetter'])
  },
  created() {
  },
  data() {
    return {
    };
  },
  methods: {
    // ...mapActions('someModule', ['someAction'])
  },
  props: [
  ]
};
</script>

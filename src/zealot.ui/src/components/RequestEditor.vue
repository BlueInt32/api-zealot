<template>
  <div class="request-panel">
    <div class="row mb-2">
      <RequestUrlBar class="col-12"></RequestUrlBar>
    </div>
    <div class="row">
      <RequestPanel class="col-6"></RequestPanel>
      <ResponsePanel class="col-6"></ResponsePanel>
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
  created() {},
  data() {
    return {};
  },
  methods: {
    // ...mapActions('someModule', ['someAction'])
  },
  props: []
};
</script>

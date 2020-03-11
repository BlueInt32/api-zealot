<template>
  <div class="request-panel pure-g">
    <div class="pure-u-1-2">
      <RequestUrlBar></RequestUrlBar>
      <RequestPanel></RequestPanel>
    </div>
    <div class="pure-u-1-2">
      <ResponsePanel></ResponsePanel>
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

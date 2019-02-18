<template>
<div class="homeContainer">
  <div class="row">
    <div class="col-lg-3 col-sm-4">
      <input type="button" class="btn btn-primary" value="Save" @click="updateProject"/>
      <tree-view-container></tree-view-container>
    </div>
    <div class="col">
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
import { mapGetters, mapActions } from 'vuex';
import RequestUrlBar from './RequestUrlBar.vue';
import RequestPanel from './RequestPanel.vue';
import ResponsePanel from './ResponsePanel.vue';
import TreeViewContainer from './TreeViewContainer.vue';

export default {
  components: {
    TreeViewContainer,
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
    ...mapGetters('projectModule', ['projectId'])
  },
  created() {
    console.log('created');
    if (this.$route.params.projectId !== this.projectId) {
      this.getProjectDetails({ projectId: this.$route.params.projectId });
    }
  },
  mounted() {
    console.log('mounted');
  },
  methods: {
    ...mapActions('projectModule', ['getProjectDetails', 'updateProject'])
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
#text-img {
  width: 70px;
}
</style>

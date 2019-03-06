<template>
<div class="homeContainer">
  <div class="row">
    <div class="col-lg-3 col-sm-4">
      <input type="button" class="btn btn-primary" value="Save" @click="updateProject"/>
      <tree-view-container></tree-view-container>
    </div>
    <div class="col">
      <code-editor v-if="selectedNodeType === 'code'"></code-editor>
      <pack-editor v-else-if="selectedNodeType === 'pack'"></pack-editor>
      <request-editor v-else></request-editor>
    </div>
  </div>
</div>
</template>

<script>
import { mapGetters, mapActions } from 'vuex';
import TreeViewContainer from './TreeViewContainer.vue';
import RequestEditor from './RequestEditor.vue';
import CodeEditor from './CodeEditor.vue';
import PackEditor from './PackEditor.vue';
import { log } from '../helpers/consoleHelpers';

export default {
  components: {
    CodeEditor,
    PackEditor,
    RequestEditor,
    TreeViewContainer
  },
  filters: {
  },
  data() {
    return {
    };
  },
  computed: {
    ...mapGetters('projectModule', ['projectId']),
    ...mapGetters('treeModule', ['selectedNodeId', 'selectedNodeType']),
  },
  created() {
  },
  mounted() {
    if (this.$route.params.projectId !== this.projectId) {
      this.getProjectDetails({ projectId: this.$route.params.projectId });
    }
  },
  methods: {
    ...mapActions('projectModule', ['getProjectDetails', 'updateProject'])
  },
  watch: {
    '$route': function (to) {
      if (to.path === '/random') {
        log('random');
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

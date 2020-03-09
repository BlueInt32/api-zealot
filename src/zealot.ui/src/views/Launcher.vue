<template>
  <div class="launcher" v-if="currentProject">
    <h1>{{ currentProject.name }}</h1>
    <div id="launcher__grid" class="pure-g">
      <div class="pure-u-1-5">
        <TreeViewContainer />
      </div>
      <div class="pure-u-4-5">
        <RequestEditor></RequestEditor>
        <!-- <code-editor v-if="selectedNodeType === 'code'"></code-editor>
        <pack-editor v-else-if="selectedNodeType === 'pack'"></pack-editor>
        <request-editor v-else></request-editor> -->
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue, Watch } from 'vue-property-decorator';
import { getModule } from 'vuex-module-decorators';
import ProjectEditionModule from '@/store/ProjectEditionModule';
import Project from '@/domain/Project';
import TreeViewContainer from '@/components/TreeViewContainer.vue';
import RequestEditor from '@/components/RequestEditor.vue';

@Component({ components: { TreeViewContainer, RequestEditor } })
export default class Launcher extends Vue {
  private projectEditionModule = getModule(ProjectEditionModule);
  private currentProject: Project | null = null;

  private async created() {
    if (this.$route.params.projectId) {
      await this.projectEditionModule.getProjectDetails(
        this.$route.params.projectId
      );
      this.currentProject = this.projectEditionModule.currentProject;
    }
  }
}
</script>

<style lang="scss" scoped></style>

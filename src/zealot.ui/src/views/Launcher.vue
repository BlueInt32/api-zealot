<template>
  <div class="launcher">
    <h1>Launcher</h1>
    <div v-if="currentProject">
      {{ currentProject.id }} - {{ currentProject.name }}
    </div>
    <div id="launcher__grid" class="pure-g">
      <div class="pure-u-1-3">
        <TreeViewContainer />
      </div>
      <div class="pure-u-2-3">
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

@Component({ components: { TreeViewContainer } })
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

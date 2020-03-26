<template>
  <div class="launcher" v-if="currentProject">
    <h1>{{ currentProject.name }}</h1>
    <div id="launcher__grid" class="pure-g">
      <div class="pure-u-1-5">
        <button class="pure-button" @click="saveAllClickHandler">
          Save all
        </button>
        <TreeViewContainer />
      </div>
      <div class="pure-u-4-5">
        <RequestEditor
          v-if="treeModule.selectedNodeType === requestNodeType"
        ></RequestEditor>
        <ScriptEditor
          v-if="treeModule.selectedNodeType === scriptNodeType"
        ></ScriptEditor>
        <!-- <code-editor v-if="selectedNodeType === 'code'"></code-editor>
        <pack-editor v-else-if="selectedNodeType === 'pack'"></pack-editor>
        <request-editor v-else></request-editor>-->
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
import ScriptEditor from '@/components/ScriptEditor.vue';
import TreeModule from '@/store/TreeModule';
import { NodeType } from '@/domain/NodeType';

@Component({ components: { TreeViewContainer, RequestEditor, ScriptEditor } })
export default class Launcher extends Vue {
  private projectEditionModule = getModule(ProjectEditionModule);
  private treeModule = getModule(TreeModule);
  private currentProject: Project | null = null;
  private requestNodeType = NodeType.Request;
  private scriptNodeType = NodeType.Script;

  private async created() {
    if (this.$route.params.projectId) {
      await this.projectEditionModule.getProjectDetails(
        this.$route.params.projectId
      );
      this.currentProject = this.projectEditionModule.currentProject;
    }
  }

  saveAllClickHandler() {
    this.projectEditionModule.updateProject();
  }
}
</script>

<style lang="scss" scoped></style>

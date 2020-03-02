<template>
  <div class="row">
    <h1>Hey</h1>
    <div class="col-4">
      <ul class="list-group list-group-flush">
        <li
          v-for="projectConfig in projectsConfigs"
          :key="projectConfig.id"
          class="list-group-item projectsList__item"
          @click="projectClickHandler(projectConfig.id)"
        >
          <p :title="projectConfig.id">
            <i class="fas fa-folder-open"></i>
            {{ projectConfig.path }}
          </p>
        </li>
      </ul>
    </div>
    <div class="col-8"></div>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue, Watch } from 'vue-property-decorator';
import ProjectsListModule from '@/store/projectsListModule';
import { getModule } from 'vuex-module-decorators';
import { log } from '../helpers/consoleHelpers';
import ProjectConfig from '@/domain/Project';

@Component({})
export default class ProjectsList extends Vue {
  private projectsListModule = getModule(ProjectsListModule);
  private projectsConfigs: ProjectConfig[] = [];

  private mounted() {
    console.log('hey');
  }

  private async created() {
    this.projectsConfigs = await this.projectsListModule.getProjectsConfigsList();
  }
  projectClickHandler(projectId: string) {
    this.$router.push({ name: 'projectView', params: { projectId } });
  }
}
</script>

<style lang="scss" scoped></style>

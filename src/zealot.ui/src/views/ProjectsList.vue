<template>
  <div class="projectsList">
    <h1>Projects</h1>
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
            <span class="projectsList__projectTitle">{{
              projectConfig.name
            }}</span>
            - {{ projectConfig.path }}
          </p>
        </li>
      </ul>
    </div>
    <div class="col-8"></div>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue, Watch } from 'vue-property-decorator';
import { getModule } from 'vuex-module-decorators';
import ProjectsListModule from '@/store/ProjectsListModule';
import { log } from '../helpers/consoleHelpers';
import ProjectConfig from '@/domain/ProjectConfig';

@Component({})
export default class ProjectsList extends Vue {
  private projectsListModule = getModule(ProjectsListModule);
  private projectsConfigs: ProjectConfig[] = [];

  private mounted() {}

  private async created() {
    this.projectsConfigs = await this.projectsListModule.getProjectsConfigsList();
  }
  projectClickHandler(projectId: string) {
    this.$router.push({ name: 'launcher', params: { projectId } });
  }
}
</script>

<style lang="scss" scoped>
.projectsList__item {
  cursor: pointer;
}
.projectsList__projectTitle {
  font-size: 1.2em;
}
</style>

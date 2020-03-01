<template>
  <div class="row">
    <div class="col-4">
      <ul class="list-group list-group-flush">
        <li
          v-for="project in projects"
          :key="project.id"
          class="list-group-item projectsList__item"
          @click="projectClickHandler(project.id)">
          <p :title="project.id">
            <i class="fas fa-folder-open"></i>
            {{project.path}}
          </p>
        </li>
      </ul>
    </div>
    <div class="col-8"></div>
  </div>
</template>

<script>
import { mapGetters, mapActions } from 'vuex';
import { log } from '../helpers/consoleHelpers';

export default {
  components: {
  },
  computed: {
    ...mapGetters('projectsListModule', ['projects'])
  },
  created() {
    this.getProjectsList();
  },
  methods: {
    ...mapActions('projectsListModule', ['getProjectsList']),
    ...mapActions('projectModule', ['getProjectDetails']),
    projectClickHandler(projectId) {
      this.getProjectDetails({ projectId })
        .then(() => {
          log('going to route projectView');
          this.$router.push({ name: 'projectView', params: { projectId } });
        });
    }
  },
  props: [
  ]
};
</script>

<style lang="scss" scoped>
</style>

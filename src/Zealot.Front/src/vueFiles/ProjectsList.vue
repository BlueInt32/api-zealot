<template>
  <div class="row">
    <div class="col-4">
      <ul class="list-group list-group-flush">
        <li
          v-for="project in projects"
          :key="project.id"
          class="list-group-item projectsList__item">

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
    ...mapActions('projectsListModule', ['getProjectsList'])
  },
  props: [
  ]
};
</script>

<style lang="scss" scoped>
.projectsList__item {
}
</style>

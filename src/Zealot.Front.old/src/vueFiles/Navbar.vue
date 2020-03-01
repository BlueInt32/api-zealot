<template>
<b-navbar fixed="top" toggleable="md" type="primary" variant="dark">

  <b-navbar-toggle target="nav_collapse"></b-navbar-toggle>

  <b-navbar-brand :to="'/'">Api Zealot</b-navbar-brand>

  <b-collapse is-nav id="nav_collapse">

    <b-navbar-nav>

      <b-nav-item v-b-modal.projectEditionModal text-variant="green">
        <i class="fas fa-plus"></i> New project</b-nav-item>
      <b-nav-item :to="'/projects'" text-variant="green">
        <i class="fas fa-list-ul"></i> Projects</b-nav-item>
      <!-- <b-nav-item v-b-modal.projectLoadModal text-variant="green">
         Open project</b-nav-item> -->
    </b-navbar-nav>
  </b-collapse>
</b-navbar>
</template>

<script>
import { mapActions, mapGetters } from 'vuex';

export default {
  components: {
  },
  computed: {
    ...mapGetters('stuffModule', [])
  },
  data() {
    return {
    };
  },
  created() {
  },
  methods: {
    ...mapActions('stuffModule', []),
    toggleTag(tagLabel) {
      if (this.isActive(tagLabel)) {
        this.deactivateTag(tagLabel).then(() => {
          this.loadPosts();
        });
      } else {
        this.activateTag(tagLabel).then(() => {
          this.loadPosts();
        });
      }
    },
    isActive(tagLabel) {
      return this.activeTags.includes(tagLabel);
    },
    retriggerRandom() {
      this.setSortingProperty('random');
      this.resetLoadPosts();
      this.loadPosts();
    }
  },
  props: [
  ]
};
</script>

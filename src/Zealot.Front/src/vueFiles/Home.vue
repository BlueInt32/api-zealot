<template>
<div class="homeContainer">
  <b-input-group>
    <b-dropdown variant="outline-secondary" slot="prepend">
      <template slot="button-content">
        {{selectedVerb}}<span class="sr-only"></span>
      </template>
      <b-dropdown-item id="GET" @click="selectVerbHandler('GET')">GET</b-dropdown-item>
      <b-dropdown-item id="POST" @click="selectVerbHandler('POST')">POST</b-dropdown-item>
    </b-dropdown>
    <b-form-input></b-form-input>
    <b-input-group-append >
      <b-btn variant="success">SEND</b-btn>
    </b-input-group-append>
  </b-input-group>
  </div>
</template>

<script>
import { mapActions, mapGetters } from 'vuex';
// import PostsList from './PostsList.vue';
// import PostEditionModal from './PostEdition.Modal.vue';

import image from '../img/vuejs-logo.png';

export default {
  components: {
    // 'PostsList': PostsList,
    // 'PostEditionModal': PostEditionModal
  },
  filters: {
  },
  data() {
    return {
      isGenerating: false,
      scrollTimeout: null,
      visible: true,
      logo: image,
      selectedVerb: 'GET'
    };
  },
  computed: {
    ...mapGetters('stuffModule', ['text'])
  },
  created() {
  },
  mounted() {
  },
  methods: {
    ...mapActions('stuffModule', []),
    selectVerbHandler(verbId) {
      this.selectedVerb = verbId;
    }
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
.homeContainer {
  margin: 65px 1em;
  height: 100%;
}
#text-img {
  width: 70px;
}
</style>

<template>
<div class="homeContainer">
  <img id="text-img" v-bind:src="logo" />
  <p>Welcome to the boiler plate vueJS ! <br/>
  It has <u>vueJs, vuex, webpack, eslint, bootstrap, scss, axios, modals, icons and routing</u> supported out of the box !</p>
  <h3><i class="fas fa-dog"></i></h3>
  <p>For dev : <br/>
    - run 'npm start'<br/>
  </p>
  <p>For production : <br/>
    - have a look at <i>build/webpack.client.config.js</i> and replace the 'serverAppName' value<br/>
    (this will use templated <i>index.prod.html</i> file to set the <b>&lt;base href&gt;</b> tag)<br/>
    - if using an api, also replace 'apiHost'
    - run 'npm run build'<br/>
  </p>
  <p><strong>{{text}}</strong></p>
  <posts-list></posts-list>
  <post-edition-modal></post-edition-modal>
</div>
</template>

<script>
import { mapActions, mapGetters } from 'vuex';
import PostsList from './PostsList.vue';
import PostEditionModal from './PostEdition.Modal.vue';

import image from '../img/vuejs-logo.png';

export default {
  components: {
    'PostsList': PostsList,
    'PostEditionModal': PostEditionModal
  },
  filters: {
  },
  data() {
    return {
      isGenerating: false,
      scrollTimeout: null,
      visible: true,
      logo: image
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
    ...mapActions('stuffModule', [])
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

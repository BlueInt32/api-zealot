<template>
<div class="homeContainer">
  <b-input-group>
    <b-dropdown variant="outline-secondary" slot="prepend">
      <template slot="button-content">
        {{selectedVerb}}<span class="sr-only"></span>
      </template>
      <b-dropdown-item v-for="verb in httpVerbs" :key="verb" :id="verb"
        @click="selectVerbHandler(verb)">{{verb}}</b-dropdown-item>
    </b-dropdown>
    <b-form-input v-model="url"></b-form-input>
    <b-input-group-append >
      <b-btn variant="success" @click="sendWiz">SEND</b-btn>
    </b-input-group-append>
  </b-input-group>
  <vue-json-pretty
      :data="result">
  </vue-json-pretty>
  </div>
</template>

<script>
import { mapActions, mapGetters } from 'vuex';
import VueJsonPretty from 'vue-json-pretty';
import appService from '../app.service';

export default {
  components: {
    VueJsonPretty
  },
  filters: {
  },
  data() {
    return {
      selectedVerb: 'GET',
      result: '',
      url: 'http://localhost:9500'
    };
  },
  computed: {
    ...mapGetters('configModule', ['httpVerbs'])
  },
  created() {
  },
  mounted() {
  },
  methods: {
    ...mapActions('stuffModule', []),
    selectVerbHandler(verbId) {
      this.selectedVerb = verbId;
    },
    sendWiz() {
      appService.sendWiz({
        verb: this.selectedVerb,
        endpointUrl: this.url,
        body: '{}'
      }).then((data) => {
        this.result = data;
      });
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

<template>
<div class="homeContainer">
  <b-input-group>
    <b-dropdown variant="outline-secondary" slot="prepend">
      <template slot="button-content">
        {{selectedHttpVerb}}<span class="sr-only"></span>
      </template>
      <b-dropdown-item v-for="httpVerb in httpVerbs" :key="httpVerb" :id="httpVerb"
        @click="selectHttpVerb({httpVerb})">{{httpVerb}}</b-dropdown-item>
    </b-dropdown>
    <b-form-input v-model="localRequestUrl"></b-form-input>
    <b-input-group-append >
      <b-btn variant="success" @click="sendWiz">SEND</b-btn>
    </b-input-group-append>
  </b-input-group>
  <vue-json-pretty :data="requestResult"></vue-json-pretty>
  </div>
</template>

<script>
import { mapActions, mapGetters } from 'vuex';
import VueJsonPretty from 'vue-json-pretty';

export default {
  components: {
    VueJsonPretty
  },
  filters: {
  },
  data() {
    return {
    };
  },
  computed: {
    ...mapGetters('configModule', ['httpVerbs']),
    ...mapGetters('wizModule', ['selectedHttpVerb',
      'requestResult',
      'requestUrl']),
    localRequestUrl: {
      get() {
        return this.$store.state.wizModule.requestUrl;
      },
      set(value) {
        this.$store.dispatch('wizModule/setRequestUrl', { requestUrl: value }, { root: true });
      }
    }
  },
  created() {
    this.localRequestUrl = 'http://localhost:9500';
  },
  mounted() {
  },
  methods: {
    ...mapActions('wizModule', ['selectHttpVerb',
      'sendWiz',
      'setRequestUrl']),
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

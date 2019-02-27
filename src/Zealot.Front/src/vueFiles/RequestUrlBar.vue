<template>
  <b-input-group>
    <b-dropdown variant="outline-secondary" slot="prepend">
      <template slot="button-content">
        {{selectedHttpMethod}}<span class="sr-only"></span>
      </template>
      <b-dropdown-item v-for="httpMethod in httpMethods" :key="httpMethod" :id="httpMethod"
        @click="selectHttpMethod({httpMethod})">{{httpMethod}}</b-dropdown-item>
    </b-dropdown>
    <b-input-group-prepend>
      <b-btn variant="success" @click="sendRequest"><i class="fas fa-play"></i></b-btn>
    </b-input-group-prepend>
    <b-form-input v-model="localRequestUrl"></b-form-input>
  </b-input-group>
</template>

<script>
import { mapActions, mapGetters } from 'vuex';
import { log } from '../helpers/consoleHelpers';

export default {
  components: {
  },
  filters: {
  },
  data() {
    return {
    };
  },
  computed: {
    ...mapGetters('configModule', ['httpMethods']),
    ...mapGetters('requestModule', ['selectedHttpMethod',
      'requestResult',
      'requestUrl']),
    localRequestUrl: {
      get() {
        return this.$store.state.requestModule.requestUrl;
      },
      set(value) {
        this.$store.dispatch('requestModule/setRequestUrl', { requestUrl: value }, { root: true });
      }
    }
  },
  created() {
    this.localRequestUrl = 'http://localhost:9500';
  },
  mounted() {
  },
  methods: {
    ...mapActions('requestModule', ['selectHttpMethod',
      'sendRequest',
      'setRequestUrl']),
  },
  watch: {
    '$route': function (to) {
      if (to.path === '/random') {
        log('random');
      }
    }
  }
};
</script>
<style lang="scss" scoped>
.btn-success:focus {
  box-shadow: none;
  //inset 0px 2px 7px 0.2rem rgba(76, 150, 93, 0.5);
}
</style>

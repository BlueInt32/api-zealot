<template>
  <div>
    <b-nav pills>
      <b-nav-item>Raw</b-nav-item>
      <b-nav-item>Preview</b-nav-item>
    </b-nav>
    <vue-json-pretty :data="requestResult"></vue-json-pretty>
  </div>
</template>
<script>
import VueJsonPretty from 'vue-json-pretty';
import { mapGetters } from 'vuex';

export default {
  components: {
    VueJsonPretty
  },
  computed: {
    ...mapGetters('wizModule', ['requestResult'])
  },
  created() {
  },
  data() {
    return {
    };
  },
  methods: {
    // ...mapActions('someModule', ['someAction'])
  },
  props: [
  ]
};
</script>

<template>
  <div>
    <ul class="nav nav-tabs ml-2">
      <li role="presentation" class="nav-item active">
        <a class="nav-link" href="#">Raw</a>
      </li>
      <li role="presentation" class="nav-item">
        <a class="nav-link" href="#">Preview</a>
      </li>
    </ul>
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
    ...mapGetters('requestModule', ['requestResult'])
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

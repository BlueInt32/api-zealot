<template>
  <div class="code-editor">
    <h1>Code editor</h1>
    <button class="btn btn-light" @click="runCode">Run</button>
    <b-textarea rows="15" v-model="code" class="code-editor__maintextarea"></b-textarea>
  </div>
</template>

<script>
// import { mapGetters, mapActions } from "vuex";
// import compiler from 'expression-sandbox'; //eslint-disable-line
import { getPackContext } from '../services/packContextService';

const compiler = require('expression-sandbox');

export default {
  components: {
  },
  computed: {
    // ...mapGetters('someModule', ['someGetter'])
  },
  created() {
  },
  data() {
    return {
      code: 'bag.pioupiou = context.lastResult.version.number;'
    };
  },
  methods: {
    // ...mapActions('someModule', ['someAction'])
    runCode() {
      const codeRunner = compiler(this.code);

      const packContextA = getPackContext('a');
      const stuff = { pioupiou: '' };
      const result = codeRunner({ context: packContextA, bag: stuff });
      console.log(result);
      console.log(stuff);
    }
  },
  props: [
  ]
};
</script>

<style lang="scss" scoped>
.code-editor__maintextarea {
  font-family: monospace;
}
</style>

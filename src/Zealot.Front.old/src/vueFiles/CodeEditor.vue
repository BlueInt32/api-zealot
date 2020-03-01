<template>
  <div class="code-editor">
    <h1>Code editor</h1>
    <button class="btn btn-light" @click="runCode">Run</button>
    <b-textarea rows="15" v-model="code" class="code-editor__maintextarea"></b-textarea>
  </div>
</template>

<script>
// import { mapGetters, mapActions } from "vuex";
import Worker from "../helpers/file.worker.js"; // eslint-disable-line

import { getPackContext } from '../services/packContextService';

export default {
  components: {
  },
  computed: {
    // ...mapGetters('someModule', ['someGetter'])
  },
  created() {
    this.worker = new Worker();
  },
  data() {
    return {
      code: 'context.stuff = 1;',
      worker: null,
      context: {}
    };
  },
  methods: {
    // ...mapActions('someModule', ['someAction'])
    runCode() {
      this.context = getPackContext('a');
      const launchWorker = () => {
        this.worker.postMessage({ code: this.code, context: getPackContext('a') });
        this.worker.onmessage = (data) => {
          const message = data;
          console.log('Host received: ', message.data.context);
          if (message.type === 'done!') {
            this.worker.terminate();
          }
        };
      };
      launchWorker();
      console.log('Host: posting "start" with context', this.context);
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

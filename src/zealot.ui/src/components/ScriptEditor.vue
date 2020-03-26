<template>
  <div class="request-panel pure-g">
    <div class="pure-u-1">
      <textarea
        :value="scriptModule.currentNode.code"
        @input="updateCodeHandler"
      ></textarea>
    </div>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue, Watch } from 'vue-property-decorator';
import { getModule } from 'vuex-module-decorators';
import ScriptModule from '@/store/ScriptModule';
// import { mapGetters, mapActions } from "vuex";
@Component({})
export default class ScriptEditor extends Vue {
  private scriptModule = getModule(ScriptModule);
  created() {}
  updateCodeHandler(event: any) {
    this.scriptModule.setCode(event.target.value);
  }
}
</script>

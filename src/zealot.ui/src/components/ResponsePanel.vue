<template>
  <div>
    <button class="pure-button">Raw</button>
    <button class="pure-button">Preview</button>
    <vue-json-pretty
      :data="requestModule.requestResult"
      :showLength="false"
      :showDoubleQuote="true"
      :highlightMouseoverNode="false"
      :showLine="false"
    ></vue-json-pretty>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue, Watch } from 'vue-property-decorator';
import { getModule } from 'vuex-module-decorators';
import VueJsonPretty from 'vue-json-pretty';

import RequestModule from '@/store/RequestModule';

@Component({ components: { VueJsonPretty } })
export default class ResponsePanel extends Vue {
  private requestModule = getModule(RequestModule);
}
</script>

<style lang="scss" scoped></style>

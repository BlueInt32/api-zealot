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
    <vue-json-pretty :data="requestModule.requestResult"></vue-json-pretty>
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

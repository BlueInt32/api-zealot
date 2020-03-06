<template>
  <div class="RequestUrlBar">
    {{ requestModule.httpMethod }}
    <font-awesome-icon class="typePrompt__icon" icon="play"></font-awesome-icon>
    <input
      class="form-control"
      type="text"
      :value="requestModule.endpointUrl"
      @input="requestModule.setEndpointUrl($event.target.value)"
    />
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue, Watch } from 'vue-property-decorator';
import { getModule } from 'vuex-module-decorators';
import RequestModule from '@/store/RequestModule';

@Component({})
export default class RequestUrlBar extends Vue {
  private requestModule = getModule(RequestModule);
}
</script>

<style lang="scss" scoped></style>

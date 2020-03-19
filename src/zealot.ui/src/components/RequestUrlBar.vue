<template>
  <form class="RequestUrlBar pure-form pure-g">
    <div class="pure-u-1-8">
      <select
        name="httpMethod"
        id="httpMethod"
        class="pure-input-1"
        :value="requestModule.httpMethod"
        @input="updateHttpMethodHandler"
      >
        <option v-for="method in httpMethodsAvailable" :key="method">{{ method }}</option>
      </select>
    </div>
    <div class="pure-u-7-8">
      <input
        class="pure-input-1"
        type="text"
        :value="requestModule.endpointUrl"
        @input="endpointUrlChanged"
        placeholder="Endpoint url"
      />
    </div>
    <div class="pure-u-1">
      <button
        type="button"
        class="pure-button pure-input-1 pure-button-primary"
        @click="clickSendHandler"
      >
        <font-awesome-icon class="typePrompt__icon" icon="play"></font-awesome-icon>Send request
      </button>
    </div>
  </form>
</template>

<script lang="ts">
import { Component, Prop, Vue, Watch } from 'vue-property-decorator';
import { getModule } from 'vuex-module-decorators';
import RequestModule from '@/store/RequestModule';
import { HttpMethodEnum } from '@/domain/HttpMethodEnum';
import { SendRequestParams } from '../domain/apiParams/SendRequestParams';

@Component({})
export default class RequestUrlBar extends Vue {
  private requestModule = getModule(RequestModule);
  private httpMethodsAvailable = Object.keys(HttpMethodEnum);
  // private selectedHttpMethod: string = '';
  // private endpointUrl!: string;

  created() {
    console.log(this.requestModule.endpointUrl);
  }
  updateHttpMethodHandler(event: any) {
    let strMethod = this.httpMethodsAvailable[event.target.selectedIndex];
    let enumValue = strMethod as keyof typeof HttpMethodEnum;
    this.requestModule.setHttpMethod(enumValue as HttpMethodEnum);
  }

  endpointUrlChanged(event: any) {
    this.requestModule.setEndpointUrl(event.target.value);
  }

  clickSendHandler() {
    this.requestModule.sendRequest();
  }
}
</script>

<style lang="scss" scoped></style>

<template>
  <div id="app">
    <Menu></Menu>
    <router-view />
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue, Watch } from 'vue-property-decorator';
import { getModule } from 'vuex-module-decorators';
import Menu from '@/components/Menu.vue';

// font-awesome dependencies
import { library } from '@fortawesome/fontawesome-svg-core';
import {
  faCaretRight,
  faGlobeAmericas,
  faPlay
} from '@fortawesome/free-solid-svg-icons';
import { faJsSquare } from '@fortawesome/free-brands-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome';
library.add(faCaretRight, faJsSquare, faGlobeAmericas, faPlay);
Vue.component('font-awesome-icon', FontAwesomeIcon);

Vue.filter('capitalize', function(value: string) {
  if (!value) return '';
  value = value.toString();
  return value.charAt(0).toUpperCase() + value.slice(1);
});

@Component({
  components: {
    Menu
  }
})
export default class App extends Vue {
  private menuLinkOpen: boolean = false;
}
</script>

<style lang="scss">
@import './styles/custom.scss';
@import 'purecss';
@import '~vue-snotify/styles/simple';
</style>

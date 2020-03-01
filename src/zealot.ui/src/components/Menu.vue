<template>
  <div id="menu" class="pure-u" :class="{ active: isMenuOpen }">
    <div class="pure-menu">
      <router-link class="pure-menu-heading" to="/">Shelter</router-link>

      <ul class="pure-menu-list">
        <li
          class="pure-menu-item"
          :class="computeMenuItemSelectedClass(['browse', 'viewElement'])"
        >
          <router-link to="/browse" class="pure-menu-link">Browse</router-link>
        </li>
        <li
          class="pure-menu-item"
          :class="
            computeMenuItemSelectedClass(['addElementStep1', 'addElementStep2'])
          "
        >
          <router-link to="/add/step1" class="pure-menu-link">Add</router-link>
        </li>
        <li
          class="pure-menu-item"
          :class="computeMenuItemSelectedClass(['about'])"
        >
          <router-link to="/about" class="pure-menu-link">About</router-link>
        </li>
        <li class="pure-menu-item">
          <a href="#" class="pure-menu-link">Manage labels</a>
        </li>
      </ul>
    </div>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue, Watch } from 'vue-property-decorator';
@Component({})
export default class Menu extends Vue {
  computeMenuItemSelectedClass(routeNames: string[]) {
    return routeNames.some(r => this.$route.name === r)
      ? { 'pure-menu-selected': true }
      : {};
  }
}
</script>

<style></style>

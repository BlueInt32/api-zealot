<template>
  <MovableNode :model="treeModule.tree" :depth="0"></MovableNode>
</template>

<script lang="ts">
import { Component, Prop, Vue, Watch } from 'vue-property-decorator';
import { getModule } from 'vuex-module-decorators';
import MovableNode from '@/components/MovableNode.vue';
import TreeModule from '@/store/TreeModule';

@Component({ components: { MovableNode } })
export default class TreeViewContainer extends Vue {
  private treeModule = getModule(TreeModule);
}
</script>

<style lang="scss" scoped>
#tree-view__container {
  font-family: Menlo, Consolas, monospace;
  color: #444;
}
.item {
  cursor: pointer;
}
.bold {
  font-weight: bold;
}
ul {
  padding-left: 1em;
  line-height: 1.5em;
  list-style-type: dot;
}
</style>

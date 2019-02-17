<template>
    <drag-node :model='tree' :depth="0"></drag-node>
</template>

<script>
import { mapGetters } from 'vuex';
import DragNode from './DragNode.vue';

export default {
  components: {
    DragNode
  },
  computed: {
    ...mapGetters('treeModule', ['tree'])
  },
  methods: {
  }
};
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

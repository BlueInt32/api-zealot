<template>
  <ul id="tree-view__container">
    <tree-view-item
      class="item"
      :model="treeData">
    </tree-view-item>
  </ul>
</template>

<script>
import TreeViewItem from './TreeViewItem.vue';

export default {
  components: {
    TreeViewItem
  },
  data: function () {
    return {
      treeData: {
        name: 'My Tree',
        children: [
          { name: 'hello' },
          { name: 'wat' },
          {
            name: 'child folder',
            children: [
              {
                name: 'child folder',
                children: [{ name: 'hello' }, { name: 'wat' }]
              },
              { name: 'hello' },
              { name: 'wat' },
              {
                name: 'child folder',
                children: [{ name: 'hello' }, { name: 'wat' }]
              }
            ]
          }
        ]
      },
    };
  },
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

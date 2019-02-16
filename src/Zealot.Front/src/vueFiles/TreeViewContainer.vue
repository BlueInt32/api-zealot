<template>
    <vue-drag-tree :data='[tree]'></vue-drag-tree>
</template>

<script>
import { mapGetters } from 'vuex';
import VueDragTree from './DragTree/VueDragTree.vue';

export default {
  components: {
    VueDragTree
  },
  computed: {
    ...mapGetters('projectModule', ['tree'])
  },
  data: function () {
    return {
      treeData: [{
          name: 'Node 0-0',
          id: 0,
          children: [
            {
              name: 'Node 1-1',
              id: 3,
              children: [{
                  name: 'Node 2-1',
                  id: 4,
                  children: []
                }, {
                  name: 'Node 2-2',
                  id: 10,
                  children: []
                }
              ]
            }, {
              name: 'Node 1-2',
              id: 13,
              children: []
            }
          ]
        }, {
          name: 'Node 0-1',
          id: 14,
          children: []
        }
      ]
    };
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

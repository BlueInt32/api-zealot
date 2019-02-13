<template>
  <div>
    <template v-for='(item,index) in newData'>
      <drag-node :model='item' :allowDrag='allowDrag' :allowDrop='allowDrop' :depth='increaseDepth' :defaultText='defaultText' :disableDBClick='disableDBClick' :key='index'></drag-node>
    </template>
  </div>
</template>

<script>
import DragNode from './DragNode.vue';

export default {
  name: 'VueDragTree',
  props: {
    data: Array,
    allowDrag: {
      type: Function,
      default: () => true
    },
    allowDrop: {
      type: Function,
      default: () => true
    },
    defaultText: {
      // 填加节点时显示的默认文本．
      type: String,
      default: 'New Node'
    },
    depth: {
      type: Number,
      default: 0
    },
    disableDBClick: {
      type: Boolean,
      default: false
    }
  },
  computed: {
    increaseDepth() {
      return this.depth + 1;
    },
    newData: {
      // getter
      get() {
        return this.data;
      },
      // setter
      set(newValue) {
        const { length } = this.data;
        for (let nodeId = 0; nodeId < length; nodeId += 1) {
          this.data.shift(nodeId);
        }
        this.data = Object.assign(this.data, newValue);
      }
    }
  },
  methods: {
    emitCurNodeClicked(model, component) {
      this.$emit('current-node-clicked', model, component);
    },
    emitDrag(model, component) {
      this.$emit('drag', model, component);
    },
    emitDragEnter(model, component) {
      this.$emit('drag-enter', model, component);
    },
    emitDragLeave(model, component) {
      this.$emit('drag-leave', model, component);
    },
    emitDragOver(model, component) {
      this.$emit('drag-over', model, component);
    },
    emitDragEnd(model, component) {
      this.$emit('drag-end', model, component);
    },
    emitDrop(model, component) {
      this.$emit('drop', model, component);
    }
  },
  components: {
    DragNode
  }
};
</script>

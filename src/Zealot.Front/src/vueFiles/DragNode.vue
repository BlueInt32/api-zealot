<template>
  <li class='node-container'
    :style='styleObj'
    :draggable='isDraggable'
    @drag.stop='drag'
    @dragover.stop='dragOver'
    @dragenter.stop='dragEnter'
    @dragleave.stop='dragLeave'
    @drop.stop='drop'
    @dragend.stop='dragEnd'
    :class='{ "is-selected": isSelected }'>
      <div
        class='treeNodeText'
        :style="{ 'padding-left': (this.depth - 1) * 12 + 'px' }"
        @click="toggle">
        <span
          :class="{ 'nodeClicked': (isFolder && open), 'vue-drag-node-icon':true }"
          v-if="isFolder"></span>

        <span class='text'>
          <i v-if="model.type === 'code'" class="fab fa-js-square"></i>
          <i class="fas fa-globe-americas" v-if="model.type === 'request'"></i>
          {{model.name}}</span>
      </div>
    <ul class='treeMargin' v-show="open" v-if="isFolder">
      <drag-node
        v-for="child in model.children"
        :depth='increaseDepth'
        :model="child"
        :key='child.id'
        :root-reference='rootReference' >
      </drag-node>
    </ul>
  </li>
</template>

<script>
import { mapGetters, mapActions } from 'vuex';

export default {
  name: 'DragNode',
  data() {
    return {
      open: false,
      styleObj: {
        opacity: 1
      }
    };
  },
  props: {
    rootReference: Object,
    model: Object,
    depth: {
      type: Number,
      default: 0
    }
  },
  computed: {
    ...mapGetters('treeModule', ['defaultNewNodeName',
      'currentlySelectedId',
      'draggedNode']),
    isSelected() {
      return this.id === this.currentlySelectedId;
    },
    isFolder() {
      return this.model.children && this.model.children.length;
    },
    increaseDepth() {
      return this.depth + 1;
    },
    isDraggable() {
      return this.allowDrag(this.model, this);
    }
  },
  methods: {
    ...mapActions('treeModule', [
      'allowDrag',
      'curNodeClicked',
      'dragHandler',
      'dragEnterHandler',
      'dragLeaveHandler',
      'dragOverHandler',
      'dragEndHandler',
      'dropHandler',
      'setDraggedNodeId',
      'dropOn'
    ]),
    select() {
      this.curNodeClicked({ model: this.model, component: this });
    },
    toggle() {
      if (this.isFolder) {
        this.open = !this.open;
      }
    },
    removeChild(childId) {
      const parentModelChildren = this.$parent.model.children;
      console.error('not implemented', childId, parentModelChildren);
      // for (let index in parentModelChildren) {
      //   if (parentModelChildren[index].id == childId) {
      //     parentModelChildren = parentModelChildren.splice(index, 1);
      //     break;
      //   }
      // }
    },
    drag() {
      // that = this;
      this.setDraggedNodeId(this.model.id);
      // this.dragHandler(this.model, this, event);
    },
    dragOver(event) {
      event.preventDefault();
      this.dragOverHandler(this.model, this, event);
      return true;
    },
    dragEnter() {
      if (this.model.id !== this.draggedNode._uid) { // eslint-disable-line no-underscore-dangle
        this.styleObj.opacity = 0.5;
      }
    },
    dragLeave(event) {
      this.styleObj.opacity = 1;
      this.dragLeaveHandler(this.model, this, event);
    },
    drop() {
      this.styleObj.opacity = 1;
      this.dropOn(this.model.id);
    },
    dragEnd(event) {
      this.styleObj.opacity = 1;
      this.dragEndHandler(this.model, this, event);
    }
  },
  created() {
  }
};
</script>

<style lang="scss">
.node-container {
  list-style: none;
  &:hover {
    cursor: pointer;
  }
}
.is-selected {
  background: rgba(30, 0, 0, 0.1);
}

.item {
  cursor: pointer;
}

.bold {
  font-weight: bold;
}

.text {
  font-size: 0.65em;
}

.treeNodeText {
  display: inline-block;
  height: 28px;
  box-sizing: border-box;
  width: 100%;
  font-size: 18px;
  color: #324057;
  align-items: center;
}

.changeTree {
  width: 16px;
  color: #324057;
}

.vue-drag-node-icon {
  display: inline-block;
  width: 0;
  height: 0;
  margin-left: 10px;
  margin-right: 8px;
  border-left: 4px solid grey;
  border-top: 4px solid transparent;
  border-bottom: 4px solid transparent;
  border-right: 0 solid yellow;
  transition: transform 0.1s ease-in-out;
}

.nodeClicked {
  transform: rotate(90deg);
}
</style>

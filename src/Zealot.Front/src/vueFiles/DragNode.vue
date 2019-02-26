<template>
  <li class='node-container'
    :style='styleObj'
    :class='{ "is-selected": isSelected }'>
      <div
        :draggable='true'
        @drag.stop='drag'
        @dragover.stop='dragOver'
        @drop.stop='drop'
        @dragend.stop='dragEnd'
        @dragenter.stop='dragEnter'
        @dragleave.stop='dragLeave'
        class='treeNodeText'
        :style="{ 'padding-left': (this.depth - 1) * 12 + 'px' }"
        @click="nodeClickHandler">
        <span
          :class="{ 'nodeClicked': (isFolder && open), 'vue-drag-node-icon':true }"
          v-if="isFolder"></span>

        <span class='text'
          @dragenter.stop='dragEnter'
          @dragleave.stop='dragLeave'>
          <i v-if="model.type === 'code'" class="fab fa-js-square"></i>
          <i class="fas fa-globe-americas" v-if="model.type === 'request'"></i>
          {{model.name}}</span>
      </div>
      <div
        :style=styleObj2
        v-if="isDragging && isFolder" class="nodeDropZone"
        @dragleave.stop='dragLeave'
        @dragenter.stop='dragEnter' >
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
        opacity: 1,
      },
      styleObj2: {
        background: 'rgba(0, 0, 255, 0)'
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
      'selectedNode',
      'draggedNode',
      'isDragging']),
    isSelected() {
      return this.model.id === this.selectedNode.id;
    },
    isFolder() {
      return this.model.children && this.model.children.length;
    },
    increaseDepth() {
      return this.depth + 1;
    }
  },
  methods: {
    ...mapActions('treeModule', [
      'allowDrag',
      'selectNode',
      'dragHandler',
      'dragEnterHandler',
      'dragLeaveHandler',
      'dragOverHandler',
      'dragEndHandler',
      'dropHandler',
      'setDraggedNodeId',
      'dropOn',
      'setIsDragging'
    ]),
    nodeClickHandler() {
      if (this.isFolder) {
        this.open = !this.open;
      }
      this.selectNode(this.model);
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
      if (!this.isDragging) {
        this.setDraggedNodeId(this.model.id);
        this.setIsDragging(true);
      }
    },
    dragOver(event) {
      event.preventDefault();
      this.dragOverHandler(this.model, this, event);
      return true;
    },
    dragEnter() {
      if (this.model.id !== this.draggedNode.id) {
        this.styleObj.opacity = 0.5;
        this.styleObj2.background = 'rgba(0, 0, 255, 0.1)';
      }
    },
    dragLeave(event) {
      this.styleObj.opacity = 1;
      this.dragLeaveHandler(this.model, this, event);
      this.styleObj2.background = 'rgba(0, 0, 255, 0)';
    },
    drop() {
      this.styleObj.opacity = 1;
      this.dropOn(this.model.id);
    },
    dragEnd(event) {
      this.styleObj.opacity = 1;
      this.dragEndHandler(this.model, this, event);
      this.setIsDragging(false);
    }
  },
  created() {
  }
};
</script>

<style lang="scss">
.node-container {
  list-style: none;
  position: relative;
  &:hover {
    cursor: pointer;
  }
}
.is-selected > div > span.text {
  font-weight: bold;
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

.nodeDropZone {
  position: absolute;
  top: 0px;
  height: 100%;
  width: 100%;
  pointer-events: none;
  z-index: 30;
}
</style>

<template>
  <li class='treeview__node-container'
    :style='styleObj'
    :class='{ "treeview__node-container--selected": isSelected }'>
    <div
      :draggable='true'
      @drag.stop='drag'
      @dragover.stop='dragOver'
      @drop.stop='drop'
      @dragend.stop='dragEnd'
      @dragenter.stop='dragEnter'
      @dragleave.stop='dragLeave'
      class='treeview__node-header'
      :style="{ 'padding-left': (this.depth - 1) * 12 + 'px' }">
      <!-- <span v-if="isFolder" class="vue-drag-node-icon"
        :class="{ nodeClicked: isFolder && open}"
        @click="openCloseClickHandler"></span> -->
      <span class="treeview__node-icon"
        :class="{
          'treeview__pack-arrow': isFolder,
          'treeview__pack-arrow--open': isFolder && open }"
          @click="openCloseClickHandler">
          <i v-if="isFolder" class="fas fa-caret-right"></i>
          <i v-if="model.type === 'code'" class="fab fa-js-square"></i>
          <i v-if="model.type === 'request'" class="fas fa-globe-americas" ></i>
          </span>
      <i v-if="!model.isPristine" class="fas fa-circle treeview__node-not-pristine-icon"></i>
      <span class='treeview__node-header-text'
        @dragenter.stop='dragEnter'
        @dragleave.stop='dragLeave'
        @click="textClickHandler">
        {{model.name}}</span>
    </div>
    <div
      :style=styleObj2
      v-if="isDragging && isFolder" class="treeview__node-drop-zone"
      @dragleave.stop='dragLeave'
      @dragenter.stop='dragEnter' >
      </div>
    <ul class='treeview__node-childlist' v-show="open" v-if="isFolder">
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
      open: true,
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
      'selectedNodeId',
      'selectedNodeType',
      'draggedNode',
      'isDragging']),
    isSelected() {
      return this.model.id === this.selectedNodeId;
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
      'dropHandler',
      'setDraggedNodeId',
      'dropOn',
      'setIsDragging'
    ]),
    openCloseClickHandler() {
      if (this.isFolder) {
        this.open = !this.open;
      }
    },
    textClickHandler() {
      this.selectNode(this.model.id);
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
      return true;
    },
    dragEnter() {
      if (this.model.id !== this.draggedNode.id) {
        this.styleObj.opacity = 0.5;
        this.styleObj2.background = 'rgba(0, 0, 255, 0.1)';
      }
    },
    dragLeave() {
      this.styleObj.opacity = 1;
      this.styleObj2.background = 'rgba(0, 0, 255, 0)';
    },
    drop() {
      this.styleObj.opacity = 1;
      this.dropOn(this.model.id);
    },
    dragEnd() {
      this.styleObj.opacity = 1;
      this.setIsDragging(false);
    }
  },
  created() {
  }
};
</script>

<style lang="scss">
.treeview__node-container {
  list-style: none;
  position: relative;
}

.treeview__node-container--selected {
  background: #ececec;
  border-radius: 5px;
  & > div > span.treeview__node-header-text {
    font-weight: bold;
  }
}

.treeview__node-header {
  display: inline-block;
  height: 28px;
  box-sizing: border-box;
  width: 100%;
  font-size: 18px;
  color: #324057;
  align-items: center;
}

.treeview__node-icon {
  padding-left: 10px;
  padding-right: 8px;
}

.treeview__pack-arrow {
  &:hover {
    color: black;
    cursor: pointer;
  }

  i {
    transition: transform 0.1s ease-in-out;
  }
}

.treeview__node-header-text {
  display: inline-block;
  line-height: 28px;
  font-size: 0.7em;

  &:hover {
    color: rgb(31, 31, 31);
    cursor: pointer;
  }
}
.treeview__node-not-pristine-icon {
  font-size: 0.5em;
  color: #a70000;
}

.treeview__pack-arrow--open i {
  transform: rotate(90deg);
}

.treeview__node-childlist {
  padding-inline-start: 25px;
}

.treeview__node-drop-zone {
  position: absolute;
  top: 0px;
  height: 100%;
  width: 100%;
  pointer-events: none;
  z-index: 30;
}
</style>

<template>
  <li
    class="treeview__node-container"
    :style="styleObj"
    :class="{ 'treeview__node-container--selected': isSelected }"
  >
    <div
      :draggable="true"
      @drag.stop="drag"
      @dragover.stop="dragOver"
      @drop.stop="drop"
      @dragend.stop="dragEnd"
      @dragenter.stop="dragEnter"
      @dragleave.stop="dragLeave"
      class="treeview__node-header"
      :style="{ 'padding-left': (this.depth - 1) * 12 + 'px' }"
    >
      <!-- <span v-if="isFolder" class="vue-drag-node-icon"
        :class="{ nodeClicked: isFolder && open}"
        @click="openCloseClickHandler"></span> -->
      <span
        class=""
        :class="{
          'treeview__pack-arrow': isFolder,
          'treeview__pack-arrow--open': isFolder && open
        }"
        @click="openCloseClickHandler"
      >
        <span v-if="isFolder" class="treeview__nodeType">P</span>
        <span
          v-if="model.type === scriptNodeType"
          class="treeview__nodeType"
          :icon="['fab', 'js-square']"
          >S</span
        >
        <span
          v-if="model.type === requestNodeType"
          class="treeview__nodeType"
          icon="globe-americas"
          >R</span
        >
      </span>
      <i
        v-if="!model.isPristine"
        class="fas fa-circle treeview__node-not-pristine-icon"
      ></i>
      <span
        class="treeview__node-header-text"
        @dragenter.stop="dragEnter"
        @dragleave.stop="dragLeave"
        @click="textClickHandler"
      >
        {{ model.name }}</span
      >
    </div>
    <div
      :style="styleObj2"
      v-if="treeModule.isDragging && isFolder"
      class="treeview__node-drop-zone"
      @dragleave.stop="dragLeave"
      @dragenter.stop="dragEnter"
    ></div>
    <ul class="treeview__node-childlist" v-show="open" v-if="isFolder">
      <MovableNode
        v-for="child in model.children"
        :depth="increaseDepth"
        :model="child"
        :key="child.id"
        :root-reference="rootReference"
      >
      </MovableNode>
    </ul>
  </li>
</template>

<script lang="ts">
import { Component, Prop, Vue, Watch } from 'vue-property-decorator';
import { getModule } from 'vuex-module-decorators';
import { Node, RequestNode } from '@/domain/Node';
import TreeModule from '@/store/TreeModule';
import { NodeType } from '@/domain/NodeType';

@Component({ components: { MovableNode } })
export default class MovableNode extends Vue {
  private PackNodeType = NodeType.Pack;
  private requestNodeType = NodeType.Request;
  private scriptNodeType = NodeType.Script;

  private open: boolean = true;
  private styleObj: { [key: string]: any } = { opacity: 1 };
  private styleObj2: { [key: string]: any } = {
    background: 'rgba(0, 0, 255, 0'
  };
  private treeModule = getModule(TreeModule);

  @Prop(Object) rootReference!: Node;
  @Prop(Object) model!: Node;
  @Prop(Number) depth!: number;

  public get isSelected() {
    return this.model.id === this.treeModule.selectedNodeId;
  }
  public get isFolder() {
    return this.model.children && this.model.children.length;
  }
  public get increaseDepth() {
    return this.depth + 1;
  }
  openCloseClickHandler() {
    if (this.isFolder) {
      this.open = !this.open;
    }
  }
  textClickHandler() {
    this.treeModule.selectNode(this.model.id);
  }
  removeChild(childId: string) {
    const parentModelChildren = (this.$parent as MovableNode).model.children;
    console.error('not implemented', childId, parentModelChildren);
    // for (let index in parentModelChildren) {
    //   if (parentModelChildren[index].id == childId) {
    //     parentModelChildren = parentModelChildren.splice(index, 1);
    //     break;
    //   }
    // }
  }
  drag() {
    if (!this.treeModule.isDragging) {
      this.treeModule.setDraggedNodeId(this.model.id);
      this.treeModule.setIsDragging(true);
    }
  }
  dragOver(event: any) {
    event.preventDefault();
    return true;
  }
  dragEnter() {
    if (
      this.treeModule.draggedNode &&
      this.model.id !== this.treeModule.draggedNode.id
    ) {
      this.styleObj.opacity = 0.5;
      this.styleObj2.background = 'rgba(0, 0, 255, 0.1)';
    }
  }
  dragLeave() {
    this.styleObj.opacity = 1;
    this.styleObj2.background = 'rgba(0, 0, 255, 0)';
  }
  drop() {
    this.styleObj.opacity = 1;
    this.treeModule.dropOn(this.model.id);
  }
  dragEnd() {
    this.styleObj.opacity = 1;
    this.treeModule.setIsDragging(false);
  }
}
</script>

<style lang="scss" scoped>
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
.treeview__nodeType {
  display: inline-block;
  padding: 1px 3px;
  background: grey;
  color: white;
  border-radius: 3px;
  margin: 0 3px 0 0;
}
</style>

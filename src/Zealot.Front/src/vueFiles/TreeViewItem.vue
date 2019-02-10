<template>
  <li>
    <div
      :class="{bold: isFolder}"
      @click="toggle"
      @dblclick="changeType">
      {{ model.name }}
      <span v-if="isFolder">[{{ open ? '-' : '+' }}]</span>
    </div>
    <ul v-show="open" v-if="isFolder">
      <tree-view-item
        class="item"
        v-for="(model, index) in model.children"
        :key="index"
        :model="model">
      </tree-view-item>
      <li class="add" @click="addChild">+</li>
    </ul>
  </li>
</template>

<script>

export default {
  name: 'tree-view-item',
  data: function () {
    return {
      open: false
    };
  },
  props: {
    model: Object
  },
  computed: {
    isFolder: function () {
      return this.model.children && this.model.children.length;
    }
  },
  methods: {
    toggle: function () {
      if (this.isFolder) {
        this.open = !this.open;
      }
    },
    changeType: function () {
      if (!this.isFolder) {
        // vue.set(this.model, 'children', []);
        console.log('avant il y avait du code');
        this.addChild();
        this.open = true;
      }
    },
    addChild: function () {
      this.model.children.push({
        name: 'new stuff'
      });
    }
  }
};
</script>

<template>
  <b-modal id="projectLoadModal" title="Load a project" @ok="loadProjectLocal">
    <b-form-group id="fieldset2" label="Project folder" label-for="projectPath" >
      <b-form-input id="projectPath" v-model="projectPath" type="text" placeholder="C:\path\to\project"></b-form-input>
    </b-form-group>
  </b-modal>
</template>
<script>

export default {
  data() {
    return {
      projectPath: ''
    };
  },
  methods: {
    loadProjectLocal() {
    }
  }
};
</script>

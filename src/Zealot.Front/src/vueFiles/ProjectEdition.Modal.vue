<template>
  <b-modal id="projectEditionModal" title="Create project" @ok="addProjectLocal">
    <b-form-group id="fieldset2" label="Project name" label-for="projectName" >
      <b-form-input id="projectName" v-model="projectName" type="text" placeholder=""></b-form-input>
    </b-form-group>
    <b-form-group id="fieldset2" label="Project folder" label-for="projectFolder" >
      <b-form-input id="projectFolder" v-model="projectFolder" type="text" placeholder="C:\path\to\project"></b-form-input>
    </b-form-group>
  </b-modal>
</template>
<script>
import { mapActions } from 'vuex';

export default {
  data() {
    return {
      projectName: 'test1',
      projectFolder: 'D:\\_Prog\\Projects\\Zealot\\test\\folder1'
    };
  },
  methods: {
    ...mapActions('projectModule', ['saveProject']),
    addProjectLocal() {
      this.saveProject({
        projectName: this.projectName,
        projectFolder: this.projectFolder
      });
    }
  }
};
</script>

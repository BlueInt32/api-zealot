<template>
  <b-modal id="projectEditionModal" title="Create project" @ok="addProjectLocal">
    <b-form-group id="fieldset2" label="Project name" label-for="projectName" >
      <b-form-input id="projectName" v-model="projectName" type="text" placeholder=""></b-form-input>
    </b-form-group>
    <b-form-group id="fieldset2" label="Project path" label-for="projectPath" >
      <b-form-input id="projectPath" v-model="projectPath" type="text" placeholder="C:\path\to\project"></b-form-input>
    </b-form-group>
  </b-modal>
</template>
<script>
import { mapActions } from 'vuex';

export default {
  data() {
    return {
      projectName: 'test1',
      projectPath: 'D:\\_Prog\\Projects\\Zealot\\test\\folder1'
    };
  },
  methods: {
    ...mapActions('projectModule', ['createProject']),
    addProjectLocal() {
      this.createProject({
        projectName: this.projectName,
        projectPath: this.projectPath
      });
    }
  }
};
</script>

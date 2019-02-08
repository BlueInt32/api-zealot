<template>
  <b-modal id="postEditionModal" title="Add stuff" @ok="addStuffLocal">
    <b-form-group id="fieldset2" label="Title" label-for="stuffTitle" >
      <b-form-input id="stuffTitle" v-model="stuffTitle" type="text" placeholder="Title"></b-form-input>
    </b-form-group>
    <b-form-group id="fieldset2" label="Text" label-for="stuffText" >
      <b-form-input id="stuffText" v-model="stuffText" type="text" placeholder="Text"></b-form-input>
    </b-form-group>
  </b-modal>
</template>
<script>
import { mapActions } from 'vuex';

export default {
  data() {
    return {
      stuffTitle: '',
      stuffText: ''
    };
  },
  methods: {
    ...mapActions('stuffModule', ['addStuff']),
    addStuffLocal() {
      this.addStuff({ title: this.stuffTitle, text: this.stuffText });
    }
  }
};
</script>

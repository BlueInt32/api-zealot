<template>
  <b-input-group>
    <b-dropdown variant="outline-secondary" slot="prepend">
      <template slot="button-content">
        {{ httpMethod }}<span class="sr-only"></span>
      </template>
      <b-dropdown-item v-for="httpMethod in httpMethods" :key="httpMethod" :id="httpMethod"
        @click="setHttpMethod(httpMethod)">{{ httpMethod }}</b-dropdown-item>
    </b-dropdown>
    <b-input-group-prepend>
      <b-btn variant="success" @click="sendRequest"><i class="fas fa-play"></i></b-btn>
    </b-input-group-prepend>
    <div v-if="false">
      <!-- I have been trying to use 'b-form-input' instead of this raw html input for days,
      but it seemed to trigger the @input event when it was not necessary.
      I prefer my 'helpers' when they actually help... -->
    </div>
    <input
      class="form-control"
      type="text"
      :value="requestUrl"
      @input="setRequestUrl($event.target.value)"
      />
  </b-input-group>
</template>

<script>
import { mapActions, mapGetters } from 'vuex';

export default {
  components: {
  },
  filters: {
  },
  data() {
    return {
    };
  },
  computed: {
    ...mapGetters('configModule', ['httpMethods']),
    ...mapGetters('requestModule', ['requestUrl', 'httpMethod']),
  },
  created() {
  },
  mounted() {
  },
  methods: {
    ...mapActions('treeModule', ['setNodeProperties']),
    ...mapActions('requestModule', [
      'sendRequest',
      'setRequestUrl',
      'setHttpMethod']),
  }
};
</script>
<style lang="scss" scoped>
.btn-success:focus {
  box-shadow: none;
}
</style>

<template>
  <div>
    <ul class="nav nav-tabs ml-2">
      <li role="presentation" class="nav-item active">
        <a class="nav-link" href="#">Body</a>
      </li>
      <li role="presentation" class="nav-item">
        <a class="nav-link" href="#">Headers</a>
      </li>
    </ul>
    <div class="form-group request-panel__container">
      <textarea class="form-control" rows="10"></textarea>
    </div>
  </div>
</template>

<script>
export default {

};
</script>
<style lang="scss" scoped>
.request-panel__container {
  padding: 0.5em;
}
.nav-pills .nav-link {
  border-radius: 4px 4px 0 0;
}
</style>

<template>
  <div class="postsListContainer">
    <div id="posts" class="grid" >
    </div>
  </div>
</template>

<script>
import { mapActions, mapGetters } from 'vuex';

export default {
  data() {
    return {
    };
  },
  components: {
  },
  computed: {
    ...mapGetters('stuffModule', []),
      title() {
        if (this.posts[this.zoomedPostPageIndex]) {
          return this.posts[this.zoomedPostPageIndex].title;
        }
        return '';
      },
      tags() {
        if (this.posts[this.zoomedPostPageIndex]) {
          return this.posts[this.zoomedPostPageIndex].tags;
        }
        return '';
      }
  },
  filters: {
  },
  methods: {
    ...mapActions('stuffModule', ['zoomOnPost', 'closeZoom']),
    getThumbnailBgImage(imageRootUrl, fileName) {
      return `url(${imageRootUrl}${fileName})`;
    }
  }
};
</script>

<style lang="scss" scoped>
</style>

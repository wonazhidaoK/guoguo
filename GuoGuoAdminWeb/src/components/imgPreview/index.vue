<template>
  <div class="imgPreview">
    <img :src="nowimg" alt="">
  </div>
</template>

<script>

export default {
  name: 'ImgPreview',
  components: { },
  props: {
    nowimg: {
      type: String,
      default() {
        return ''
      }
    }
  },
  methods: {
  }
}
</script>

<style scoped>
  img{
    width: 100%;
  }
</style>

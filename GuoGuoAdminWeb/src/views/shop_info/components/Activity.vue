<template>
  <div class="user-activity">
    <div class="post">
      <!-- <div class="user-block">
        <img :src="'https://wpimg.wallstcn.com/fb57f689-e1ab-443c-af12-8d4066e202e2.jpg'+avatarPrefix" class="img-circle">
        <span class="username">Spider Man</span>
        <span class="description">Posted 4 photos - 2 days ago</span>
      </div> -->
      <!-- <div class="user-images">
        <el-carousel :interval="6000" type="card" height="220px">
          <el-carousel-item v-for="item in images" :key="item">
            <img :src="item+carouselPrefix" class="image">
          </el-carousel-item>
        </el-carousel>
      </div> -->
      <img :src="images[0]" alt="" style="width:100%;">
      <!-- <ul class="list-inline">
        <li><span class="link-black text-sm"><i class="el-icon-share" /> Share</span></li>
        <li>
          <span class="link-black text-sm"><svg-icon icon-class="like" /> Like</span>
        </li>
      </ul> -->
    </div>
  </div>
</template>

<script>
const avatarPrefix = '?imageView2/1/w/80/h/80'
const carouselPrefix = '?imageView2/2/h/440'

export default {
  props: {
    images: {
      type: Array,
      default: () => {
        return [
          'https://wpimg.wallstcn.com/9679ffb0-9e0b-4451-9916-e21992218054.jpg'
        ]
      }
    }
  },
  data() {
    return {
      avatarPrefix,
      carouselPrefix
    }
  }
}
</script>

<style lang="scss" scoped>
.user-activity {
  .user-block {

    .username,
    .description {
      display: block;
      margin-left: 50px;
      padding: 2px 0;
    }

    .username{
      font-size: 16px;
      color: #000;
    }

    :after {
      clear: both;
    }

    .img-circle {
      border-radius: 50%;
      width: 40px;
      height: 40px;
      float: left;
    }

    span {
      font-weight: 500;
      font-size: 12px;
    }
  }

  .post {
    font-size: 14px;
    border-bottom: 1px solid #d2d6de;
    margin-bottom: 15px;
    padding-bottom: 15px;
    color: #666;

    .image {
      width: 100%;
      height: 100%;

    }

    .user-images {
      padding-top: 20px;
    }
  }

  .list-inline {
    padding-left: 0;
    margin-left: -5px;
    list-style: none;

    li {
      display: inline-block;
      padding-right: 5px;
      padding-left: 5px;
      font-size: 13px;
    }

    .link-black {

      &:hover,
      &:focus {
        color: #999;
      }
    }
  }

}

.box-center {
  margin: 0 auto;
  display: table;
}

.text-muted {
  color: #777;
}
</style>

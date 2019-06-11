<template>
  <div class="navbar">
    <!-- 控制左侧导航 显示还是缩略图 -->
    <hamburger :toggle-click="toggleSideBar" :is-active="sidebar.opened" class="hamburger-container"/>
    <!-- 顶部 路由位置 -->
    <breadcrumb class="breadcrumb-container"/>
    <!-- <el-button
      plain
      @click="open5">
      消息
    </el-button> -->
    <!-- 顶部 右边 -->
    <div class="right-menu">
      <template v-if="device!=='mobile'">
        <!-- 搜索 -->
        <!-- <search class="right-menu-item" /> -->
        <div v-show="dep=='Property'" class="right-menu-item hover-effect emailWrap">
          <router-link to="/wy_letter/wy_letter">
            <svg-icon icon-class="email" />
            <span v-show="isHaveUnRead" class="radioRed"/>
          </router-link>
        </div>
        <!-- <error-log class="errLog-container right-menu-item hover-effect"/> -->
        <screenfull class="right-menu-item hover-effect"/>
      </template>
      <el-dropdown class="avatar-container right-menu-item hover-effect" trigger="click">
        <div class="avatar-wrapper">
          <img :src="avatar+'?imageView2/1/w/80/h/80'" class="user-avatar">
          <i class="el-icon-caret-bottom"/>
        </div>
        <el-dropdown-menu slot="dropdown">
          <router-link to="/">
            <el-dropdown-item>
              {{ $t('navbar.dashboard') }}
            </el-dropdown-item>
          </router-link>
          <el-dropdown-item divided>
            <span style="display:block;" @click="logout">{{ $t('navbar.logOut') }}</span>
          </el-dropdown-item>
        </el-dropdown-menu>
      </el-dropdown>
    </div>
    <signalr-coms :apiurl="apiurl" :source="source" :comid="comid" :eid="eid"/>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
import Breadcrumb from '@/components/Breadcrumb'
import Hamburger from '@/components/Hamburger'
import ErrorLog from '@/components/ErrorLog'
import Screenfull from '@/components/Screenfull'
import SizeSelect from '@/components/SizeSelect'
import LangSelect from '@/components/LangSelect'
import ThemePicker from '@/components/ThemePicker'
import Search from '@/components/HeaderSearch'
import SignalrComs from '@/components/SignalrComs'

export default {
  components: {
    Breadcrumb,
    Hamburger,
    ErrorLog,
    Screenfull,
    SizeSelect,
    LangSelect,
    ThemePicker,
    Search,
    SignalrComs
  },
  data() {
    return {
      IsHaveUnRead: false,
      apiurl: '',
      source: '',
      comid: '',
      eid: ''
    }
  },
  computed: {
    ...mapGetters([
      'sidebar',
      'name',
      'avatar',
      'device',
      'loginuser'
    ])
  },
  created() {
    this.isHaveUnRead = this.$store.getters.loginuser.IsHaveUnRead
    this.dep = this.$store.getters.loginuser.DepartmentValue
    this.apiurl = process.env.API_HOST
    console.log(this.$store.getters.loginuser)
    this.eid = this.$store.getters.loginuser.Id
    if (this.$store.getters.loginuser.DepartmentValue === 'Shop') {
      this.source = '2'
      this.comid = this.$store.getters.loginuser.ShopId
    } else if (this.$store.getters.loginuser.DepartmentValue === 'Property') {
      this.source = '1'
      this.comid = this.$store.getters.loginuser.PropertyCompanyId
    } else {
      this.comid = ''
    }
    console.log(this.apiurl, this.source, this.comid, this.eid)
  },
  methods: {
    toggleSideBar() {
      this.$store.dispatch('toggleSideBar')
    },
    logout() {
      this.$store.dispatch('LogOut').then(() => {
        location.reload()// In order to re-instantiate the vue-router object to avoid bugs
      })
    }
    // open5() {
    //   this.$notify({
    //     title: 'HTML 片段',
    //     dangerouslyUseHTMLString: true,
    //     message: '<strong onclick="sss()">这是 <i>HTML</i> 片段</strong>',
    //     duration: 0
    //   })
    // },
  }
}

</script>

<style rel="stylesheet/scss" lang="scss" scoped>
.navbar {
  height: 50px;
  overflow: hidden;

  .hamburger-container {
    line-height: 46px;
    height: 100%;
    float: left;
    cursor: pointer;
    transition: background .3s;

    &:hover {
      background: rgba(0, 0, 0, .025)
    }
  }

  .breadcrumb-container {
    float: left;
  }

  .errLog-container {
    display: inline-block;
    vertical-align: top;
  }

  .right-menu {
    float: right;
    height: 100%;
    line-height: 50px;

    &:focus {
      outline: none;
    }

    .right-menu-item {
      display: inline-block;
      padding: 0 8px;
      height: 100%;
      font-size: 18px;
      color: #5a5e66;
      vertical-align: text-bottom;

      &.hover-effect {
        cursor: pointer;
        transition: background .3s;

        &:hover {
          background: rgba(0, 0, 0, .025)
        }
      }
    }
    .emailWrap{
      position: relative;
    }
    .radioRed {
      background: red;
      width: 5px;
      height: 5px;
      border-radius: 50%;
      position: absolute;
      right:4px;
      top:12px;
    }

    .avatar-container {
      margin-right: 30px;

      .avatar-wrapper {
        margin-top: 5px;
        position: relative;

        .user-avatar {
          cursor: pointer;
          width: 40px;
          height: 40px;
          border-radius: 10px;
        }

        .el-icon-caret-bottom {
          cursor: pointer;
          position: absolute;
          right: -20px;
          top: 25px;
          font-size: 12px;
        }
      }
    }
  }

}
</style>

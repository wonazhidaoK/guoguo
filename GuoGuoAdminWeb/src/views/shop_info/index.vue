<template>
  <div class="app-container">
    <div v-if="user">
      <el-row :gutter="20">

        <el-col :span="6" :xs="24">
          <user-card :user="user" :imgajaxurl="imgajaxurl"/>
        </el-col>

        <el-col :span="18" :xs="24">
          <el-card>
            <el-tabs v-model="activeTab">
              <el-tab-pane label="商铺资质" name="activity">
                <activity :images="images" />
              </el-tab-pane>
              <!-- <el-tab-pane label="Timeline" name="timeline">
                <timeline />
              </el-tab-pane> -->
              <el-tab-pane label="编辑商铺信息" name="account">
                <account :userfrom="userfrom" :typelist="typelist"/>
              </el-tab-pane>
            </el-tabs>
          </el-card>
        </el-col>

      </el-row>
    </div>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
import { getShopDetail, fetchTypeList } from '@/api/shopinfo'
import UserCard from './components/UserCard'
import Activity from './components/Activity'
// import Timeline from './components/Timeline'
import Account from './components/Account'

export default {
  name: 'Profile',
  // components: { UserCard, Activity, Timeline, Account },
  components: { UserCard, Activity, Account },
  provide() {
    return {
      getDetail: this.getDetail
    }
  },
  data() {
    return {
      user: {
        Address: '',
        CreateTime: '',
        Description: '',
        Id: '',
        LogoImageUrl: '',
        MerchantCategoryName: '',
        MerchantCategoryValue: '',
        Name: '',
        PhoneNumber: '',
        QualificationImageUrl: ''
      },
      userfrom: {
        Address: '',
        CreateTime: '',
        Description: '',
        Id: '',
        LogoImageUrl: '',
        MerchantCategoryName: '',
        MerchantCategoryValue: '',
        Name: '',
        PhoneNumber: '',
        QualificationImageUrl: ''
      },
      activeTab: 'activity',
      images: [],
      typelist: [],
      imgajaxurl: ''
    }
  },
  computed: {
    ...mapGetters([
      'loginuser'
    ])
  },
  created() {
    this.getDetail()
    this.getType()
    // this.getDetail2()
    this.imgajaxurl = process.env.API_HOST + '/Upload/'
  },
  methods: {
    getDetail() {
      getShopDetail(this.loginuser.ShopId).then(response => {
        const user = response.data.data
        this.user = user
        this.userfrom = JSON.parse(JSON.stringify(response.data.data))
        this.userfrom.showImg = this.userfrom.LogoImageUrl ? process.env.API_HOST + '/Upload/' + this.userfrom.LogoImageUrl : ''
        console.log(this.userfrom)
        this.images.push(this.imgajaxurl + response.data.data.QualificationImageUrl)
        // this.images.push(response.data.data.LogoImageUrl)
      })
    },
    getType() {
      fetchTypeList().then(response => {
        this.typelist = response.data.data
      })
    }
  }
}
</script>

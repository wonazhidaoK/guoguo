<template>
  <div class="app-container">
    <div v-if="user">
      <el-row :gutter="20">

        <el-col :span="6" :xs="24">
          <user-card :user="user" />
        </el-col>

        <el-col :span="18" :xs="24">
          <el-card>
            <el-tabs v-model="activeTab">
              <!-- <el-tab-pane label="商铺资质" name="activity">
                <activity :images="images" />
              </el-tab-pane> -->
              <!-- <el-tab-pane label="Timeline" name="timeline">
                <timeline />
              </el-tab-pane> -->
              <el-tab-pane label="编辑账号信息" name="account">
                <account :userfrom="userfrom" />
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
import { getWyUserDetail } from '@/api/wyinfo'
import UserCard from './components/UserCard'
// import Activity from './components/Activity'
// import Timeline from './components/Timeline'
import Account from './components/Account'

export default {
  name: 'Profile',
  // components: { UserCard, Activity, Timeline, Account },
  components: { UserCard, Account },
  provide() {
    return {
      getDetail: this.getDetail
    }
  },
  data() {
    return {
      user: {
        Account: '',
        City: '',
        CommunityId: '',
        CommunityName: '',
        DepartmentName: '',
        DepartmentValue: '',
        Id: '',
        Name: '',
        Password: '',
        PhoneNumber: '',
        Region: '',
        RoleId: '',
        RoleName: '',
        SmallDistrictId: '',
        SmallDistrictName: '',
        State: '',
        StreetOfficeId: '',
        StreetOfficeName: ''
      },
      userfrom: {
        Id: '',
        Name: '',
        Password: '',
        PhoneNumber: ''
      },
      activeTab: 'account'
    }
  },
  computed: {
    ...mapGetters([
      'loginuser'
    ])
  },
  created() {
    this.getDetail()
    // this.getType()
    // this.getDetail2()
  },
  methods: {
    getDetail() {
      getWyUserDetail(this.loginuser.Id).then(response => {
        const user = response.data.data
        this.user = user
        this.userfrom = JSON.parse(JSON.stringify(response.data.data))
      })
    }
  }
}
</script>

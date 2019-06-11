<template>
  <div class="app-container">
    <div class="filter-container">
      <el-input v-model="listQuery.Name" placeholder="用户名" style="width: 200px;" class="filter-item" @keyup.enter.native="handleFilter"/>
      <el-select v-model="listQuery.state" :placeholder="$t('table.State')" clearable style="width: 90px" class="filter-item" @change="selectState(listQuery.state, 'select')">
        <el-option v-for="item in statelistSelect" :key="item.Name" :label="item.Name" :value="item.Name"/>
      </el-select>

      <el-select v-model="listQuery.city" :placeholder="$t('table.City')" clearable style="width: 90px" class="filter-item" @change="selectCity(listQuery.city, 'select')">
        <el-option v-for="item in citylistSelect" :key="item.Name" :label="item.Name" :value="item.Name"/>
      </el-select>

      <el-select v-model="listQuery.region" :placeholder="$t('table.Region')" clearable style="width: 90px" class="filter-item" @change="selectRegion(listQuery.region, 'select')">
        <el-option v-for="item in regionListSelect" :key="item.Name" :label="item.Name" :value="item.Name"/>
      </el-select>
      <!-- streetOfficeId -->
      <el-select v-model="listQuery.streetOfficeId" :placeholder="$t('table.Jdb')" clearable style="width: 90px" class="filter-item" @change="selectJdb(listQuery.streetOfficeId, 'select')">
        <el-option v-for="item in jdbListSelect" :key="item.Name" :label="item.Name" :value="item.Id"/>
      </el-select>
      <el-select v-model="listQuery.communityId" :placeholder="$t('table.Shequ')" clearable style="width: 90px" class="filter-item" @change="selectShequ(listQuery.communityId, 'select')">
        <el-option v-for="item in shequListSelect" :key="item.Name" :label="item.Name" :value="item.Id"/>
      </el-select>
      <el-select v-model="listQuery.smallDistrictId" :placeholder="$t('table.Xiaoqu')" clearable style="width: 90px" class="filter-item" @change="handleFilter">
        <el-option v-for="item in xiaoquListSelect" :key="item.Name" :label="item.Name" :value="item.Id"/>
      </el-select>

      <el-button v-waves class="filter-item" type="primary" icon="el-icon-search" @click="handleFilter">{{ $t('table.search') }}</el-button>
      <!-- 登记 -->
      <el-button class="filter-item" style="margin-left: 10px;" type="primary" icon="el-icon-edit" @click="handleCreate">{{ $t('table.register') }}</el-button>
    </div>

    <el-table
      v-loading="listLoading"
      :key="tableKey"
      :data="list"
      border
      fit
      highlight-current-row
      style="width: 100%;">
      <el-table-column label="省市区" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.State }}-{{ scope.row.City }}-{{ scope.row.Region }}</span>
        </template>
      </el-table-column>
      <el-table-column label="街道办" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.StreetOfficeName }}</span>
        </template>
      </el-table-column>
      <el-table-column label="社区" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.CommunityName }}</span>
        </template>
      </el-table-column>
      <el-table-column label="小区" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.SmallDistrictName }}</span>
        </template>
      </el-table-column>
      <el-table-column label="角色" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.RoleName }}</span>
        </template>
      </el-table-column>
      <el-table-column label="用户姓名" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.Name }}</span>
        </template>
      </el-table-column>
      <el-table-column label="手机号" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.PhoneNumber }}</span>
        </template>
      </el-table-column>
      <el-table-column label="账号" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.Account }}</span>
        </template>
      </el-table-column>
      <el-table-column :label="$t('table.actions')" align="center" class-name="small-padding fixed-width">
        <template slot-scope="scope">
          <el-button type="primary" size="mini" @click="handleUpdate(scope.row)">{{ $t('table.edit') }}</el-button>
          <el-button v-if="scope.row.status!='deleted'" size="mini" type="danger" @click="delYwhBtn(scope.row, scope.row.Id)">{{ $t('table.logoutBtn') }}
          </el-button>
        </template>
      </el-table-column>
    </el-table>

    <pagination v-show="total>0" :total="total" :page.sync="listQuery.pageIndex" :limit.sync="listQuery.pageSize" @pagination="getList" />

    <el-dialog :title="textMap[dialogStatus]" :visible.sync="dialogFormVisible">
      <el-form ref="dataForm" :rules="rules" :model="temp" label-position="left" label-width="120px" style="width: 100%;box-sizing:border-box; padding:0 50px;">
        <el-form-item v-show="dialogStatus==='create'" label="省" prop="State">
          <el-select v-model="temp.State" class="filter-item" placeholder="省" @change="selectState(temp.State)">
            <el-option v-for="item in statelist" :key="item.Name" :label="item.Name" :value="item.Name" :disabled="temp.Id!=''&&temp.Id!=null"/>
          </el-select>
        </el-form-item>
        <el-form-item v-show="dialogStatus==='create'" label="市" prop="City">
          <el-select v-model="temp.City" class="filter-item" placeholder="市" @change="selectCity(temp.City)">
            <el-option v-for="item in citylist" :key="item.Name" :label="item.Name" :value="item.Name" :disabled="temp.Id!=''&&temp.Id!=null"/>
          </el-select>
        </el-form-item>
        <el-form-item v-show="dialogStatus==='create'" :label="$t('table.Region')" prop="Region">
          <el-select v-model="temp.Region" class="filter-item" placeholder="区" @change="selectRegion(temp.Region)">
            <el-option v-for="item in regionList" :key="item.Name" :label="item.Name" :value="item.Name" :disabled="temp.Id!=''&&temp.Id!=null"/>
          </el-select>
        </el-form-item>
        <el-form-item v-show="dialogStatus==='create'" :label="$t('table.Jdb')" prop="StreetOfficeId">
          <el-select v-model="temp.StreetOfficeId" class="filter-item" placeholder="街道办" @change="selectJdb(temp.StreetOfficeId)">
            <el-option v-for="item in jdbList" :key="item.Name" :label="item.Name" :value="item.Id" :disabled="temp.Id!=''&&temp.Id!=null"/>
          </el-select>
        </el-form-item>
        <el-form-item v-show="dialogStatus==='create'" :label="$t('table.Shequ')" prop="CommunityId">
          <el-select v-model="temp.CommunityId" class="filter-item" placeholder="社区" @change="selectShequ(temp.CommunityId)">
            <el-option v-for="item in shequList" :key="item.Name" :label="item.Name" :value="item.Id" :disabled="temp.Id!=''&&temp.Id!=null"/>
          </el-select>
        </el-form-item>
        <el-form-item v-show="dialogStatus==='create'" :label="$t('table.Xiaoqu')" prop="SmallDistrictId">
          <el-select v-model="temp.SmallDistrictId" class="filter-item" placeholder="小区">
            <el-option v-for="item in xiaoquList" :key="item.Name" :label="item.Name" :value="item.Id" :disabled="temp.Id!=''&&temp.Id!=null"/>
          </el-select>
        </el-form-item>
        <el-form-item v-show="dialogStatus==='create'" :label="$t('table.Role')" prop="RoleId">
          <el-select v-model="temp.RoleId" class="filter-item" placeholder="角色">
            <el-option v-for="item in roleList" :key="item.Name" :label="item.Name" :value="item.Id" :disabled="temp.Id!=''&&temp.Id!=null"/>
          </el-select>
        </el-form-item>
        <el-form-item label="用户名称" prop="Name">
          <el-input v-model="temp.Name"/>
        </el-form-item>
        <el-form-item v-show="dialogStatus==='create'" label="用户账号" prop="Account">
          <el-input v-model="temp.Account"/>
        </el-form-item>
        <el-form-item label="手机号" prop="PhoneNumber">
          <el-input v-model="temp.PhoneNumber"/>
        </el-form-item>
        <el-form-item label="密码" prop="Password">
          <el-input v-model="temp.Password"/>
        </el-form-item>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="dialogFormVisible = false">{{ $t('table.cancel') }}</el-button>
        <el-button v-loading="createLoading" type="primary" @click="dialogStatus==='create'?createData():updateData()">{{ $t('table.confirm') }}</el-button>
      </div>
    </el-dialog>

  </div>
</template>

<script>
import { mapGetters } from 'vuex'
import { fetchUserWyList, createUserWy, updateUserWy, getState, getCity, getRegion, getJbdList, delUserRequest, getShequList, getXiaoquList, fetRoleListWy } from '@/api/wwJdb'
import waves from '@/directive/waves' // Waves directive
import Pagination from '@/components/Pagination' // Secondary package based on el-pagination

const calendarTypeOptions = [
  { key: 'CN', display_name: 'China' },
  { key: 'US', display_name: 'USA' },
  { key: 'JP', display_name: 'Japan' },
  { key: 'EU', display_name: 'Eurozone' }
]

// arr to obj ,such as { CN : "China", US : "USA" }
const calendarTypeKeyValue = calendarTypeOptions.reduce((acc, cur) => {
  acc[cur.key] = cur.display_name
  return acc
}, {})

// string 转 object
const string2Object = str => {
  return JSON.parse(str)
}
// object 转 string
const object2String = obj => {
  return JSON.stringify(obj)
}

export default {
  name: 'ComplexTable',
  components: { Pagination },
  directives: { waves },
  filters: {
    statusFilter(status) {
      const statusMap = {
        published: 'success',
        draft: 'info',
        deleted: 'danger'
      }
      return statusMap[status]
    },
    typeFilter(type) {
      return calendarTypeKeyValue[type]
    },
    string2Object,
    object2String
  },
  data() {
    var checkPhone = (rule, value, callback) => {
      if (!value) {
        return callback(new Error('手机号不能为空'))
      } else {
        const reg = /^1[3|4|5|7|8][0-9]\d{8}$/
        // console.log(reg.test(value))
        if (reg.test(value)) {
          callback()
        } else {
          return callback(new Error('请输入正确的手机号'))
        }
      }
    }
    var checkAccount = (rule, value, callback) => {
      if (!value) {
        return callback(new Error('用户账号不能为空'))
      } else {
        const reg = /^[a-zA-Z0-9_]{1,}$/
        // console.log(reg.test(value))
        if (reg.test(value)) {
          callback()
        } else {
          return callback(new Error('用户账号由六位以上的英文字母，数字，下划线组成'))
        }
      }
    }
    return {
      tableKey: 0,
      list: null,
      total: 0,
      listLoading: true,
      listQuery: {
        pageIndex: 1,
        pageSize: 20,
        state: this.$store.getters.province,
        city: this.$store.getters.city,
        region: this.$store.getters.region
      },
      importanceOptions: [1, 2, 3],
      calendarTypeOptions,
      sortOptions: [{ label: 'ID Ascending', key: '+id' }, { label: 'ID Descending', key: '-id' }],
      statusOptions: ['published', 'draft', 'deleted'],
      showReviewer: false,
      temp: {
        Name: '',
        State: '',
        City: '',
        Region: '',
        StreetOfficeId: '',
        CommunityId: '',
        SmallDistrictId: '',
        PhoneNumber: '',
        Password: '',
        RoleId: '',
        Account: ''
      },
      dialogFormVisible: false,
      dialogStatus: '',
      textMap: {
        update: '物业 - 编辑用户账号',
        create: '物业 - 新建用户账号'
      },
      pvData: [],
      rules: {
        Name: [{ required: true, message: '用户名称是必填的', trigger: 'blur' }],
        PhoneNumber: [{ required: true, trigger: 'blur', validator: checkPhone }],
        Password: [{ required: true, message: '密码是必填的', trigger: 'blur' }],
        State: [{ required: true, message: '省不能为空', trigger: 'change' }],
        City: [{ required: true, message: '市不能为空', trigger: 'change' }],
        Region: [{ required: true, message: '区不能为空', trigger: 'change' }],
        StreetOfficeId: [{ required: true, message: '街道办不能为空', trigger: 'change' }],
        CommunityId: [{ required: true, message: '社区不能为空', trigger: 'change' }],
        SmallDistrictId: [{ required: true, message: '小区不能为空', trigger: 'change' }],
        RoleId: [{ required: true, message: '角色是必选项', trigger: 'change' }],
        Account: [{ required: true, trigger: 'blur', validator: checkAccount }]
      },
      statelist: '',
      citylist: '',
      regionList: '',
      jdbList: '',
      shequList: '',
      xiaoquList: '',
      adressState: '',
      adressCity: '',
      adressRegion: '',
      // 筛选条件
      statelistSelect: '',
      citylistSelect: '',
      regionListSelect: '',
      adressStateSelect: this.$store.getters.province,
      adressCitySelect: this.$store.getters.city,
      adressRegionSelect: '',
      jdbListSelect: '',
      shequListSelect: '',
      xiaoquListSelect: '',
      roleList: [],
      createLoading: false,
      num: 0
    }
  },
  computed: {
    ...mapGetters([
      'name',
      'avatar',
      'roles',
      'province'
    ])
  },
  created() {
    this.getList()
    this.getSheng()
    this.getRoleList()
    this.selectState(this.$store.getters.province, 'select', 'one')
    this.selectCity(this.$store.getters.city, 'select', 'one')
    this.selectRegion(this.$store.getters.region, 'select')
  },
  methods: {
    getList() {
      this.listLoading = true
      // console.log(this.listQuery)
      fetchUserWyList(this.listQuery).then(response => {
        this.list = response.data.data.List
        this.total = response.data.data.TotalCount
        setTimeout(() => {
          this.listLoading = false
        }, 1.5 * 1000)
      })
    },
    getRoleList() {
      fetRoleListWy().then(response => {
        // console.log(response.data.data)
        this.roleList = response.data.data
      })
    },
    getSheng() {
      // 获取省
      getState().then(response => {
        this.statelist = response.data.data
        this.statelistSelect = response.data.data
        // console.log(response)
      })
    },
    // 切换 省 获取市
    selectState(stateName, type, one) {
      if (type === 'select') {
        if (one !== 'one') {
          this.listQuery.city = ''
          this.listQuery.region = ''
        }
        this.regionListSelect = ''
        this.jdbListSelect = ''
        this.shequListSelect = ''
        this.xiaoquListSelect = ''
        this.listQuery.streetOfficeId = ''
        this.listQuery.communityId = ''
        this.listQuery.smallDistrictId = ''
        if (stateName === '') {
          this.adressStateSelect = ''
          this.citylistSelect = ''
          this.handleFilter()
        } else {
          this.adressStateSelect = stateName
          getCity(stateName).then(response => {
            this.citylistSelect = response.data.data
          })
        }
      } else {
        // console.log(this.temp)
        this.adressState = stateName
        this.citylist = ''
        this.regionList = ''
        this.jdbList = ''
        this.shequList = ''
        this.xiaoquList = ''
        this.temp.City = ''
        this.temp.Region = ''
        this.temp.StreetOfficeId = ''
        this.temp.CommunityId = ''
        this.temp.SmallDistrictId = ''
        getCity(stateName).then(response => {
          this.citylist = response.data.data
          this.$nextTick(() => {
            this.$refs['dataForm'].clearValidate()
          })
        })
        // }
      }
    },
    // 切换 市 获取区
    selectCity(cityName, type, one) {
      // console.log(cityName)
      if (type === 'select') {
        if (one !== 'one') {
          this.listQuery.region = ''
        }
        this.listQuery.streetOfficeId = ''
        this.listQuery.communityId = ''
        this.listQuery.smallDistrictId = ''
        this.jdbListSelect = ''
        this.shequListSelect = ''
        this.xiaoquListSelect = ''
        if (cityName === '') {
          this.regionListSelect = ''
        } else {
          const stateName = this.adressStateSelect
          this.adressCitySelect = cityName
          getRegion(stateName, cityName).then(response => {
            this.regionListSelect = response.data.data
          })
        }
      } else {
        this.regionList = ''
        this.jdbList = ''
        this.shequList = ''
        this.xiaoquList = ''
        this.temp.Region = ''
        this.temp.StreetOfficeId = ''
        this.temp.CommunityId = ''
        this.temp.SmallDistrictId = ''
        if (cityName) {
          const stateName = this.adressState
          this.adressCity = cityName
          getRegion(stateName, cityName).then(response => {
            this.regionList = response.data.data
          })
        }
      }
    },
    // 切换 区 获取街道办
    selectRegion(region, type) {
      if (type === 'select') {
        this.listQuery.streetOfficeId = ''
        this.listQuery.communityId = ''
        this.listQuery.smallDistrictId = ''
        this.shequListSelect = ''
        this.xiaoquListSelect = ''
        if (region === '') {
          this.jdbListSelect = ''
        } else {
          const state = this.adressStateSelect
          const city = this.adressCitySelect
          // console.log(region)
          getJbdList(state, city, region).then(response => {
            // console.log(response.data.data)
            this.jdbListSelect = response.data.data
          })
        }
      } else {
        this.jdbList = ''
        this.shequList = ''
        this.xiaoquList = ''
        this.temp.StreetOfficeId = ''
        this.temp.CommunityId = ''
        this.temp.SmallDistrictId = ''
        if (region) {
          const state = this.adressState
          const city = this.adressCity
          // console.log(region)
          getJbdList(state, city, region).then(response => {
            // console.log(response.data.data)
            this.jdbList = response.data.data
          })
        }
      }
    },
    // 切换街道办获取社区
    selectJdb(jdbId, type) {
      if (type === 'select') {
        this.listQuery.communityId = ''
        this.listQuery.smallDistrictId = ''
        this.shequListSelect = ''
        this.xiaoquListSelect = ''
        if (jdbId) {
          getShequList(jdbId).then(response => {
            // console.log(response.data.data)
            this.shequListSelect = response.data.data
          })
        }
      } else {
        this.shequList = ''
        this.xiaoquList = ''
        this.temp.CommunityId = ''
        this.temp.SmallDistrictId = ''
        if (jdbId) {
          // const jdbId = string2Object(jdb).Id
          getShequList(jdbId).then(response => {
            // console.log(response.data.data)
            this.shequList = response.data.data
          })
        }
      }
    },
    // 切换社区 获取小区
    selectShequ(shequId, type) {
      if (type === 'select') {
        this.listQuery.smallDistrictId = ''
        if (shequId) {
          getXiaoquList(shequId).then(response => {
            // console.log(response.data.data)
            this.xiaoquListSelect = response.data.data
          })
        }
      } else {
        this.xiaoquList = ''
        this.temp.SmallDistrictId = ''
        if (shequId) {
          // const shequId = string2Object(shequ).Id
          getXiaoquList(shequId).then(response => {
            // console.log(response.data.data)
            this.xiaoquList = response.data.data
          })
        }
      }
    },
    handleFilter() {
      this.listQuery.pageIndex = 1
      this.getList()
    },
    handleModifyStatus(row, status) {
      this.$message({
        message: '操作成功',
        type: 'success'
      })
      row.status = status
    },
    // 删除
    delYwhBtn(row, Id) {
      this.$confirm('确定要注销吗?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'delete'
      }).then(() => {
        delUserRequest(Id).then(response => {
          this.getList()
          this.$notify({
            title: '成功',
            message: '注销成功',
            type: 'success',
            duration: 2000
          })
        })
      }).catch(() => {
      })
    },
    resetTemp() {
      this.temp = {
        Name: '',
        State: '',
        City: '',
        Region: '',
        StreetOfficeId: '',
        CommunityId: '',
        SmallDistrictId: '',
        PhoneNumber: '',
        Password: '',
        RoleId: '',
        Account: ''
      }
    },
    // 登记 按钮
    handleCreate() {
      this.resetTemp()
      this.dialogStatus = 'create'
      this.dialogFormVisible = true
      this.citylist = ''
      this.regionList = ''
      this.jdbList = ''
      this.ShequList = ''
      this.XiaoquList = ''

      this.$nextTick(() => {
        this.$refs['dataForm'].clearValidate()
      })
    },
    // 新建
    createData() {
      this.$refs['dataForm'].validate((valid) => {
        if (valid) {
          this.num = this.num + 1
          if (this.num < 2) {
            this.createLoading = true
            createUserWy(this.temp).then((response) => {
              // console.log(response.data)
              this.getList()
              this.$notify({
                title: '成功',
                message: '创建成功',
                type: 'success',
                duration: 2000
              })
              this.dialogFormVisible = false
              this.createLoading = false
              setTimeout(() => {
                this.num = 0
              }, 2000)
            }).catch((ErrMsg) => {
              this.createLoading = false
              setTimeout(() => {
                this.num = 0
              }, 2000)
            })
          }
        }
      })
    },
    handleUpdate(row) {
      // console.log(row)
      // row.State = 'State'
      // row.City = 'City'
      // row.Region = 'Region'
      // row.Jdb = 'JDB'
      // row.Shequ = 'Shequ'
      // row.Xiaoqu = row.SmallDistrictName
      // 编辑楼宇
      this.temp = Object.assign({}, row) // copy obj
      this.dialogStatus = 'update'
      this.dialogFormVisible = true
      this.$nextTick(() => {
        this.$refs['dataForm'].clearValidate()
      })
    },
    updateData() {
      this.$refs['dataForm'].validate((valid) => {
        if (valid) {
          const tempData = Object.assign({}, this.temp)
          this.num = this.num + 1
          if (this.num < 2) {
            this.createLoading = true
            updateUserWy(tempData).then((response) => {
              // console.log(response.data)
              this.getList()
              this.$notify({
                title: '成功',
                message: '编辑成功',
                type: 'success',
                duration: 2000
              })
              this.dialogFormVisible = false
              this.createLoading = false
              setTimeout(() => {
                this.num = 0
              }, 2000)
            }).catch((ErrMsg) => {
              this.createLoading = false
              setTimeout(() => {
                this.num = 0
              }, 2000)
            })
          }
        }
      })
    }
  }
}
</script>


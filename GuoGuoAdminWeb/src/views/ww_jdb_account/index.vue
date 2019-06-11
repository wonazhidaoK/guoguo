<template>
  <div class="app-container">
    <div class="filter-container">
      <el-input v-model="listQuery.Name" placeholder="用户名" style="width: 200px;" class="filter-item" @keyup.enter.native="handleFilter"/>
      <el-select v-model="listQuery.state" :placeholder="$t('table.State')" clearable style="width: 90px" class="filter-item" @change="handleFilter(),selectState(listQuery.state, 'select')">
        <el-option v-for="item in statelistSelect" :key="item.Name" :label="item.Name" :value="item.Name"/>
      </el-select>

      <el-select v-model="listQuery.city" :placeholder="$t('table.City')" clearable style="width: 90px" class="filter-item" @change="handleFilter(),selectCity(listQuery.city, 'select')">
        <el-option v-for="item in citylistSelect" :key="item.Name" :label="item.Name" :value="item.Name"/>
      </el-select>

      <el-select v-model="listQuery.region" :placeholder="$t('table.Region')" clearable style="width: 90px" class="filter-item" @change="handleFilter(),selectRegion(listQuery.region, 'select')">
        <el-option v-for="item in regionListSelect" :key="item.Name" :label="item.Name" :value="item.Name"/>
      </el-select>
      <el-select v-model="listQuery.streetOfficeId" :placeholder="$t('table.Jdb')" clearable style="width: 90px" class="filter-item" @change="handleFilter()">
        <el-option v-for="item in jdbListSelect" :key="item.Name" :label="item.Name" :value="item.Id"/>
      </el-select>
      <el-button v-waves class="filter-item" type="primary" icon="el-icon-search" @click="handleFilter">{{ $t('table.search') }}</el-button>
      <!-- 登记 -->
      <el-button class="filter-item" style="margin-left: 10px;" type="primary" icon="el-icon-edit" @click="handleCreate">{{ $t('table.Add') }}</el-button>
    </div>

    <el-table
      v-loading="listLoading"
      :key="tableKey"
      :data="list"
      border
      fit
      highlight-current-row
      style="width: 100%;">
      <el-table-column :label="$t('table.State')" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.State }}</span>
        </template>
      </el-table-column>
      <el-table-column :label="$t('table.City')" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.City }}</span>
        </template>
      </el-table-column>
      <el-table-column :label="$t('table.Region')" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.Region }}</span>
        </template>
      </el-table-column>
      <!-- 街道办 -->
      <el-table-column label="街道办名称" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.StreetOfficeName }}</span>
        </template>
      </el-table-column>
      <el-table-column label="角色名称" align="center">
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
          <el-button v-if="scope.row.status!='deleted'" size="mini" type="danger" @click="delSqBtn(scope.row, scope.row.Id)">{{ $t('table.logoutBtn') }}
          </el-button>
        </template>
      </el-table-column>
    </el-table>

    <pagination v-show="total>0" :total="total" :page.sync="listQuery.pageIndex" :limit.sync="listQuery.pageSize" @pagination="getList" />

    <el-dialog :title="textMap[dialogStatus]" :visible.sync="dialogFormVisible">
      <el-form ref="dataForm" :rules="rules" :model="temp" label-position="left" label-width="120px" style="width: 100%;box-sizing:border-box; padding:0 50px;">
        <el-form-item v-show="dialogStatus==='create'" :label="$t('table.State')" prop="State">
          <el-select v-model="temp.State" class="filter-item" placeholder="省" @change="selectState(temp.State)">
            <el-option v-for="item in statelist" :key="item.Name" :label="item.Name" :value="item.Name" :disabled="temp.Id!=''&&temp.Id!=null"/>
          </el-select>
        </el-form-item>
        <el-form-item v-show="dialogStatus==='create'" :label="$t('table.City')" prop="City">
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
          <el-select v-model="temp.StreetOfficeId" class="filter-item" placeholder="街道办">
            <el-option v-for="item in jdbList" :key="item.Name" :label="item.Name" :value="item.Id" :disabled="temp.Id!=''&&temp.Id!=null"/>
          </el-select>
        </el-form-item>
        <el-form-item v-show="dialogStatus==='create'" :label="$t('table.Role')" prop="RoleId">
          <el-select v-model="temp.RoleId" class="filter-item" placeholder="角色">
            <el-option v-for="item in roleList" :key="item.Name" :label="item.Name" :value="item.Id" :disabled="temp.Id!=''&&temp.Id!=null"/>
          </el-select>
        </el-form-item>
        <el-form-item label="用户姓名" prop="Name">
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
import { fetUserJdbList, createUserJdb, updateUserJdb, getState, getCity, getRegion, getJbdList, delUserRequest, fetRoleListJdb } from '@/api/wwJdb'
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
        return callback(new Error('账号不能为空'))
      } else {
        const reg = /^[a-zA-Z0-9_]{1,}$/
        // console.log(reg.test(value))
        if (reg.test(value)) {
          callback()
        } else {
          return callback(new Error('账号由六位以上的英文字母，数字，下划线组成'))
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
        PhoneNumber: '',
        Password: '',
        RoleId: '',
        Account: ''
      },
      dialogFormVisible: false,
      dialogStatus: '',
      textMap: {
        update: '街道办 - 编辑用户账号',
        create: '街道办 - 新建用户账号'
      },
      pvData: [],
      rules: {
        Name: [{ required: true, message: '用户姓名是必填的', trigger: 'blur' }],
        State: [{ required: true, message: '省不能为空', trigger: 'blur' }],
        City: [{ required: true, message: '市不能为空', trigger: 'blur' }],
        Region: [{ required: true, message: '区不能为空', trigger: 'blur' }],
        StreetOfficeId: [{ required: true, message: '街道办不能为空', trigger: 'blur' }],
        PhoneNumber: [{ required: true, trigger: 'blur', validator: checkPhone }],
        Password: [{ required: true, message: '密码是必填的', trigger: 'blur' }],
        RoleId: [{ required: true, message: '角色是必选项', trigger: 'change' }],
        Account: [{ required: true, trigger: 'blur', validator: checkAccount }]
      },
      statelist: '',
      citylist: '',
      regionList: '',
      jdbList: '',
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
      fetUserJdbList(this.listQuery).then(response => {
        this.list = response.data.data.List
        this.total = response.data.data.TotalCount
        setTimeout(() => {
          this.listLoading = false
        }, 1.5 * 1000)
      })
    },
    getRoleList() {
      fetRoleListJdb().then(response => {
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
        this.listQuery.streetOfficeId = ''
        this.jdbListSelect = ''
        if (stateName === '') {
          this.adressStateSelect = ''
          this.citylistSelect = ''
        } else {
          this.adressStateSelect = stateName
          getCity(stateName).then(response => {
            this.citylistSelect = response.data.data
          })
        }
      } else {
        this.adressState = ''
        this.citylist = ''
        this.regionList = ''
        this.jdbList = ''
        this.temp.City = ''
        this.temp.Region = ''
        this.temp.StreetOfficeId = ''

        if (stateName) {
          this.adressState = stateName
          getCity(stateName).then(response => {
            this.citylist = response.data.data
          })
        }
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
        this.jdbListSelect = ''
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
        this.temp.Region = ''
        this.temp.StreetOfficeId = ''
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
        this.temp.StreetOfficeId = ''
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
    // 删除街道办
    delSqBtn(row, Id) {
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
        PhoneNumber: '',
        Password: '',
        RoleId: '',
        Account: ''
      }
    },
    // 登记街道办 按钮
    handleCreate() {
      this.resetTemp()
      this.dialogStatus = 'create'
      this.dialogFormVisible = true
      this.citylist = ''
      this.regionList = ''
      this.jdbList = ''
      this.$nextTick(() => {
        this.$refs['dataForm'].clearValidate()
      })
    },
    // 新建街道办
    createData() {
      this.$refs['dataForm'].validate((valid) => {
        if (valid) {
          this.num = this.num + 1
          if (this.num < 2) {
            this.createLoading = true
            createUserJdb(this.temp).then((response) => {
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
      row.Jdb = row.StreetOfficeName
      // 编辑街道办
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
            updateUserJdb(tempData).then((response) => {
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


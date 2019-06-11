<template>
  <div class="app-container">
    <div class="filter-container">
      <el-input v-model="listQuery.Name" placeholder="名称和备注名称" style="width: 200px;" class="filter-item" @keyup.enter.native="handleFilter"/>
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

      <el-button v-waves class="filter-item" type="primary" icon="el-icon-search" @click="handleFilter('search')">{{ $t('table.search') }}</el-button>
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
      <el-table-column label="小区名称" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.SmallDistrictName }}</span>
        </template>
      </el-table-column>
      <el-table-column label="名称" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.Name }}</span>
        </template>
      </el-table-column>
      <el-table-column label="备注名称" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.RemarkName }}</span>
        </template>
      </el-table-column>
      <el-table-column label="是否有效" align="center">
        <template slot-scope="scope">
          <el-tag v-show="scope.row.IsValid" type="warning">有效</el-tag>
          <el-tag v-show="!scope.row.IsValid" type="danger">无效</el-tag>
        </template>
      </el-table-column>
      <el-table-column label="是否选举" align="center">
        <template slot-scope="scope">
          <!-- <span>{{ scope.row.IsElection?'是':'否' }}</span> -->
          <el-tag v-show="scope.row.IsElection" type="warning">是</el-tag>
          <el-tag v-show="!scope.row.IsElection" type="danger">否</el-tag>
        </template>
      </el-table-column>
      <el-table-column :label="$t('table.actions')" align="center" class-name="small-padding fixed-width">
        <template slot-scope="scope">
          <el-button type="primary" size="mini" @click="handleUpdate(scope.row)">{{ $t('table.edit') }}</el-button>
          <el-button v-if="scope.row.IsCanDeleted" size="mini" type="danger" @click="delYwhBtn(scope.row, scope.row.Id)">{{ $t('table.logoutBtn') }}</el-button>
          <el-button v-if="scope.row.IsCanInvalid" size="mini" type="success" style="width:80px;" @click="wuxiaoBtn(scope.row, scope.row.Id)">置为无效</el-button>
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
        <el-form-item label="备注名" prop="RemarkName">
          <el-input v-model="temp.RemarkName"/>
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
import { Message } from 'element-ui'
import { mapGetters } from 'vuex'
import { fetchYwhList, createYwh, updateYwh, getState, getCity, getRegion, getJbdList, delYwhRequest, getShequList, getXiaoquList, wuxiaoRequest } from '@/api/wwJdb'
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
    return {
      tableKey: 0,
      list: null,
      total: 0,
      listLoading: true,
      listQuery: {
        pageIndex: 1,
        pageSize: 20,
        // state: this.$store.getters.province,
        // city: this.$store.getters.city,
        // region: this.$store.getters.region,
        state: '',
        city: '',
        region: ''
      },
      importanceOptions: [1, 2, 3],
      calendarTypeOptions,
      sortOptions: [{ label: 'ID Ascending', key: '+id' }, { label: 'ID Descending', key: '-id' }],
      statusOptions: ['published', 'draft', 'deleted'],
      showReviewer: false,
      temp: {
        RemarkName: '',
        State: '',
        City: '',
        Region: '',
        StreetOfficeId: '',
        CommunityId: '',
        SmallDistrictId: ''
      },
      dialogFormVisible: false,
      dialogStatus: '',
      textMap: {
        update: '编辑业委会',
        create: '新建业委会'
      },
      pvData: [],
      rules: {
        RemarkName: [{ required: true, message: '业委会名称是必填的', trigger: 'change' }],
        State: [{ required: true, message: '省不能为空', trigger: 'blur' }],
        City: [{ required: true, message: '市不能为空', trigger: 'blur' }],
        Region: [{ required: true, message: '区不能为空', trigger: 'blur' }],
        StreetOfficeId: [{ required: true, message: '街道办不能为空', trigger: 'blur' }],
        CommunityId: [{ required: true, message: '社区不能为空', trigger: 'blur' }],
        SmallDistrictId: [{ required: true, message: '业委会不能为空', trigger: 'blur' }]
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
    // this.selectState(this.$store.getters.province, 'select', 'one')
    // this.selectCity(this.$store.getters.city, 'select', 'one')
    // this.selectRegion(this.$store.getters.region, 'select')
  },
  methods: {
    getList() {
      this.listLoading = true
      // console.log(this.listQuery)
      fetchYwhList(this.listQuery).then(response => {
        this.list = response.data.data.List
        // console.log(this.list)

        this.total = response.data.data.TotalCount

        // Just to simulate the time of the request
        setTimeout(() => {
          this.listLoading = false
        }, 1.5 * 1000)
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
        this.listQuery.smallDistrictId = ''
        this.listQuery.communityId = ''
        this.xiaoquListSelect = ''
        if (jdbId === '') {
          this.shequListSelect = ''
        } else {
          getShequList(jdbId).then(response => {
            this.shequListSelect = response.data.data
          })
        }
      } else {
        this.shequList = ''
        this.xiaoquList = ''
        this.temp.CommunityId = ''
        this.temp.SmallDistrictId = ''
        if (jdbId) {
          getShequList(jdbId).then(response => {
            this.shequList = response.data.data
          })
        }
      }
    },
    // 切换社区 获取小区
    selectShequ(shequId, type) {
      if (type === 'select') {
        if (shequId === '') {
          this.xiaoquListSelect = ''
        } else {
          getXiaoquList(shequId).then(response => {
            this.xiaoquListSelect = response.data.data
          })
        }
      } else {
        this.xiaoquList = ''
        this.temp.SmallDistrictId = ''
        if (shequId) {
          getXiaoquList(shequId).then(response => {
            this.xiaoquList = response.data.data
          })
        }
      }
    },
    // handleFilter() {
    //   this.listQuery.pageIndex = 1
    //   this.getList()
    // },
    handleFilter(type) {
      // console.log(type)
      if (type === 'search') {
        // if (this.listQuery.smallDistrictId === '' && this.listQuery.state !== '') {
        if (!this.listQuery.smallDistrictId && this.listQuery.state) {
          Message({
            message: '小区为必选项',
            type: 'error',
            duration: 3 * 1000
          })
        } else {
          this.listQuery.pageIndex = 1
          this.getList()
        }
      } else {
        this.listQuery.pageIndex = 1
        this.getList()
      }
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
        delYwhRequest(Id).then(response => {
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
    wuxiaoBtn(row, Id) {
      this.$confirm('确定要置为无效吗?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'delete'
      }).then(() => {
        wuxiaoRequest(Id).then(response => {
          this.getList()
          this.$notify({
            title: '成功',
            message: '已置为无效',
            type: 'success',
            duration: 2000
          })
        })
      }).catch(() => {
      })
    },
    resetTemp() {
      this.temp = {
        RemarkName: '',
        State: '',
        City: '',
        Region: '',
        StreetOfficeId: '',
        CommunityId: '',
        SmallDistrictId: ''
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
      this.shequList = ''
      this.xiaoquList = ''

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
            createYwh(this.temp).then((response) => {
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
      row.State = 'State'
      row.City = 'City'
      row.Region = 'Region'
      row.StreetOfficeId = 'JDB'
      row.CommunityId = 'Shequ'
      row.SmallDistrictId = row.SmallDistrictId
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
            updateYwh(tempData).then((response) => {
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


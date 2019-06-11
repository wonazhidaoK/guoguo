<template>
  <div class="app-container">
    <div class="filter-container">
      <el-input v-model="listQuery.Name" placeholder="门号" style="width: 200px;" class="filter-item" @keyup.enter.native="handleFilter"/>
      <el-select v-model="listQuery.buildingId" placeholder="楼宇" clearable style="width: 90px" class="filter-item" @change="handleFilter(),selectLy(listQuery.buildingId,'Top')">
        <el-option v-for="item in louyulistTop" :key="item.Name" :label="item.Name" :value="item.Id"/>
      </el-select>
      <el-select v-model="listQuery.buildingUnitId" placeholder="单元" clearable style="width: 90px" class="filter-item" @change="handleFilter()">
        <el-option v-for="item in danyuanlistTop" :key="item.UnitName" :label="item.UnitName" :value="item.Id"/>
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
      <el-table-column label="楼宇" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.BuildingName }}</span>
        </template>
      </el-table-column>
      <el-table-column label="单元" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.BuildingUnitName }}</span>
        </template>
      </el-table-column>
      <el-table-column label="门号" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.Name }}</span>
        </template>
      </el-table-column>
      <el-table-column label="层数" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.NumberOfLayers }}</span>
        </template>
      </el-table-column>
      <el-table-column label="面积" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.Acreage }}</span>
        </template>
      </el-table-column>
      <el-table-column label="朝向" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.Oriented }}</span>
        </template>
      </el-table-column>
      <el-table-column :label="$t('table.actions')" align="center" class-name="small-padding fixed-width" min-width="140%">
        <template slot-scope="scope">
          <el-button size="mini" type="success" style="width:100px" @click="handleUSerDetail(scope.row)">业主信息</el-button>
          <el-button type="primary" size="mini" @click="handleUpdate(scope.row)">{{ $t('table.edit') }}</el-button>
          <el-button v-if="scope.row.status!='deleted'" size="mini" type="danger" @click="delYehuBtn(scope.row, scope.row.Id)">删除
          </el-button>
        </template>
      </el-table-column>
    </el-table>

    <pagination v-show="total>0" :total="total" :page.sync="listQuery.pageIndex" :limit.sync="listQuery.pageSize" @pagination="getList" />

    <el-dialog :title="textMap[dialogStatus]" :visible.sync="dialogFormVisible">
      <el-form ref="dataForm" :rules="rules" :model="temp" label-position="left" label-width="120px" style="width: 100%;box-sizing:border-box; padding:0 50px;">
        <el-form-item v-show="dialogStatus==='create'" label="楼宇" prop="BuildingId">
          <el-select v-model="temp.BuildingId" class="filter-item" placeholder="楼宇" @change="selectLy(temp.BuildingId,'Add')">
            <el-option v-for="item in louyulistAdd" :key="item.Name" :label="item.Name" :value="item.Id" :disabled="temp.Id!=''&&temp.Id!=null"/>
          </el-select>
        </el-form-item>
        <el-form-item v-show="dialogStatus==='create'" label="单元" prop="BuildingUnitId">
          <el-select v-model="temp.BuildingUnitId" class="filter-item" placeholder="单元">
            <el-option v-for="item in danyuanlistAdd" :key="item.UnitName" :label="item.UnitName" :value="item.Id" :disabled="temp.Id!=''&&temp.Id!=null"/>
          </el-select>
        </el-form-item>
        <el-form-item label="业户门号" prop="Name">
          <el-input v-model="temp.Name"/>
        </el-form-item>
        <el-form-item v-show="dialogStatus==='create'" label="层数" prop="NumberOfLayers">
          <el-input v-model="temp.NumberOfLayers" :disabled="temp.Id!=''&&temp.Id!=null"/>
        </el-form-item>
        <el-form-item v-show="dialogStatus==='create'" label="面积" prop="Acreage">
          <el-input v-model="temp.Acreage" :disabled="temp.Id!=''&&temp.Id!=null"/>
        </el-form-item>
        <el-form-item v-show="dialogStatus==='create'" label="朝向" prop="Oriented">
          <el-input v-model="temp.Oriented" :disabled="temp.Id!=''&&temp.Id!=null"/>
        </el-form-item>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="dialogFormVisible = false">{{ $t('table.cancel') }}</el-button>
        <el-button v-loading="createLoading" type="primary" @click="dialogStatus==='create'?createData():updateData()">{{ $t('table.confirm') }}</el-button>
      </div>
    </el-dialog>

    <!-- 楼宇 单元信息 -->
    <el-dialog :visible.sync="dialogFormVisibleUserMesDetailList" title="业主信息">
      <el-input v-model="listQueryUser.name" placeholder="姓名" style="width: 150px;" class="filter-item" @keyup.enter.native="handleUSerDetail"/>
      <el-input v-model="listQueryUser.gender" placeholder="性别" style="width: 150px;" class="filter-item" @keyup.enter.native="handleUSerDetail"/>
      <el-input v-model="listQueryUser.phoneNumber" placeholder="手机号" style="width: 150px;" class="filter-item" @keyup.enter.native="handleUSerDetail"/>
      <el-input v-model="listQueryUser.iDCard" placeholder="身份证" style="width: 150px;" class="filter-item" @keyup.enter.native="handleUSerDetail"/>
      <el-button v-waves class="filter-item" type="primary" icon="el-icon-search" @click="handleUSerDetail">{{ $t('table.search') }}</el-button>
      <el-button class="filter-item" style="margin-left: 10px;" type="primary" icon="el-icon-edit" @click="handleCreateUser">{{ $t('table.register') }}</el-button>
      <el-table
        v-loading="listLoadingLy"
        :key="tableKeyLy"
        :data="listUser"
        border
        fit
        highlight-current-row
        style="width: 100%;margin-top:20px;">
        <el-table-column label="姓名" align="center">
          <template slot-scope="scope">
            <span>{{ scope.row.Name }}</span>
          </template>
        </el-table-column>
        <el-table-column label="生日" align="center">
          <template slot-scope="scope">
            <span>{{ scope.row.Birthday }}</span>
          </template>
        </el-table-column>
        <el-table-column label="性别" align="center">
          <template slot-scope="scope">
            <span>{{ scope.row.Gender }}</span>
          </template>
        </el-table-column>
        <el-table-column label="手机号" align="center">
          <template slot-scope="scope">
            <span>{{ scope.row.PhoneNumber }}</span>
          </template>
        </el-table-column>
        <el-table-column label="身份证" align="center">
          <template slot-scope="scope">
            <span>{{ scope.row.IDCard }}</span>
          </template>
        </el-table-column>
        <el-table-column label="是否认证" align="center">
          <template slot-scope="scope">
            <el-tag v-show="scope.row.IsLegalize" type="warning">已认证</el-tag>
            <el-tag v-show="!scope.row.IsLegalize" type="danger">未认证</el-tag>
          </template>
        </el-table-column>
        <el-table-column :label="$t('table.actions')" align="center" class-name="small-padding fixed-width">
          <template slot-scope="scope">
            <el-button type="primary" size="mini" @click="handleUpdateUser(scope.row)">{{ $t('table.edit') }}</el-button>
            <el-button v-show="!scope.row.IsLegalize" size="mini" type="danger" @click="delUserBtn(scope.row, scope.row.Id)">删除
            </el-button>
          </template>
        </el-table-column>
      </el-table>
      <pagination v-show="totalUser>0" :total="totalUser" :page.sync="listQueryUser.pageIndex" :limit.sync="listQueryUser.pageSize" @pagination="getListUserMes" />
      <span slot="footer" class="dialog-footer">
        <el-button type="primary" @click="dialogFormVisibleUserMesDetailList = false">{{ $t('table.confirm') }}</el-button>
      </span>
    </el-dialog>
    <el-dialog :title="textMap[dialogStatusUserMes]" :visible.sync="dialogFormVisibleUserMesDetail">
      <el-form ref="dataFormUser" :rules="ruleUserMes" :model="tempUserMes" label-position="left" label-width="120px" style="width: 100%;box-sizing:border-box; padding:0 50px;">
        <el-form-item v-show="dialogStatusUserMes==='create'||!tempUserMes.IsLegalize" label="姓名" prop="Name">
          <el-input v-model="tempUserMes.Name" :disabled="tempUserMes.IsLegalize"/>
        </el-form-item>
        <el-form-item v-show="dialogStatusUserMes==='create'||!tempUserMes.IsLegalize" label="生日" prop="Birthday">
          <el-date-picker v-model="tempUserMes.Birthday" :disabled="tempUserMes.IsLegalize" type="date" format="yyyy-MM-dd" value-format="yyyy-MM-dd" class="filter-item" placeholder="生日"/>
        </el-form-item>
        <el-form-item v-show="dialogStatusUserMes==='create'||!tempUserMes.IsLegalize" label="性别" prop="Gender">
          <el-select v-model="tempUserMes.Gender" class="filter-item" placeholder="性别">
            <el-option v-for="item in GenderList" :key="item.Name" :label="item.Name" :value="item.Name" :disabled="tempUserMes.IsLegalize" class="filter-item"/>
          </el-select>
        </el-form-item>
        <el-form-item label="手机号" prop="PhoneNumber">
          <el-input v-model="tempUserMes.PhoneNumber"/>
        </el-form-item>
        <el-form-item v-show="dialogStatusUserMes==='create'||!tempUserMes.IsLegalize" label="身份证" prop="IDCard">
          <el-input v-model="tempUserMes.IDCard" :disabled="tempUserMes.IsLegalize"/>
        </el-form-item>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="dialogFormVisibleUserMesDetail = false">{{ $t('table.cancel') }}</el-button>
        <el-button v-loading="createLoading" type="primary" @click="dialogStatusUserMes==='create'?createDataUser():updateYezhuUser()">{{ $t('table.confirm') }}</el-button>
      </div>
    </el-dialog>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
import { getListLy, getListDy, createYehuUser, fetchListYehu, updateLy, delYehu, fetchUserMesList, updateUser, createYezhuUser, delUser } from '@/api/wySelectList'
import waves from '@/directive/waves' // Waves directive
import Pagination from '@/components/Pagination' // Secondary package based on el-pagination

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
    }
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
    var checkNumber = (rule, value, callback) => {
      if (!value) {
        return callback(new Error('层数不能为空'))
      } else {
        const reg = /^-?[1-9]\d*$/
        // console.log(reg.test(value))
        if (reg.test(value)) {
          callback()
        } else {
          return callback(new Error('层数必须是数字'))
        }
      }
    }
    var checkCar = (rule, value, callback) => {
      if (value && (!(/\d{17}[\d|x]|\d{15}/).test(value) || (value.length !== 15 && value.length !== 18))) {
        return callback(new Error('身份证号码不符合规范'))
      } else {
        callback()
      }
    }
    return {
      GenderList: [
        {
          Name: '男',
          Id: 0
        }, {
          Name: '女',
          Id: 1
        }
      ],
      tableKey: 0,
      tableKeyLy: 0,
      list: null,
      listUser: null,
      total: 0,
      totalUser: 0,
      listLoading: true,
      listQuery: {
        pageIndex: 1,
        pageSize: 20
      },
      listQueryUser: {
        pageIndex: 1,
        pageSize: 20
      },
      importanceOptions: [1, 2, 3],
      sortOptions: [{ label: 'ID Ascending', key: '+id' }, { label: 'ID Descending', key: '-id' }],
      statusOptions: ['published', 'draft', 'deleted'],
      showReviewer: false,
      temp: {
        Name: '',
        BuildingId: '',
        BuildingUnitId: '',
        NumberOfLayers: '',
        Acreage: '',
        Oriented: ''
      },
      tempUserMes: {
        Name: '',
        Birthday: '',
        Gender: '',
        PhoneNumber: '',
        IDCard: ''
      },
      dialogFormVisible: false,
      dialogStatus: '',
      textMap: {
        update: '编辑业户信息',
        create: '新建业户信息'
      },
      dialogPvVisible: false,
      pvData: [],
      rules: {
        BuildingId: [{ required: true, message: '楼宇不能为空', trigger: 'change' }],
        BuildingUnitId: [{ required: true, message: '单元不能为空', trigger: 'change' }],
        Name: [{ required: true, message: '业户门号是必填的', trigger: 'blur' }],
        NumberOfLayers: [{ required: true, validator: checkNumber, trigger: 'blur' }],
        Acreage: [{ required: true, message: '面积是必填的', trigger: 'blur' }],
        Oriented: [{ required: true, message: '朝向是必填的', trigger: 'blur' }]
      },
      ruleUserMes: {
        Name: [{ required: true, message: '姓名是必填的', trigger: 'blur' }],
        Birthday: [{ required: true, message: '生日是必填的', trigger: 'blur' }],
        Gender: [{ required: true, message: '性别是必填的', trigger: 'change' }],
        PhoneNumber: [{ required: true, validator: checkPhone, trigger: 'blur' }],
        IDCard: [{ required: true, validator: checkCar, trigger: 'blur' }]
      },
      dialogFormVisibleUserMesDetail: false,
      dialogStatusUserMes: 'create',
      louyulistTop: '',
      louyulistAdd: '',
      danyuanlistTop: '',
      danyuanlistAdd: '',
      tempUserMesId: '',
      dialogFormVisibleUserMesDetailList: false,
      listLoadingLy: true,
      createLoading: false,
      num: 0
    }
  },
  computed: {
    ...mapGetters([
      'loginuser'
    ])
  },
  created() {
    this.getList()
    // this.getSheng()
    this.getLouyu()
  },
  methods: {
    getList() {
      this.listLoading = true
      fetchListYehu(this.listQuery).then(response => {
        this.list = response.data.data.List
        // console.log(this.list)
        this.total = response.data.data.TotalCount
        setTimeout(() => {
          this.listLoading = false
        }, 1.5 * 1000)
      })
    },
    getLouyu() {
      const smallId = this.$store.getters.loginuser.SmallDistrictId
      getListLy(smallId).then(response => {
        this.louyulistAdd = response.data.data
        this.louyulistTop = response.data.data
      })
    },
    handleUSerDetail(row) {
      // console.log(row.Id)
      if (row.Id !== undefined) {
        this.tempUserMesId = row.Id
        this.listQueryUser.industryId = row.Id
      }
      this.getListUserMes()
    },
    getListUserMes() {
      this.dialogFormVisibleUserMesDetailList = true
      fetchUserMesList(this.listQueryUser).then(response => {
        this.listUser = response.data.data.List
        setTimeout(() => {
          this.listLoadingLy = false
        }, 1.5 * 1000)
      })
    },
    // 切换楼宇
    selectLy(BuildingId, type) {
      if (BuildingId === '') {
        if (type === 'Top') {
          this.listQuery.buildingUnitId = ''
          this.danyuanlistTop = ''
        } else {
          this.temp.BuildingUnitId = ''
          this.danyuanlistAdd = ''
        }
      } else {
        // 获取单元单元
        getListDy(BuildingId).then(response => {
          if (type === 'Top') {
            this.listQuery.buildingUnitId = ''
            this.danyuanlistTop = response.data.data
          } else {
            this.temp.BuildingUnitId = ''
            this.danyuanlistAdd = response.data.data
          }
        })
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
    // 删除业户
    delYehuBtn(row, Id) {
      this.$confirm('确定要删除吗?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'delete'
      }).then(() => {
        delYehu(Id).then(response => {
          this.getList()
          this.$notify({
            title: '成功',
            message: '删除成功',
            type: 'success',
            duration: 2000
          })
        })
      }).catch(() => {
      })
    },
    delUserBtn(row, Id) {
      this.$confirm('确定要删除吗?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'delete'
      }).then(() => {
        delUser(Id).then(response => {
          this.getListUserMes()
          this.$notify({
            title: '成功',
            message: '删除成功',
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
        BuildingId: '',
        BuildingUnitId: '',
        NumberOfLayers: '',
        Acreage: '',
        Oriented: ''
      }
      this.tempUserMes = {
        Name: '',
        Birthday: '',
        Gender: '',
        PhoneNumber: '',
        IDCard: ''
      }
    },
    // 登记按钮 业户
    handleCreate() {
      this.resetTemp()
      this.dialogStatus = 'create'
      this.dialogFormVisible = true
      this.$nextTick(() => {
        this.$refs['dataForm'].clearValidate()
      })
    },
    // 登记按钮 业主
    handleCreateUser() {
      this.resetTemp()
      this.dialogStatusUserMes = 'create'
      this.dialogFormVisibleUserMesDetail = true
      this.$nextTick(() => {
        this.$refs['dataFormUser'].clearValidate()
      })
    },
    // 新建 业户
    createData() {
      this.$refs['dataForm'].validate((valid) => {
        if (valid) {
          this.num = this.num + 1
          if (this.num < 2) {
            this.createLoading = true
            createYehuUser(this.temp).then((response) => {
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
    // 新建业主
    createDataUser() {
      this.$refs['dataFormUser'].validate((valid) => {
        if (valid) {
          this.tempUserMes.IndustryId = this.tempUserMesId
          this.num = this.num + 1
          if (this.num < 2) {
            this.createLoading = true
            createYezhuUser(this.tempUserMes).then((response) => {
              // console.log(response.data)
              this.getListUserMes()
              this.$notify({
                title: '成功',
                message: '创建成功',
                type: 'success',
                duration: 2000
              })
              this.dialogFormVisibleUserMesDetail = false
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

      // 编辑街道办
      this.temp = Object.assign({}, row) // copy obj
      // this.temp.timestamp = new Date(this.temp.timestamp)
      this.dialogStatus = 'update'
      this.dialogFormVisible = true
      this.$nextTick(() => {
        this.$refs['dataForm'].clearValidate()
      })
    },
    handleUpdateUser(row) {
      // console.log(row)
      // 编辑街道办
      this.tempUserMes = Object.assign({}, row) // copy obj
      this.dialogStatusUserMes = 'update'
      this.dialogFormVisibleUserMesDetail = true
      this.$nextTick(() => {
        this.$refs['dataFormUser'].clearValidate()
      })
    },
    updateData() {
      this.$refs['dataForm'].validate((valid) => {
        if (valid) {
          const tempData = Object.assign({}, this.temp)
          this.num = this.num + 1
          if (this.num < 2) {
            this.createLoading = true
            updateLy(tempData).then((response) => {
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
    },
    updateYezhuUser() {
      this.$refs['dataFormUser'].validate((valid) => {
        if (valid) {
          const tempData = Object.assign({}, this.tempUserMes)
          this.num = this.num + 1
          if (this.num < 2) {
            this.createLoading = true
            updateUser(tempData).then((response) => {
              // console.log(response.data)
              this.getListUserMes()
              this.$notify({
                title: '成功',
                message: '编辑成功',
                type: 'success',
                duration: 2000
              })
              this.dialogFormVisibleUserMesDetail = false
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


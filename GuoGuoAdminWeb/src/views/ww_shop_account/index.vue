<template>
  <div class="app-container">
    <div class="filter-container">
      <el-input v-model="listQuery.Name" placeholder="用户名" style="width: 200px;" class="filter-item" @keyup.enter.native="handleFilter"/>
      <el-input v-model="listQuery.PhoneNumber" placeholder="手机号" style="width: 200px;" class="filter-item" @keyup.enter.native="handleFilter"/>
      <el-select v-model="listQuery.ShopId" placeholder="所属商铺" clearable style="width: 200px" class="filter-item" @change="handleFilter()">
        <el-option v-for="item in typeList" :key="item.Name" :label="item.Name" :value="item.Id"/>
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
      <el-table-column label="用户名" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.Name }}</span>
        </template>
      </el-table-column>
      <el-table-column label="账号" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.Account }}</span>
        </template>
      </el-table-column>
      <el-table-column label="手机号" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.PhoneNumber }}</span>
        </template>
      </el-table-column>
      <el-table-column label="所属商铺" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.ShopName }}</span>
        </template>
      </el-table-column>
      <el-table-column label="角色" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.RoleName }}</span>
        </template>
      </el-table-column>
      <el-table-column label="创建时间" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.CreateTime }}</span>
        </template>
      </el-table-column>
      <el-table-column :label="$t('table.actions')" align="center" class-name="small-padding fixed-width">
        <template slot-scope="scope">
          <el-button type="primary" size="mini" @click="handleUpdate(scope.row, scope.row.Id)">{{ $t('table.edit') }}</el-button>
          <el-button v-if="scope.row.status!='deleted'" size="mini" type="danger" @click="delJdbBtn(scope.row, scope.row.Id)">{{ $t('table.logoutBtn') }}
          </el-button>
        </template>
      </el-table-column>
    </el-table>

    <pagination v-show="total>0" :total="total" :page.sync="listQuery.pageIndex" :limit.sync="listQuery.pageSize" @pagination="getList"/>

    <el-dialog :title="textMap[dialogStatus]" :visible.sync="dialogCreatShow">
      <el-form ref="dataForm" :rules="rules" :model="temp" label-position="left" label-width="120px" style="width: 100%;box-sizing:border-box; padding:0 50px;">

        <el-form-item v-show="dialogStatus==='create'" label="用户名" prop="Name">
          <el-input v-model="temp.Name"/>
        </el-form-item>
        <el-form-item label="手机号" prop="PhoneNumber">
          <el-input v-model="temp.PhoneNumber"/>
        </el-form-item>
        <el-form-item v-show="dialogStatus==='create'" label="账号" prop="Account">
          <el-input v-model="temp.Account"/>
        </el-form-item>
        <el-form-item label="密码" prop="Password">
          <el-input v-model="temp.Password"/>
        </el-form-item>
        <el-form-item v-show="dialogStatus==='create'" label="商家" prop="ShopId">
          <el-select v-model="temp.ShopId" placeholder="商家" style="width:100%">
            <el-option v-for="item in typeList" :key="item.Name" :label="item.Name" :value="item.Id"/>
          </el-select>
        </el-form-item>
        <el-form-item :label="$t('table.Role')" prop="RoleId">
          <el-select v-model="temp.RoleId" placeholder="角色" style="width:100%">
            <el-option v-for="item in roleList" :key="item.Name" :label="item.Name" :value="item.Id"/>
          </el-select>
        </el-form-item>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="dialogCreatShow = false">{{ $t('table.cancel') }}</el-button>
        <el-button v-loading="createLoading" type="primary" @click="dialogStatus==='create'?createData():updateData()">{{ $t('table.confirm') }}</el-button>
      </div>
    </el-dialog>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
import { fetchShopUserList, createShopUser, updateShopUser, fetchTShopSelectList, delShopUserRequest, fetRoleList } from '@/api/wwShop'
import waves from '@/directive/waves' // Waves directive
import Pagination from '@/components/Pagination' // Secondary package based on el-pagination
import Dropzone from '@/components/Dropzone'

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

export default {
  name: 'ComplexTable',
  components: { Pagination, Dropzone },
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
      dialogDetailVisible: false,
      detailCon: [],
      listLoading: true,
      listQuery: {
        pageIndex: 1,
        pageSize: 20
      },
      calendarTypeOptions,
      temp: {
        Name: '',
        ShopId: '',
        PhoneNumber: '',
        Password: '',
        // Address: '',
        RoleId: '',
        Account: ''
        // QualificationImageId: '',
        // LogoImageId: ''
      },
      dialogCreatShow: false,
      dialogStatus: '',
      textMap: {
        update: '编辑商铺--用户账号',
        create: '新建商铺--用户账号'
      },
      rules: {
        Name: [{ required: true, message: '用户名是必填的', trigger: 'blur' }],
        ShopId: [{ required: true, message: '类别是必选的', trigger: 'change' }],
        Password: [{ required: true, message: '密码是必填的', trigger: 'blur' }],
        // Address: [{ required: true, message: '地址是必填的', trigger: 'blur' }],
        RoleId: [{ required: true, message: '角色是必选的', trigger: 'change' }],
        PhoneNumber: [{ required: true, trigger: 'blur', validator: checkPhone }],
        Account: [{ required: true, trigger: 'blur', validator: checkAccount }]
      },
      typeList: [],
      createLoading: false,
      num: 0,
      roleList: []
    }
  },
  computed: {
    ...mapGetters([
      'name',
      'avatar',
      'roles',
      'province'
    ])
    // this.$store.getters.province
  },
  created() {
    this.getList()
    this.getType()
    this.getRoleList()
    this.rootUrl = process.env.API_HOST
  },
  methods: {
    getRoleList() {
      fetRoleList().then(response => {
        // console.log(response.data.data)
        this.roleList = response.data.data
      })
    },
    getList() {
      this.listLoading = true
      fetchShopUserList(this.listQuery).then(response => {
        this.list = response.data.data.List
        this.total = response.data.data.TotalCount
        setTimeout(() => {
          this.listLoading = false
        }, 1.5 * 1000)
      })
    },
    getType() {
      fetchTShopSelectList().then(response => {
        this.typeList = response.data.data
      })
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
    // 删除商铺
    delJdbBtn(row, Id) {
      this.$confirm('确定要注销吗?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'delete'
      }).then(() => {
        delShopUserRequest(Id).then(response => {
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
        ShopId: '',
        PhoneNumber: '',
        Password: '',
        // Address: '',
        RoleId: '',
        Account: ''
        // QualificationImageId: '',
        // LogoImageId: ''
      }
    },
    // 登记商铺 按钮
    handleCreate() {
      this.createLoading = false
      this.resetTemp()
      this.dialogStatus = 'create'
      this.dialogCreatShow = true
      this.$nextTick(() => {
        this.$refs['dataForm'].clearValidate()
      })
    },
    // 新建商铺
    createData() {
      this.$refs['dataForm'].validate((valid) => {
        if (valid) {
          this.num = this.num + 1
          if (this.num < 2) {
            this.createLoading = true
            createShopUser(this.temp).then((response) => {
              // console.log(response.data)
              this.getList()
              this.$notify({
                title: '成功',
                message: '创建成功',
                type: 'success',
                duration: 2000
              })
              this.dialogCreatShow = false
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
    handleUpdate(row, Id) {
      this.createLoading = false
      this.temp = Object.assign({}, row) // copy obj
      // this.temp.timestamp = new Date(this.temp.timestamp)
      this.dialogStatus = 'update'
      this.dialogCreatShow = true
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
            updateShopUser(tempData).then(() => {
              this.getList()
              this.$notify({
                title: '成功',
                message: '编辑成功',
                type: 'success',
                duration: 2000
              })
              this.dialogCreatShow = false
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

<style scoped>
.shopDetail{
  padding: 0;
  margin: 0;
}
.shopDetail>li{
  list-style: none;
  border-bottom: 1px solid #ddd;
  font-size: 16px;
  box-sizing: border-box;
}
.shopDetail>li>div{
  width: 15%;
  float: left;
  padding: 10px;
}
.shopDetail>li>div:last-child{
  width: 70%;
}
.shopDetail>li>div img{
  width: 100%;
}
 .clearfloat:after{display:block;clear:both;content:"";visibility:hidden;height:0}
 .clearfloat{zoom:1}
</style>

<template>
  <div class="app-container">
    <div class="filter-container">
      <el-input v-model="listQuery.name" placeholder="角色名称" style="width: 200px;" class="filter-item" @keyup.enter.native="handleFilter"/>
      <el-select v-model="listQuery.departmentValue" placeholder="发起部门" clearable style="width: 150px" class="filter-item" @change="handleFilter()">
        <el-option v-for="item in bmlist" :key="item.Name" :label="item.Name" :value="item.Value"/>
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
      <el-table-column label="角色名称" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.Name }}</span>
        </template>
      </el-table-column>
      <el-table-column label="发起部门" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.DepartmentName }}</span>
        </template>
      </el-table-column>
      <el-table-column label="角色描述" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.Description }}</span>
        </template>
      </el-table-column>
      <el-table-column :label="$t('table.actions')" align="center" class-name="small-padding fixed-width">
        <template slot-scope="scope">
          <!-- <el-button type="primary" size="mini" @click="handleUpdate(scope.row)">{{ $t('table.edit') }}</el-button> -->
          <el-button type="primary" style="width:80px;" size="mini" @click="addRole(scope.row)">权限管理</el-button>
          <el-button v-if="scope.row.status!='deleted'" size="mini" type="danger" @click="delBtn(scope.row, scope.row.Id)">{{ $t('table.logoutBtn') }}
          </el-button>
        </template>
      </el-table-column>
    </el-table>
    <pagination v-show="total>0" :total="total" :page.sync="listQuery.pageIndex" :limit.sync="listQuery.pageSize" @pagination="getList" />

    <el-dialog :title="textMap[dialogStatus]" :visible.sync="dialogFormVisible">
      <el-form ref="dataForm" :rules="rules" :model="temp" label-position="left" label-width="120px" style="width: 100%;box-sizing:border-box; padding:0 50px;">
        <el-form-item label="角色名称" prop="Name">
          <el-input v-model="temp.Name" placeholder="请输入投诉类型名称"/>
        </el-form-item>
        <el-form-item label="角色描述" prop="Description">
          <el-input v-model="temp.Description" placeholder="请输入投诉描述"/>
        </el-form-item>
        <el-form-item :label="$t('table.Bumen')" prop="DepartmentValue">
          <el-select v-model="temp.DepartmentValue" class="filter-item" placeholder="所属部门">
            <el-option v-for="item in bmlist" :key="item.Value" :label="item.Name" :value="item.Value"/>
          </el-select>
        </el-form-item>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="dialogFormVisible = false">{{ $t('table.cancel') }}</el-button>
        <el-button v-loading="createLoading" type="primary" @click="dialogStatus==='create'?createData():updateData()">{{ $t('table.confirm') }}</el-button>
      </div>
    </el-dialog>

    <el-dialog :visible.sync="dialogPvVisible" title="用户权限管理" style="padding:0;margin:0">
      <div class="components-container" style="padding:0;margin:0">
        <div class="editor-container" style="padding:0;margin:0">
          <dnd-list :list1="list1" :list2="list2" :roleid="roleId" list1-title="已有权限" list2-title="未有权限"/>
        </div>
      </div>
      <!-- <span slot="footer" class="dialog-footer">
        <el-button type="primary" @click="dialogPvVisible = false">{{ $t('table.confirm') }}</el-button>
      </span> -->
    </el-dialog>

  </div>
</template>

<script>
import DndList from '@/components/DndList'
import { fetchList, createRole, update, delRequest, fetchListBm, getRoleMenu, getMenu } from '@/api/role'
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

// object 转 string
const object2String = obj => {
  return JSON.stringify(obj)
}

export default {
  name: 'ComplexTable',
  components: { Pagination, DndList },
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
    object2String
  },
  data() {
    return {
      tableKey: 0,
      list1: [],
      list2: [],
      list: null,
      total: 0,
      listLoading: true,
      listQuery: {
        departmentValue: '',
        name: ''
      },
      importanceOptions: [1, 2, 3],
      calendarTypeOptions,
      sortOptions: [{ label: 'ID Ascending', key: '+id' }, { label: 'ID Descending', key: '-id' }],
      statusOptions: ['published', 'draft', 'deleted'],
      showReviewer: false,
      temp: {
        Name: '',
        Description: '',
        DepartmentValue: ''
      },
      levelList: [1, 2, 3, 4, 5, 6, 7, 8],
      dialogFormVisible: false,
      dialogStatus: '',
      textMap: {
        update: '编辑角色',
        create: '新建角色'
      },
      dialogPvVisible: false,
      pvData: [],
      rules: {
        Name: [{ required: true, message: '角色名称是必填的', trigger: 'blur' }],
        Description: [{ required: true, message: '角色描述是必填的', trigger: 'blur' }],
        DepartmentValue: [{ required: true, message: '部门是必选的', trigger: 'change' }]
      },
      bmlist: [],
      roleId: '',
      createLoading: false,
      num: 0
    }
  },
  created() {
    this.getList()
    this.getBumen()
  },
  methods: {
    getList() {
      this.listLoading = true
      // console.log(this.listQuery)
      fetchList(this.listQuery).then(response => {
        // this.list = response.data.data
        this.list = response.data.data.List
        this.total = response.data.data.TotalCount
        setTimeout(() => {
          this.listLoading = false
        }, 1.5 * 1000)
      })
    },
    getRoleMenu(id, DepartmentValue) {
      this.roleId = id
      getRoleMenu(id, DepartmentValue).then(response => {
        this.list1 = response.data.data

        getMenu(DepartmentValue).then(response => {
          // console.log(this.list1)
          const list2 = response.data.data
          const ids = []
          this.list1.filter(item => {
            ids.push(item.MenuId)
            item.Id = item.MenuId
          })
          this.list2 = list2.filter(item => ids.indexOf(item.Id) === -1)
        })
      })
    },
    addRole(row) {
      // console.log(row)
      this.list1 = []
      this.list2 = []
      this.dialogPvVisible = true

      this.getRoleMenu(row.Id, row.DepartmentValue)
    },
    getBumen() {
      fetchListBm().then(response => {
        this.bmlist = response.data.data
        // console.log(this.bmlist)
      })
    },
    handleFilter() {
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
    delBtn(row, Id) {
      this.$confirm('确定要注销吗?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'delete'
      }).then(() => {
        delRequest(Id).then(response => {
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
        Description: '',
        DepartmentValue: ''
      }
    },
    // 登记 按钮
    handleCreate() {
      this.resetTemp()
      this.dialogStatus = 'create'
      this.dialogFormVisible = true
      this.$nextTick(() => {
        this.$refs['dataForm'].clearValidate()
      })
    },
    // 新建
    createData() {
      // console.log(this.temp.DepartmentValue)
      this.$refs['dataForm'].validate((valid) => {
        if (valid) {
          this.num = this.num + 1
          if (this.num < 2) {
            this.createLoading = true
            createRole(this.temp).then((response) => {
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
      var bm = {
        'Name': row.InitiatingDepartmentName,
        'Value': row.InitiatingDepartmentValue
      }
      row.Bumen = JSON.stringify(bm)
      this.temp = Object.assign({}, row) // copy obj

      this.dialogStatus = 'update'
      this.dialogFormVisible = true
      this.$nextTick(() => {
        this.$refs['dataForm'].clearValidate()
      })
    },
    updateData() {
      // console.log(this.temp)
      this.$refs['dataForm'].validate((valid) => {
        if (valid) {
          const tempData = Object.assign({}, this.temp)
          // console.log(tempData)
          update(tempData).then(() => {
            this.getList()
            this.dialogFormVisible = false
            this.$notify({
              title: '成功',
              message: '编辑成功',
              type: 'success',
              duration: 2000
            })
          })
        }
      })
    }
  }
}
</script>


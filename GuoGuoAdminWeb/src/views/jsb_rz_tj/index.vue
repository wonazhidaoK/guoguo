<template>
  <div class="app-container">
    <div class="filter-container">
      <!-- <el-input v-model="listQuery.title" placeholder="标题" style="width: 200px;" class="filter-item" @keyup.enter.native="handleFilter"/> -->
      <!-- <el-select v-model="listQuery.smallDistrictId" class="filter-item" placeholder="小区" @change="handleFilter">
        <el-option v-for="item in smalllist" :key="item.Name" :label="item.Name" :value="item.Id"/>
      </el-select>
      <el-button v-waves class="filter-item" type="primary" icon="el-icon-search" @click="handleFilter">{{ $t('table.search') }}</el-button> -->
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
      <el-table-column label="标题" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.Title }}</span>
        </template>
      </el-table-column>
      <el-table-column label="描述" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.Description }}</span>
        </template>
      </el-table-column>
      <el-table-column label="类型" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.TypeName }}</span>
        </template>
      </el-table-column>
      <!-- <el-table-column :label="$t('table.actions')" align="center" class-name="small-padding fixed-width">
        <template slot-scope="scope">
          <el-button size="mini" type="primary" @click="voteDetail(scope.row, scope.row.Id)">详情</el-button>
        </template> -->
      <!-- </el-table-column> -->
    </el-table>

    <pagination v-show="total>0" :total="total" :page.sync="listQuery.pageIndex" :limit.sync="listQuery.pageSize" @pagination="getList" />

    <el-dialog :title="textMap[dialogStatus]" :visible.sync="dialogFormVisible">
      <el-form ref="dataForm" :rules="rules" :model="temp" label-position="left" label-width="120px" style="width: 100%;box-sizing:border-box; padding:0 50px;">
        <el-form-item label="标题 " prop="Title">
          <el-input v-model="temp.Title" :disabled="temp.Id!=''&&temp.Id!=null"/>
        </el-form-item>
        <el-form-item label="描述" prop="Description">
          <el-input v-model="temp.Description" :disabled="temp.Id!=''&&temp.Id!=null"/>
        </el-form-item>
        <el-form-item label="类型" prop="TypeValue">
          <el-select v-model="temp.TypeValue" class="filter-item" placeholder="类型">
            <el-option v-for="item in typelist" :key="item.Name" :label="item.Name" :value="item.Id"/>
          </el-select>
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
import { createTiaojian, fetchList } from '@/api/jdbTiaojian'
import waves from '@/directive/waves' // Waves directive
import Pagination from '@/components/Pagination' // Secondary package based on el-pagination
import Dropzone from '@/components/Dropzone'

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
    }
  },
  data() {
    return {
      typelist: [
        {
          Id: 'Image',
          Name: '图片'
        }, {
          Id: 'Text',
          Name: '文字'
        }
      ],
      activeNames: ['1'],
      tableKey: 0,
      list: null,
      total: 0,
      listLoading: true,
      listQuery: {
        pageIndex: 1,
        pageSize: 20
      },
      temp: {
        Title: '',
        Description: '',
        Content: ''
      },
      dialogFormVisible: false,
      dialogStatus: '',
      textMap: {
        update: '编辑高级认证条件',
        create: '新建高级认证条件'
      },
      rules: {
        Title: [{ required: true, message: '标题是必填的', trigger: 'blur' }],
        Description: [{ required: true, message: '摘要是必填的', trigger: 'blur' }],
        Content: [{ required: true, message: '内容是必填的', trigger: 'blur' }],
        TypeValue: [{ required: true, message: '小区是必填的', trigger: 'change' }]
      },
      voteDetailCon: '',
      dialogPvVisible: false,
      smalllist: '',
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
  },
  methods: {
    handleChange(val) {
      // console.log(val)
    },
    getList() {
      this.listLoading = true
      fetchList().then(response => {
        this.list = response.data.data
        // console.log(this.list)
        // this.total = response.data.data.TotalCount
        setTimeout(() => {
          this.listLoading = false
        }, 1.5 * 1000)
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
    resetTemp() {
      this.temp = {
        Title: '',
        Description: '',
        Content: '',
        TypeValue: []
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
      this.$refs['dataForm'].validate((valid) => {
        if (valid) {
          this.num = this.num + 1
          if (this.num < 2) {
            this.createLoading = true
            createTiaojian(this.temp).then((response) => {
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
      // 编辑
      this.temp = Object.assign({}, row) // copy obj
      // this.temp.timestamp = new Date(this.temp.timestamp)
      this.dialogStatus = 'update'
      this.dialogFormVisible = true
      this.$nextTick(() => {
        this.$refs['dataForm'].clearValidate()
      })
    }
  }
}
</script>


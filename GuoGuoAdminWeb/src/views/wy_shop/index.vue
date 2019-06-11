<template>
  <div class="app-container">
    <div class="filter-container">
      <el-select v-model="listQuery.shopId" placeholder="平台商户" clearable style="width: 120px" class="filter-item" @change="handleFilter()">
        <el-option v-for="item in shopList" :key="item.Id" :label="item.Name" :value="item.Id" />
      </el-select>
      <el-button v-waves class="filter-item" type="primary" icon="el-icon-search" @click="handleFilter">{{ $t('table.search') }}</el-button>
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
      <el-table-column label="商家名称" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.Name }}</span>
        </template>
      </el-table-column>
      <el-table-column label="排序" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.Sort }}</span>
        </template>
      </el-table-column>
      <el-table-column label="创建时间" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.CreateTime }}</span>
        </template>
      </el-table-column>
      <el-table-column label="邮费" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.Postage }}</span>
        </template>
      </el-table-column>
      <el-table-column :label="$t('table.actions')" align="center" class-name="small-padding fixed-width">
        <template slot-scope="scope">
          <el-button type="primary" size="mini" @click="handleUpdate(scope.row)">{{ $t('table.edit') }}</el-button>
          <el-button v-if="scope.row.status!='deleted'" size="mini" type="danger" @click="delSmallDistrictShopBtn(scope.row, scope.row.Id)">
            {{ $t('table.logoutBtn') }}
          </el-button>
        </template>
      </el-table-column>
    </el-table>
    <pagination v-show="total>0" :total="total" :page.sync="listQuery.pageIndex" :limit.sync="listQuery.pageSize" @pagination="getList" />

    <el-dialog :title="textMap[dialogStatus]" :visible.sync="dialogFormVisible">
      <el-form ref="dataForm" :rules="rules" :model="temp" label-position="left" label-width="120px" style="width: 100%;box-sizing:border-box; padding:0 50px;">
        <el-form-item v-show="dialogStatus==='create'" label="商家" prop="State">
          <el-select v-model="temp.ShopId" class="filter-item" placeholder="商家">
            <el-option v-for="item in shopNotSelected" :key="item.Id" :label="item.Name" :value="item.Id" :disabled="temp.Id!=''&&temp.Id!=null" />
          </el-select>
        </el-form-item>
        <el-form-item label="邮费" prop="postage">
          <el-input v-model="temp.Postage" />
        </el-form-item>
        <el-form-item label="排序" prop="Sort">
          <el-select v-model="temp.Sort" placeholder="排序" style="width:100%">
            <el-option v-for="item in sortList" :key="item" :label="item" :value="item" />
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
import { getPlatformShopList, getSmallDistrictShopNotSelected, createSmallDistrictShop, fetchList, updateSmallDistrictShop, delSmallDistrictShop } from '@/api/wyShop'
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
      activeNames: ['1', '2', '3', '4', '5', '6'],
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
        Summary: '',
        Content: ''
      },
      dialogFormVisible: false,
      dialogStatus: '',
      textMap: {
        update: '修改关联商家',
        create: '新增关联商家'
      },
      sortList: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10],
      rules: {
        Sort: [{ required: true, message: '排序是必选的', trigger: 'change' }]
      },
      voteDetailCon: '',
      dialogPvVisible: false,
      shopList: '',
      shopNotSelected: '',
      createLoading: false
    }
  },
  computed: {
    ...mapGetters([
      'loginuser'
    ])
  },
  created() {
    this.getList()
    this.getShop()
    this.rootUrl = process.env.API_HOST
  },
  methods: {
    getShopNotSelected() {
      // 获取省
      getSmallDistrictShopNotSelected().then(response => {
        this.shopNotSelected = response.data.data
      })
    },
    getShop() {
      // 获取省
      getPlatformShopList().then(response => {
        this.shopList = response.data.data
      })
    },
    handleChange(val) {
      // console.log(val)
    },
    addfile(file) {
      this.createLoading = true
    },
    imgError(file) {
      this.createLoading = false
    },
    dropzoneS(file) {
      // 上传
      const succesCon = JSON.parse(file.xhr.response)
      // console.log(succesCon)
      if (succesCon.code === '200') {
        this.temp.AnnexId = succesCon.data.Id
        this.$message({ message: '上传成功', type: 'success' })
      } else {
        this.$message({ message: '上传失败', type: 'error' })
      }
      this.createLoading = false
    },
    dropzoneR(file) {
      // console.log(file)
      this.$message({ message: '删除成功', type: 'success' })
    },
    getList() {
      this.listLoading = true
      fetchList(this.listQuery).then(response => {
        this.list = response.data.data.List
        // console.log(this.list)
        this.total = response.data.data.TotalCount
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
    // 删除
    delSmallDistrictShopBtn(row, Id) {
      delSmallDistrictShop(Id).then(response => {
        this.getList()
        this.$notify({
          title: '成功',
          message: '删除成功',
          type: 'success',
          duration: 2000
        })
      })
    },
    // toupiao详情
    // voteDetail(row, Id) {
    //   detailVote(Id).then(response => {
    //     this.dialogPvVisible = true
    //     this.voteDetailCon = response.data.data
    //     this.voteDetailCon.CreateUserName = row.CreateUserName
    //     this.getList()
    //   })
    // },
    resetTemp() {
      this.temp = {
        Title: '',
        Summary: '',
        Deadline: '',
        Question: ''
      }
    },
    // 登记 按钮
    handleCreate() {
      this.resetTemp()
      this.getShopNotSelected()
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
          // console.log(this.temp)
          createSmallDistrictShop(this.temp).then(() => {
            this.getList()
            this.dialogFormVisible = false
            this.$notify({
              title: '成功',
              message: '创建成功',
              type: 'success',
              duration: 2000
            })
          })
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
    },
    updateData() {
      this.$refs['dataForm'].validate((valid) => {
        if (valid) {
          const tempData = Object.assign({}, this.temp)
          updateSmallDistrictShop(tempData).then(() => {
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


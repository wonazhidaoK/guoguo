<template>
  <div class="app-container">
    <div class="filter-container">
      <el-input v-model="listQuery.Name" placeholder="商品名称" style="width: 200px;" class="filter-item" @keyup.enter.native="handleFilter" />
      <el-input v-model="listQuery.BarCode" placeholder="商品条码" style="width: 200px;" class="filter-item" @keyup.enter.native="handleFilter" />
      <el-button v-waves class="filter-item" type="primary" icon="el-icon-search" @click="handleFilter">{{ $t('table.search') }}</el-button>
      <!-- 登记 -->
      <el-button v-waves class="filter-item" style="margin-left: 10px;" type="primary" icon="el-icon-edit" @click="handleCreate">{{ $t('table.Add') }}</el-button>
    </div>
    <el-table v-loading="listLoading" :key="tableKey" :data="list" border fit highlight-current-row style="width: 100%;">
      <el-table-column label="图片" align="center">
        <template slot-scope="scope">
          <img :src="scope.row.ImageUrl?[imgAjaxUrlTable+scope.row.ImageUrl]: defaultImage " height="50" width="50">
        </template>
      </el-table-column>
      <el-table-column label="商品名称" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.Name }}</span>
        </template>
      </el-table-column>
      <el-table-column label="商品条码" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.BarCode }}</span>
        </template>
      </el-table-column>
      <el-table-column label="参考价格" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.Price }}</span>
        </template>
      </el-table-column>
      <el-table-column :label="$t('table.actions')" align="center" class-name="small-padding fixed-width">
        <template slot-scope="scope">
          <el-button v-waves type="primary" size="mini" @click="handleUpdate(scope.row)">{{ $t('table.edit') }}</el-button>
          <el-button v-waves v-if="scope.row.status!='deleted'" size="mini" type="danger" @click="delBtn(scope.row, scope.row.Id)">
            {{ $t('table.delete') }}
          </el-button>
        </template>
      </el-table-column>
    </el-table>
    <pagination v-show="total>0" :total="total" :page.sync="listQuery.pageIndex" :limit.sync="listQuery.pageSize" @pagination="getList" />

    <el-dialog :title="textMap[dialogStatus]" :visible.sync="dialogFormVisible">
      <el-form ref="dataForm" :rules="rules" :model="temp" label-position="left" label-width="120px" style="width: 100%;box-sizing:border-box; padding:0 50px;">
        <el-form-item v-show="dialogStatus==='create'||dialogStatus==='update'" label="商品名称" prop="Name">
          <el-input v-model="temp.Name" />
        </el-form-item>
        <el-form-item v-show="dialogStatus==='create'" label="商品条码" prop="BarCode">
          <el-input v-model="temp.BarCode" />
        </el-form-item>
        <el-form-item v-show="dialogStatus==='create'||dialogStatus==='update'" label="参考价格" prop="Price">
          <el-input v-model="temp.Price" />
        </el-form-item>
        <div class="editor-container">
          <!-- 上传图片POST /api/uploadVote -->
          <dropzone v-if="dialogFormVisible" id="myVueDropzone" :url="rootUrl+'/api/uploadPlatformCommodityCertification'" :default-img="temp.ImageUrl?[imgAjaxUrl+temp.ImageUrl]:[]" accepted-files="image/*" @dropzone-fileAdded="addfile" @dropzone-error="imgError" @dropzone-removedFile="dropzoneR" @dropzone-success="dropzoneS" />
        </div>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="dialogFormVisible = false">{{ $t('table.cancel') }}</el-button>
        <el-button v-loading="createLoading" type="primary" @click="dialogStatus==='create'?createData():updateData()">{{ $t('table.confirm') }}</el-button>
      </div>
    </el-dialog>
    <!-- <SignalrComs :apiurl="rootUrl"></SignalrComs> -->
  </div>
</template>

<script>
import { getAllForPage, del, add, update } from '@/api/goods'
import waves from '@/directive/waves' // Waves directive
import Pagination from '@/components/Pagination' // Secondary package based on el-pagination
import Dropzone from '@/components/Dropzone'
import defaultImage from '@/assets/default_images/icon-ts-mrpic.png'
// import SignalrComs from '@/components/SignalrComs'
export default {
  name: 'ComplexTable',
  components: { Pagination, Dropzone },
  // components: { Pagination, Dropzone, SignalrComs},
  directives: { waves },
  data() {
    return {
      imgAjaxUrlTable: '',
      imgAjaxUrl: '',
      dialogImg: false, // 预览图片
      defaultImage: defaultImage, // 预览图片地址
      rootUrl: '',
      tableKey: 0,
      list: null,
      total: 0,
      listLoading: true,
      listQuery: {
        pageIndex: 1,
        pageSize: 20,
        Name: '',
        BarCode: ''
      },
      textMap: {
        update: '编辑商品',
        create: '新建商品'
      },
      dialogStatus: '', // 弹出层的状态
      dialogFormVisible: false,
      temp: {
        Id: '',
        BarCode: '',
        Name: '',
        ImageUrl: '',
        Price: 0
      },
      rules: {
        Name: [{ required: true, message: '商品名称是必填的', trigger: 'blur' }],
        BarCode: [{ required: true, message: '商品条码不能为空', trigger: 'blur' }],
        Price: [{ required: true, message: '参考价格不能为空', trigger: 'blur' }]
      },
      createLoading: false
    }
  },
  created() {
    this.getList()
    this.rootUrl = process.env.API_HOST
    this.imgAjaxUrlTable = this.rootUrl + '/Upload/'
  },
  methods: {
    getList() {
      this.listLoading = true
      getAllForPage(this.listQuery).then(response => {
        this.list = response.data.data.List
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
    handleCreate() {
      this.imgAjaxUrl = ''
      this.resetTemp()
      this.createLoading = false
      this.dialogStatus = 'create'
      this.dialogFormVisible = true
      this.$nextTick(() => {
        this.$refs['dataForm'].clearValidate()
      })
    },
    resetTemp() {
      this.temp = {
        Id: '',
        BarCode: '',
        Name: '',
        ImageUrl: '',
        Price: 0
      }
    },
    handleUpdate(row) {
      this.resetTemp()
      this.imgAjaxUrl = this.rootUrl + '/Upload/'
      this.createLoading = false
      this.dialogStatus = 'update'
      this.dialogFormVisible = true
      this.temp = Object.assign({}, row)
      this.$nextTick(() => {
        this.$refs['dataForm'].clearValidate()
      })
    },
    createData() {
      this.$refs['dataForm'].validate((valid) => {
        if (valid) {
          this.createLoading = true
          add(this.temp).then((response) => {
            this.getList()
            this.$notify({
              title: '成功',
              message: '创建成功',
              type: 'success',
              duration: 2000
            })
            this.dialogFormVisible = false
            this.createLoading = false
          }).catch((ErrMsg) => {
            this.createLoading = false
          })
        }
      })
    },
    updateData() {
      this.$refs['dataForm'].validate((valid) => {
        if (valid) {
          this.createLoading = true
          update(this.temp).then((response) => {
            this.getList()
            this.$notify({
              title: '成功',
              message: '保存成功',
              type: 'success',
              duration: 2000
            })
            this.dialogFormVisible = false
            this.createLoading = false
          }).catch((ErrMsg) => {
            this.createLoading = false
          })
        }
      })
    },
    delBtn(row) {
      this.$confirm('确定要删除吗?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'delete'
      }).then(() => {
        del(row.Id).then((response) => {
          this.getList()
          this.$notify({
            title: '成功',
            message: '删除成功',
            type: 'success',
            duration: 2000
          })
        }).catch((ErrMsg) => {
        })
      }).catch(() => {
      })
    },
    addfile(file) {
      this.createLoading = true
    },
    imgError(file) {
      this.createLoading = false
    },
    dropzoneS(file) {
      const succesCon = JSON.parse(file.xhr.response)
      if (succesCon.code === '200') {
        this.temp.ImageUrl = succesCon.data.Url
        this.$message({ message: '上传成功', type: 'success' })
      } else {
        this.$message({ message: '上传失败', type: 'error' })
      }
      this.createLoading = false
    },
    dropzoneR(file) {
      this.temp.ImageUrl = ''
    }
  }
}
</script>

<style scoped>
</style>

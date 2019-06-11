<template>
  <div class="app-container">
    <div class="filter-container">
      <el-input v-model="listQuery.title" placeholder="标题" style="width: 200px;" class="filter-item" @keyup.enter.native="handleFilter"/>
      <el-date-picker v-model="listQuery.startTime" type="date" clearable format="yyyy-MM-dd" value-format="yyyy-MM-dd" class="filter-item" placeholder="选择开始时间" @change="changeTime('start')"/>
      <el-date-picker v-model="listQuery.endTime" type="date" clearable format="yyyy-MM-dd" value-format="yyyy-MM-dd" class="filter-item" placeholder="选择结束时间" @change="changeTime('end')"/>
      <el-button v-waves class="filter-item" type="primary" icon="el-icon-search" @click="handleFilter">{{ $t('table.search') }}</el-button>
      <!-- 登记 -->
      <el-button class="filter-item" style="margin-left: 10px;" type="primary" icon="el-icon-edit" @click="handleCreate">新建</el-button>
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
      <el-table-column label="摘要" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.Summary }}</span>
        </template>
      </el-table-column>
      <el-table-column label="内容" align="center" show-overflow-tooltip>
        <template slot-scope="scope">
          <span>{{ scope.row.Content }}</span>
        </template>
      </el-table-column>
      <el-table-column label="发布时间" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.ReleaseTime }}</span>
        </template>
      </el-table-column>
      <!-- <el-table-column label="附件" align="center">
        <template slot-scope="scope">
          <img :src="scope.row.Url" width="40" height="40" class="head_pic">
        </template>
      </el-table-column> -->
      <el-table-column :label="$t('table.actions')" align="center" class-name="small-padding fixed-width">
        <template slot-scope="scope">
          <!-- <el-button type="primary" size="mini" @click="handleUpdate(scope.row)">{{ $t('table.edit') }}</el-button> -->
          <el-button size="mini" type="primary" @click="noticeDetail(scope.row, scope.row.Id)">详情</el-button>
          <el-button v-if="scope.row.status!='deleted'" size="mini" type="danger" @click="delJdbBtn(scope.row, scope.row.Id)">{{ $t('table.delete') }}</el-button>
        </template>
      </el-table-column>
    </el-table>

    <pagination v-show="total>0" :total="total" :page.sync="listQuery.pageIndex" :limit.sync="listQuery.pageSize" @pagination="getList" />

    <el-dialog :title="textMap[dialogStatus]" :visible.sync="dialogFormVisible">
      <!-- <h4 style="text-align:center;">请您仔细核对信息，暂不支持删除功能</h4> -->
      <el-form ref="dataForm" :rules="rules" :model="temp" label-position="left" label-width="120px" style="width: 100%;box-sizing:border-box; padding:0 50px;">
        <el-form-item label="标题 " prop="Title">
          <el-input v-model="temp.Title" :disabled="temp.Id!=''&&temp.Id!=null"/>
        </el-form-item>
        <el-form-item label="摘要" prop="Summary">
          <el-input v-model="temp.Summary" :disabled="temp.Id!=''&&temp.Id!=null"/>
        </el-form-item>
        <el-form-item label="内容" prop="Content">
          <el-input v-model="temp.Content" :disabled="temp.Id!=''&&temp.Id!=null"/>
        </el-form-item>
        <div class="editor-container">
          <!-- 上传图片 -->
          <dropzone v-if="dialogFormVisible" id="myVueDropzone" :url="rootUrl+'/api/uploadAnnouncement'" @dropzone-fileAdded="addfile" @dropzone-error="imgError" @dropzone-removedFile="dropzoneR" @dropzone-success="dropzoneS" />
        </div>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="dialogFormVisible = false">{{ $t('table.cancel') }}</el-button>
        <el-button v-loading="createLoading" type="primary" @click="dialogStatus==='create'?createData():updateData()">{{ $t('table.confirm') }}</el-button>
      </div>
    </el-dialog>

    <el-dialog :visible.sync="dialogPvVisible" title="公告详情">
      <div>
        <h3>
          {{ noticeDetailCon.Title }}
        </h3>
        <h4>{{ noticeDetailCon.Summary }}</h4>
        <h5 style="border-bottom:1px solid #e8e8e8;padding-bottom:15px;">
          结束时间: {{ noticeDetailCon.ReleaseTime }} 创建人：{{ noticeDetailCon.CreateUserName }}
        </h5>
        <div style="border-bottom:1px dotted #e8e8e8;;padding-bottom:20px;">{{ noticeDetailCon.Content }}</div>
        <img v-show="noticeDetailCon.Url!=''" :src="noticeDetailCon.Url" width="100%" class="head_pic">
      </div>
      <span slot="footer" class="dialog-footer">
        <el-button type="primary" @click="dialogPvVisible = false">{{ $t('table.confirm') }}</el-button>
      </span>
    </el-dialog>

  </div>
</template>

<script>
import { Message } from 'element-ui'
import { mapGetters } from 'vuex'
import { createNotice, fetchListNotice, updateLy, delNotice } from '@/api/wyNotice'
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
      initbol: true,
      activeNames: ['1'],
      tableKey: 0,
      list: null,
      total: 0,
      listLoading: true,
      listQuery: {
        pageIndex: 1,
        pageSize: 20
      },
      importanceOptions: [1, 2, 3],
      sortOptions: [{ label: 'ID Ascending', key: '+id' }, { label: 'ID Descending', key: '-id' }],
      statusOptions: ['published', 'draft', 'deleted'],
      showReviewer: false,
      temp: {
        Title: '',
        Summary: '',
        Content: ''
      },
      dialogFormVisible: false,
      dialogStatus: '',
      textMap: {
        update: '编辑公告',
        create: '新建公告'
      },
      dialogPvVisible: false,
      pvData: [],
      rules: {
        Title: [{ required: true, message: '标题是必填的', trigger: 'blur' }],
        Summary: [{ required: false, message: '摘要是必填的', trigger: 'blur' }],
        Content: [{ required: true, message: '内容是必填的', trigger: 'blur' }]
      },
      downloadLoading: false,
      noticeDetailCon: '',
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
    this.rootUrl = process.env.API_HOST
  },
  methods: {
    handleChange(val) {
      // console.log(val)
    },
    dropzoneS(file, element, dropzone) {
      // 上传
      const succesCon = JSON.parse(file.xhr.response)
      // console.log(succesCon)
      if (succesCon.code === '200') {
        this.temp.AnnexId = succesCon.data.Id
        this.$message({ message: '上传成功', type: 'success' })
      } else {
        this.$message({ message: '上传失败', type: 'error' })
      }
    },
    dropzoneR(file) {
      // console.log(file)
      this.temp.AnnexId = ''
      // this.$message({ message: '删除成功', type: 'success' })
    },
    changeTime(type) {
      // console.log(this.listQuery.startTime)
      // console.log(this.listQuery.endTime)
      if (this.listQuery.endTime !== undefined && this.listQuery.startTime !== undefined) {
        if (this.checkTime(this.listQuery.startTime, this.listQuery.endTime)) {
          this.listQuery.pageIndex = 1
          this.getList()
        } else {
          if (type === 'start') {
            this.listQuery.startTime = ''
          } else {
            this.listQuery.endTime = ''
          }
          this.getList()
          Message({
            message: '结束时间不能小于开始时间',
            type: 'error',
            duration: 3 * 1000
          })
        }
      } else {
        this.listQuery.pageIndex = 1
        this.getList()
      }
    },
    checkTime(stime, etime) {
      // 通过replace方法将字符串转换成Date格式
      var sdate = new Date(Date.parse(stime.replace(/-/g, '/')))
      var edate = new Date(Date.parse(etime.replace(/-/g, '/')))
      // 获取两个日期的年月日
      var smonth = sdate.getMonth() + 1
      var syear = sdate.getFullYear()
      var sday = sdate.getDate()
      var emonth = edate.getMonth() + 1
      var eyear = edate.getFullYear()
      var eday = edate.getDate()
      // 从年，月，日，分别进行比较
      if (syear > eyear) {
        return false
      } else {
        if (smonth > emonth) {
          return false
        } else {
          if (sday > eday) {
            return false
          } else {
            return true
          }
        }
      }
    },
    noticeDetail(row, Id) {
      // detailNotice(Id).then(response => {
      this.dialogPvVisible = true
      this.activeNames = ['1']
      this.noticeDetailCon = row
      // })
    },
    getList() {
      this.listLoading = true
      fetchListNotice(this.listQuery).then(response => {
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
    // 删除街道办
    delJdbBtn(row, Id) {
      this.$confirm('确定要删除吗?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'delete'
      }).then(() => {
        delNotice(Id).then(response => {
          this.getList()
          this.$notify({
            title: '成功',
            message: '删除成功',
            type: 'success',
            duration: 2000
          })
        })
      }).catch(() => {
        // this.$message({
        //   type: 'info',
        //   message: '已取消删除'
        // });
      })
    },
    resetTemp() {
      this.temp = {
        Title: '',
        Summary: '',
        Content: ''
      }
    },
    // 登记街道办 按钮
    handleCreate() {
      this.resetTemp()
      this.dialogStatus = 'create'
      this.dialogFormVisible = true
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
            createNotice(this.temp).then((response) => {
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

      // 编辑街道办
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
    }
  }
}
</script>

<style>
  .el-tooltip__popper {
    max-width: 400px;
    line-height: 180%;
  }
</style>

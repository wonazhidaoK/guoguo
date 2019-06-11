<template>
  <div class="app-container">
    <div class="filter-container">
      <el-select v-model="listQuery.smallDistrictId" class="filter-item" placeholder="请选择小区" @change="handleFilter">
        <el-option v-for="item in xiaoquList" :key="item.Name" :label="item.Name" :value="item.Id"/>
      </el-select>
      <el-date-picker v-model="listQuery.startTime" type="date" clearable format="yyyy-MM-dd" value-format="yyyy-MM-dd" class="filter-item" placeholder="选择开始时间" @change="changeTime('start')"/>
      <el-date-picker v-model="listQuery.endTime" type="date" clearable format="yyyy-MM-dd" value-format="yyyy-MM-dd" class="filter-item" placeholder="选择结束时间" @change="changeTime('end')"/>
      <el-button v-waves class="filter-item" type="primary" icon="el-icon-search" @click="handleFilter">{{ $t('table.search') }}</el-button>
      <!-- <el-button class="filter-item" style="margin-left: 10px;" type="primary" icon="el-icon-edit" @click="handleCreate">{{ $t('table.register') }}</el-button> -->
    </div>
    <el-table
      v-loading="listLoading"
      :key="tableKey"
      :data="list"
      border
      fit
      highlight-current-row
      style="width: 100%;">
      <el-table-column label="申请人姓名" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.Name }}</span>
        </template>
      </el-table-column>
      <el-table-column label="职能名称" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.StructureName }}</span>
        </template>
      </el-table-column>
      <el-table-column label="小区名称" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.SmallDistrictName }}</span>
        </template>
      </el-table-column>
      <el-table-column label="申请时间" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.CreateTime }}</span>
        </template>
      </el-table-column>
      <el-table-column label="审核结果" align="center">
        <template slot-scope="scope">
          <el-tag v-show="!scope.row.IsAdopt" type="warning"> 未通过 </el-tag>
          <el-tag v-show="scope.row.IsAdopt" type="success"> 通过 </el-tag>
        </template>
      </el-table-column>
      <el-table-column label="业委会名称" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.VipOwnerName }}</span>
        </template>
      </el-table-column>
      <el-table-column label="是否当选" align="center">
        <template slot-scope="scope">
          <el-tag v-show="!scope.row.IsElected" type="warning"> 未当选 </el-tag>
          <el-tag v-show="scope.row.IsElected" type="success"> 已当选 </el-tag>
        </template>
      </el-table-column>
    </el-table>
    <pagination v-show="total>0" :total="total" :page.sync="listQuery.pageIndex" :limit.sync="listQuery.pageSize" @pagination="getList" />

    <el-dialog :visible.sync="dialogPvVisible" title="站内信详情">
      <div>
        <h3>
          {{ voteDetailCon.Title }}
        </h3>
        <h4 v-show="voteDetailCon.Summary">-{{ voteDetailCon.Summary }}</h4>
        <h5 style="border-bottom:1px solid #e8e8e8;padding-bottom:15px;font-weight:400">
          街道办: {{ voteDetailCon.StreetOfficeName }} 创建人:{{ voteDetailCon.CreateUserName }}
        </h5>
        <div style="border-bottom:1px dotted #e8e8e8;;padding-bottom:20px;">{{ voteDetailCon.Content }}</div>
        <img v-show="voteDetailCon.Url!=''" :src="voteDetailCon.Url" width="100%" class="head_pic">
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
import { fetchList, updateVote, delVote, detailVote, getxiaoquList } from '@/api/jdbOwner'
import waves from '@/directive/waves' // Waves directive
import Pagination from '@/components/Pagination' // Secondary package based on el-pagination
import Dropzone from '@/components/Dropzone'

export default {
  name: 'WyOwner',
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
        update: '编辑投票',
        create: '新建投票'
      },
      rules: {
        Title: [{ required: true, message: '标题是必填的', trigger: 'blur' }],
        Summary: [{ required: true, message: '内容是必填的', trigger: 'blur' }],
        Deadline: [{ required: true, message: '是必填的', trigger: 'change' }],
        Question: [{ required: true, message: '问题是必填的', trigger: 'blur' }]
      },
      voteDetailCon: '',
      dialogPvVisible: false,
      xiaoquList: []
    }
  },
  computed: {
    ...mapGetters([
      'loginuser'
    ])
  },
  created() {
    this.getList()
    this.getSmall()
    this.rootUrl = process.env.API_HOST
  },
  methods: {
    getSmall() {
      getxiaoquList(this.$store.getters.loginuser.StreetOfficeId).then(response => {
        this.xiaoquList = response.data.data.List
      })
    },
    changeTime(type) {
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
    delVoteBtn(row, Id) {
      delVote(Id).then(response => {
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
    voteDetail(row, Id) {
      detailVote(Id).then(response => {
        this.dialogPvVisible = true
        this.voteDetailCon = response.data.data
        this.voteDetailCon.CreateUserName = row.CreateUserName
        this.getList()
      })
    },
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
      this.dialogStatus = 'create'
      this.dialogFormVisible = true
      this.$nextTick(() => {
        this.$refs['dataForm'].clearValidate()
      })
    },
    // 新建
    createData() {
      // this.$refs['dataForm'].validate((valid) => {
      //   if (valid) {
      //     // console.log(this.temp)
      //     createvVote(this.temp).then(() => {
      //       this.getList()
      //       this.dialogFormVisible = false
      //       this.$notify({
      //         title: '成功',
      //         message: '创建成功',
      //         type: 'success',
      //         duration: 2000
      //       })
      //     })
      //   }
      // })
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
          updateVote(tempData).then(() => {
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

<template>
  <div class="app-container">
    <div class="filter-container">
      <!-- <el-input v-model="listQuery.description" placeholder="描述" style="width: 200px;" class="filter-item" @keyup.enter.native="handleFilter"/> -->
      <el-select v-model="listQuery.statusValue" placeholder="投诉状态" clearable style="width: 200px" class="filter-item" @change="handleFilter()">
        <el-option v-for="item in tsStatusList" :key="item.Name" :label="item.Name" :value="item.Value"/>
      </el-select>
      <el-select v-model="listQuery.departmentValue" placeholder="投诉发起部门" clearable style="width: 200px" class="filter-item" @change="handleBm()">
        <el-option v-for="item in tsBmList" :key="item.Name" :label="item.Name" :value="item.Value"/>
      </el-select>
      <el-select v-model="listQuery.complaintTypeId" placeholder="投诉类型" clearable style="width: 200px" class="filter-item" @change="handleFilter()">
        <el-option v-for="item in tsTypeList" :key="item.Name" :label="item.Name" :value="item.Id"/>
      </el-select>
      <el-date-picker v-model="listQuery.startTime" type="date" clearable format="yyyy-MM-dd" value-format="yyyy-MM-dd" class="filter-item" placeholder="选择开始时间" @change="changeTime('start')"/>
      <el-date-picker v-model="listQuery.endTime" type="date" clearable format="yyyy-MM-dd" value-format="yyyy-MM-dd" class="filter-item" placeholder="选择结束时间" @change="changeTime('end')"/>
      <el-button v-waves class="filter-item" type="primary" icon="el-icon-search" @click="handleFilter">{{ $t('table.search') }}</el-button>
      <!-- 登记 -->
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
      <el-table-column label="用户" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.OperationName }}</span>
        </template>
      </el-table-column>
      <el-table-column label="用户角色" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.OperationDepartmentName }}</span>
        </template>
      </el-table-column>
      <el-table-column label="创建时间" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.CreateTime }}</span>
        </template>
      </el-table-column>
      <el-table-column label="投诉类型" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.ComplaintTypeName }}</span>
        </template>
      </el-table-column>
      <el-table-column label="状态" align="center">
        <template slot-scope="scope">
          <!-- "未受理""NotAccepted" "处理中""Processing" "已完结""Finished" "已完成""Completed"  -->
          <el-tag v-show="scope.row.StatusValue == 'NotAccepted'" type="info">{{ scope.row.StatusName }}</el-tag>
          <el-tag v-show="scope.row.StatusValue == 'Processing'" type="danger">{{ scope.row.StatusName }}</el-tag>
          <el-tag v-show="scope.row.StatusValue == 'Finished'" type="warning">{{ scope.row.StatusName }}</el-tag>
          <el-tag v-show="scope.row.StatusValue == 'Completed'" type="success">{{ scope.row.StatusName }}</el-tag>
        </template>
      </el-table-column>
      <el-table-column :label="$t('table.actions')" align="center" class-name="small-padding fixed-width">
        <template slot-scope="scope">
          <el-button v-show="scope.row.StatusValue == 'NotAccepted'||scope.row.StatusValue == 'Processing'" type="primary" size="mini" @click="handleUpdate(scope.row)">处理</el-button>
          <el-button size="mini" type="success" @click="tsDetail(scope.row, scope.row.Id)">详情</el-button>
          <el-button v-show="scope.row.StatusValue == 'NotAccepted'||scope.row.StatusValue == 'Processing'" size="mini" type="danger" style="width:80px" @click="delTs(scope.row, scope.row.Id)">置为无效</el-button>
        </template>
      </el-table-column>
    </el-table>

    <pagination v-show="total>0" :total="total" :page.sync="listQuery.pageIndex" :limit.sync="listQuery.pageSize" @pagination="getList" />

    <el-dialog :title="textMap[dialogStatus]" :visible.sync="dialogFormVisible">
      <el-form ref="dataForm" :rules="rules" :model="temp" label-position="left" label-width="120px" style="width: 100%;box-sizing:border-box; padding:0 50px;">
        <el-form-item label="描述" prop="Description">
          <el-input v-model="temp.Description" :disabled="temp.Id!=''&&temp.Id!=null"/>
        </el-form-item>
        <div class="editor-container">
          <!-- 上传图片 POST /api/uploadComplaint-->
          <dropzone v-if="dialogFormVisible" id="myVueDropzone" :url="rootUrl+'/api/uploadComplaint'" @dropzone-fileAdded="addfile" @dropzone-error="imgError" @dropzone-removedFile="dropzoneR" @dropzone-success="dropzoneS" />
        </div>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="dialogFormVisible = false">{{ $t('table.cancel') }}</el-button>
        <el-button v-loading="createLoading" type="primary" @click="dialogStatus==='create'?createData():updateData()">{{ $t('table.confirm') }}</el-button>
      </div>
    </el-dialog>

    <el-dialog :visible.sync="dialogPvVisible" title="投诉详情">
      <div class="stepComponent" >
        <div class="approvalProcess" >
          <!-- <el-steps :active="active" finish-status="success" direction="vertical"> -->
          <el-steps direction="vertical">
            <el-step v-for="(item,d) in tsDetailList" :title="item.OperationName+'('+item.OperationDepartmentName+')'" :id="item.id" :key="d">
              <template slot="description">
                <div class="step-row">
                  <table width="100%" border="0" cellspacing="0" cellpadding="0" class="processing_content">
                    <tr>
                      <td style="color:#98A6BE">
                        <div class="processing_content_detail" style="float:left;width:70%"><span >{{ item.Description }}</span></div>
                        <div class="processing_content_detail" style="float:right;"><span ><i class="el-icon-time"/>&nbsp;&nbsp;{{ item.CreateTime }}</span> </div>
                      </td>
                    </tr>
                    <tr v-show="d == 0">
                      <td>{{ tsDd.Description }}</td>
                    </tr>
                    <tr v-show="item.Url != ''">
                      <td>
                        <div class="processing_content_detail" style="float:left;width:70%">
                          <div style="float:left;width: 2px;height: 20px; background:#C7D4E9;margin-left:10px;margin-right:10px"/>
                          <img :src="item.Url" alt="" style="width:100px;height:auto">
                        </div>
                      </td>
                    </tr>
                  </table>
                </div>
              </template>
            </el-step>
          </el-steps>
        </div>
      </div>
      <div v-show="tsDd.StatusValue!=='Completed'" style="text-align:center;position:relative">
        <el-progress :stroke-width="10" :percentage="endDay/tsDd.ComplaintPeriod*100 | numFilter" :show-text="false" type="circle"/>
        <span style="position:absolute;left:45%;top:45px;width:88px;">{{ '剩余天数' + endDay + '/总天数' + tsDd.ComplaintPeriod }}</span>
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
import { createNotice, fetchList, handleTs, delTs, detailTs, chakanTs, tsStatusFn, tsBmFn, tsTypeFn } from '@/api/wyTs'
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
    },
    numFilter(value) {
      const realVal = parseFloat(value).toFixed(2)
      return realVal
    }
  },
  data() {
    return {
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
        Description: ''
      },
      dialogFormVisible: false,
      dialogStatus: '',
      textMap: {
        update: '处理投诉',
        create: '新建投诉'
      },
      dialogPvVisible: false,
      pvData: [],
      rules: {
        Description: [{ required: true, message: '描述是必填的', trigger: 'blur' }]
      },
      downloadLoading: false,
      tsDetailList: [],
      tsStatusList: [],
      tsDd: [],
      endDay: 0,
      createLoading: false,
      num: 0,
      tsBmList: '',
      tsTypeList: ''
    }
  },
  computed: {
    ...mapGetters([
      'loginuser'
    ])
  },
  created() {
    this.getList()
    this.getTsStatusList()
    this.getTsBmList()
    this.rootUrl = process.env.API_HOST
    // this.getTsTypeList('')
  },
  methods: {
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
    tsDetail(row, Id) {
      detailTs(Id).then(response => {
        this.dialogPvVisible = true
        this.tsDetailList = response.data.data.List
        this.tsDd = response.data.data
        const nowDay = days(row.CreateTime)
        this.endDay = this.tsDd.ComplaintPeriod - nowDay
        function days(time) {
          var strtime = time.replace('/-/g', '/')
          var date1 = new Date(strtime)
          var date2 = new Date()
          var date3 = date2.getTime() - date1.getTime()
          var days = Math.floor(date3 / (24 * 3600 * 1000))
          // console.log(days)
          return days
        }
      })
      chakanTs(Id).then(response => {
        this.getList()
      })
    },
    getTsStatusList() {
      tsStatusFn().then(response => {
        this.tsStatusList = response.data.data
      })
    },
    handleBm() {
      this.listQuery.complaintTypeId = ''
      this.tsTypeList = ''
      if (this.listQuery.departmentValue) {
        this.getTsTypeList(this.listQuery.departmentValue)
      }
      this.handleFilter()
    },
    getTsBmList() {
      tsBmFn().then(response => {
        this.tsBmList = response.data.data
      })
    },
    getTsTypeList(id) {
      tsTypeFn(id).then(response => {
        this.tsTypeList = response.data.data
      })
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
    // 投诉置为无效
    delTs(row, Id) {
      delTs(Id).then(response => {
        this.getList()
        this.$notify({
          title: '成功',
          message: '置为无效成功',
          type: 'success',
          duration: 2000
        })
      })
    },
    resetTemp() {
      this.temp = {
        Title: '',
        Summary: '',
        Content: ''
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
      // 编辑
      // this.temp = Object.assign({}, row) // copy obj
      // this.temp.timestamp = new Date(this.temp.timestamp)
      this.temp.ComplaintId = row.Id
      this.temp.Description = ''
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
            handleTs(tempData).then((response) => {
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
    }
  }
}
</script>
<style scoped>
  .stepComponent{
    background: #fff;
    width: 100%-20px;
    padding: 10px 10px 10px 10px;
    margin: 10px 10px 10px 10px;
  }
  .approvalProcess{
    color: #9EADC4;
    font-size: 14px;
    /* width: 100%; */
    background:#fff;
    margin-left:20px;
    margin-right:0px;
    margin-top:10px;
  }
  .processing_content{
    background-color: #D9E5F9;
  }
  .processing_content_detail{
    margin-left: 10px;
    margin-top: 3.5px;
    margin-bottom: 3.5px;
    width:150px;
    display:inline-block;
  }
  .step-row{
    min-width:500px;
    margin:12px 0;
  }
  .el-step__description{
    padding-right: 0!important;
  }
</style>


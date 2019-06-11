<template>
  <div class="app-container">
    <div class="filter-container">
      <el-input v-model="listQuery.title" placeholder="标题" style="width: 200px;" class="filter-item" @keyup.enter.native="handleFilter"/>
      <el-select v-model="listQuery.smallDistrictArray" clearable class="filter-item" placeholder="小区" @change="handleFilter">
        <el-option v-for="item in smalllist" :key="item.Name" :label="item.Name" :value="item.Id"/>
      </el-select>
      <el-select v-model="listQuery.StatusValue" clearable class="filter-item" placeholder="状态" @change="handleFilter">
        <el-option v-for="item in voteZhuangTaiList" :key="item.Name" :label="item.Name" :value="item.Id"/>
      </el-select>
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
      <el-table-column label="内容" align="center" show-overflow-tooltip>
        <template slot-scope="scope">
          <span>{{ scope.row.Summary }}</span>
        </template>
      </el-table-column>
      <el-table-column label="创建时间" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.CreateTime }}</span>
        </template>
      </el-table-column>
      <el-table-column label="状态" align="center">
        <template slot-scope="scope">
          <el-tag v-show="scope.row.StatusValue == 'Processing'" type="warning">{{ scope.row.StatusName }}</el-tag>
          <el-tag v-show="scope.row.StatusValue == 'Closed'" type="danger">{{ scope.row.StatusName }}</el-tag>
        </template>
      </el-table-column>
      <el-table-column :label="$t('table.actions')" align="center" class-name="small-padding fixed-width">
        <template slot-scope="scope">
          <!-- <el-button type="primary" size="mini" @click="handleUpdate(scope.row)">{{ $t('table.edit') }}</el-button> -->
          <!-- <el-button v-if="scope.row.status!='deleted'" size="mini" type="danger" @click="delVoteBtn(scope.row, scope.row.Id)">{{ $t('table.delete') }}
          </el-button> -->
          <el-button size="mini" type="success" @click="voteDetail(scope.row, scope.row.Id)">详情</el-button>
          <el-button size="mini" type="primary" style="width:80px;" @click="jyDetail(scope.row, scope.row.Id)">查看建议</el-button>
          <el-button v-show="scope.row.StatusValue=='Processing' && scope.row.VoteTypeValue!='VipOwnerElection'" size="mini" type="danger" style="width:80px;" @click="handTp(scope.row.Id)">手动干预</el-button>
        </template>
      </el-table-column>
    </el-table>

    <pagination v-show="total>0" :total="total" :page.sync="listQuery.pageIndex" :limit.sync="listQuery.pageSize" @pagination="getList" />

    <el-dialog :title="textMap[dialogStatus]" :visible.sync="dialogFormVisible">
      <h4 style="text-align:center;">请您仔细核对信息，暂不支持删除功能</h4>
      <el-form ref="dataForm" :rules="rules" :model="temp" label-position="left" label-width="120px" style="width: 100%;box-sizing:border-box; padding:0 50px;">
        <el-form-item label="标题 " prop="Title">
          <el-input v-model="temp.Title" :disabled="temp.Id!=''&&temp.Id!=null"/>
        </el-form-item>
        <el-form-item label="内容" prop="Summary">
          <el-input v-model="temp.Summary" :disabled="temp.Id!=''&&temp.Id!=null"/>
        </el-form-item>
        <el-form-item label="问题" prop="Question">
          <el-input v-model="temp.Question" :disabled="temp.Id!=''&&temp.Id!=null"/>
        </el-form-item>
        <el-form-item label="结束时间" prop="Deadline">
          <el-date-picker v-model="temp.Deadline" type="date" format="yyyy-MM-dd" value-format="yyyy-MM-dd" class="filter-item" placeholder="选择结束时间"/>
        </el-form-item>
        <el-form-item v-show="dialogStatus==='create'" label="投票类型" prop="VoteTypeValue">
          <el-select v-model="temp.VoteTypeValue" class="filter-item" placeholder="投票类型">
            <el-option v-for="item in typeList" :key="item.Name" :label="item.Name" :value="item.VoteTypeValue" :disabled="temp.Id!=''&&temp.Id!=null"/>
          </el-select>
        </el-form-item>
        <el-form-item label="小区" prop="SmallDistrict">
          <!-- <el-checkbox :indeterminate="isIndeterminate" v-model="checkAll" @change="handleCheckAllChange">全选</el-checkbox> -->
          <!-- <el-checkbox-group v-model="temp.SmallDistrict">
            <el-checkbox v-for="item in smalllist" :label="item.Id" :key="item.Id">{{item.Name}}</el-checkbox>
          </el-checkbox-group> -->
          <el-select v-model="temp.SmallDistrict" class="filter-item" placeholder="小区">
            <el-option v-for="item in smalllist" :key="item.Name" :label="item.Name" :value="item.Id" :disabled="temp.Id!=''&&temp.Id!=null"/>
          </el-select>
        </el-form-item>
        <div class="editor-container">
          <!-- 上传图片POST /api/uploadVote -->
          <dropzone v-if="dialogFormVisible" id="myVueDropzone" :url="rootUrl+'/api/uploadVote'" @dropzone-fileAdded="addfile" @dropzone-error="imgError" @dropzone-removedFile="dropzoneR" @dropzone-success="dropzoneS" />
        </div>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="dialogFormVisible = false">{{ $t('table.cancel') }}</el-button>
        <el-button v-loading="createLoading" type="primary" @click="dialogStatus==='create'?createData():updateData()">{{ $t('table.confirm') }}</el-button>
      </div>
    </el-dialog>

    <el-dialog :visible.sync="dialogPvVisible" title="投票详情">
      <div>
        <h3>
          {{ voteDetailCon.Title }}
        </h3>
        <h5 style="border-bottom:1px solid #e8e8e8;padding-bottom:15px;">
          结束时间: {{ voteDetailCon.Deadline }} {{ voteDetailCon.CreateUserName }} {{ voteDetailCon.SmallDistrictArrayName }}
        </h5>
        <div style="border-bottom:1px dotted #e8e8e8;;padding-bottom:20px;">{{ voteDetailCon.Summary }}</div>
        <img v-show="voteDetailCon.Url!=''" :src="voteDetailCon.Url" width="100%" class="head_pic">
        <div v-for="(i,c) in voteDetailCon.List" :key="c" class="index-footnav">
          <div style="padding-top:40px;">{{ c+1 }}、 {{ i.Title }} <span v-if="i.VoteResultName!=''">( {{ i.VoteResultName }} )</span> </div>
          <ul>
            <li v-for="(a,d) in i.List" :key="d">
              <div>{{ a.Describe + '(' + a.Votes + '票)' }}</div>
              <div style="padding:10px 0;">
                <el-progress v-show="i.VoteResultValue === ''" :stroke-width="18" :percentage="a.Votes/voteDetailCon.ShouldParticipateCount*100 | numFilter"/>
                <el-progress v-show="i.VoteResultValue === 'Adopt'" :stroke-width="18" :percentage="a.Votes/voteDetailCon.ShouldParticipateCount*100 | numFilter" status="success"/>
                <el-progress v-show="i.VoteResultValue === 'Overrule'" :stroke-width="18" :percentage="a.Votes/voteDetailCon.ShouldParticipateCount*100 | numFilter" status="exception"/>
              </div>
            </li>
          </ul>
        </div>
      </div>
      <span slot="footer" class="dialog-footer">
        <el-button type="primary" @click="dialogPvVisible = false">{{ $t('table.confirm') }}</el-button>
      </span>
    </el-dialog>

    <!-- 建议列表  -->
    <el-dialog :visible.sync="dialogPvVisibleJy" title="建议列表">
      <el-table
        v-loading="listLoadingLy"
        :key="tableKeyLy"
        :data="jyList"
        border
        fit
        highlight-current-row
        style="width: 100%;margin-top:20px;">
        <el-table-column label="姓名" align="center">
          <template slot-scope="scope">
            <span>{{ scope.row.OperationName }}</span>
          </template>
        </el-table-column>
        <el-table-column label="时间" align="center">
          <template slot-scope="scope">
            <span>{{ scope.row.ReleaseTime }}</span>
          </template>
        </el-table-column>
        <el-table-column label="建议" align="center">
          <template slot-scope="scope">
            <span>{{ scope.row.Feedback }}</span>
          </template>
        </el-table-column>
      </el-table>
      <pagination v-show="totalJy>0" :total="totalJy" :page.sync="listQueryJy.pageIndex" :limit.sync="listQueryJy.pageSize" @pagination="getJyList" />
      <span slot="footer" class="dialog-footer">
        <el-button type="primary" @click="dialogPvVisibleJy = false">{{ $t('table.confirm') }}</el-button>
      </span>
    </el-dialog>

  </div>
</template>

<script>
import { Message } from 'element-ui'
import { mapGetters } from 'vuex'
import { createvVote, fetchListVote, updateVote, delVote, detailVote, getSmallList, fetchJyList, handFn } from '@/api/jdbVote'
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
      handBol: false,
      activeNames: ['1', '2', '3', '4', '5', '6', '7'],
      tableKey: 0,
      list: null,
      total: 0,
      tableKeyLy: 0,
      voteZhuangTaiList: [{
        Id: 'Processing',
        Name: '进行中'
      }, {
        Id: 'Closed',
        Name: '已关闭'
      }],
      typeList: [{
        VoteTypeValue: 'RecallProperty',
        Name: '发起倡议'
      }, {
        VoteTypeValue: 'Ordinary',
        Name: '普通投票'
      }],
      listLoading: true,
      listQuery: {
        pageIndex: 1,
        pageSize: 20
      },
      listQueryJy: {
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
        Deadline: [{ required: true, message: '时间是必选的', trigger: 'change' }],
        Question: [{ required: true, message: '问题是必填的', trigger: 'blur' }],
        VoteTypeValue: [{ required: true, message: '投票类型是必选的', trigger: 'change' }],
        SmallDistrict: [{ required: true, message: '小区是必填的', trigger: 'change' }]
      },
      voteDetailCon: '',
      dialogPvVisible: false,
      dialogPvVisibleJy: false,
      smalllist: '',
      jyList: [],
      jyId: '',
      totalJy: 0,
      listLoadingLy: false,
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
    this.getSmall()
    this.rootUrl = process.env.API_HOST
  },
  methods: {
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
    getList() {
      this.listLoading = true
      fetchListVote(this.listQuery).then(response => {
        this.list = response.data.data.List
        // console.log(this.list)
        this.total = response.data.data.TotalCount
        setTimeout(() => {
          this.listLoading = false
        }, 1.5 * 1000)
      })
    },
    handTp(id) {
      this.$confirm('确定要手动干预吗?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'warning'
      }).then(() => {
        handFn(id).then(response => {
          this.$message({
            type: 'success',
            message: '手动干预成功!'
          })
        })
      }).catch(() => {
        // this.$message({
        //   type: 'info',
        //   message: '已取消删除'
        // });
      })
    },
    getSmall() {
      getSmallList(this.$store.getters.loginuser.StreetOfficeId).then(response => {
        let smalllist = response.data.data.List
        smalllist = smalllist.filter(item => {
          item.check = false
          return item
        })
        this.smalllist = smalllist
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
        this.activeNames = ['1']
        this.voteDetailCon = response.data.data
      })
    },
    // 建议列表
    jyDetail(row, Id) {
      this.dialogPvVisibleJy = true
      if (Id !== undefined) {
        this.jyId = Id
      }
      this.getJyList()
    },
    getJyList() {
      this.listQueryJy.id = this.jyId
      fetchJyList(this.listQueryJy).then(response => {
        this.listLoadingLy = false
        this.jyList = response.data.data.List
        this.totalJy = response.data.data.TotalCount
      })
    },
    resetTemp() {
      this.temp = {
        Title: '',
        Summary: '',
        Deadline: '',
        Question: '',
        VoteTypeValue: '',
        SmallDistrict: []
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
            createvVote(this.temp).then((response) => {
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
    },
    updateData() {
      this.$refs['dataForm'].validate((valid) => {
        if (valid) {
          const tempData = Object.assign({}, this.temp)
          this.num = this.num + 1
          if (this.num < 2) {
            this.createLoading = true
            updateVote(tempData).then((response) => {
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

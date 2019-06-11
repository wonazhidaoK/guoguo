<template>
  <div class="app-container">
    <div class="filter-container">
      <!-- <el-input v-model="listQuery.title" placeholder="标题" style="width: 200px;" class="filter-item" @keyup.enter.native="handleFilter"/> -->
      <!-- <span style="height:36px;line-height:36px;">请选择小区</span> -->
      <el-select v-model="listQuery.smallDistrictId" class="filter-item" placeholder="请选择小区" @change="handleFilter">
        <el-option v-for="item in smalllist" :key="item.Name" :label="item.Name" :value="item.Id"/>
      </el-select>
      <el-button v-waves class="filter-item" type="primary" icon="el-icon-search" @click="handleFilter">{{ $t('table.search') }}</el-button>
      <!-- 登记 -->
      <el-button class="filter-item" style="margin-left: 10px;width:150px" type="primary" icon="el-icon-edit" @click="handleCreate">业委会重组</el-button>
    </div>
    <el-table
      v-loading="listLoading"
      :key="tableKey"
      :data="list"
      border
      fit
      highlight-current-row
      style="width: 100%;">
      <el-table-column label="姓名" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.Name }}</span>
        </template>
      </el-table-column>
      <el-table-column label="申请理由" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.Reason }}</span>
        </template>
      </el-table-column>
      <el-table-column label="小区名称" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.SmallDistrictName }}</span>
        </template>
      </el-table-column>
      <el-table-column label="职能名称" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.StructureName }}</span>
        </template>
      </el-table-column>
      <el-table-column label="通过" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.IsAdopt?"是":"否" }}</span>
        </template>
      </el-table-column>
      <el-table-column :label="$t('table.actions')" align="center" class-name="small-padding fixed-width">
        <template slot-scope="scope">
          <el-button v-show="!scope.row.IsAdopt" type="danger" size="mini" @click="successBtn(scope.row)">通过</el-button>
          <!-- <el-button type="primary" size="mini" @click="handleUpdate(scope.row)">通过</el-button> -->
          <!-- <el-button v-if="scope.row.status!='deleted'" size="mini" type="danger" @click="delVoteBtn(scope.row, scope.row.Id)">{{ $t('table.delete') }}
          </el-button> -->
          <el-button size="mini" type="primary" @click="voteDetail(scope.row, scope.row.Id)">详情</el-button>
        </template>
      </el-table-column>
    </el-table>

    <pagination v-show="total>0" :total="total" :page.sync="listQuery.pageIndex" :limit.sync="listQuery.pageSize" @pagination="getList" />

    <el-dialog :title="textMap[dialogStatus]" :visible.sync="dialogFormVisible">
      <el-form ref="dataForm" :rules="rules" :model="temp" label-position="left" label-width="120px" style="width: 100%;box-sizing:border-box; padding:0 50px;">
        <el-form-item label="内容" prop="Summary">
          <el-input v-model="temp.Summary" :disabled="temp.Id!=''&&temp.Id!=null"/>
        </el-form-item>
        <el-form-item label="竞选人数" prop="electionNumber">
          <el-input v-model="temp.electionNumber" :disabled="temp.Id!=''&&temp.Id!=null"/>
        </el-form-item>
        <el-form-item label="结束时间" prop="Deadline">
          <el-date-picker v-model="temp.Deadline" type="date" format="yyyy-MM-dd" value-format="yyyy-MM-dd" class="filter-item" placeholder="选择结束时间"/>
        </el-form-item>
        <el-form-item label="小区" prop="SmallDistrictId">
          <el-select v-model="temp.SmallDistrictId" class="filter-item" placeholder="小区" @change="changeSmall">
            <el-option v-for="item in smalllist" :key="item.Name" :label="item.Name" :value="item.Id" :disabled="temp.Id!=''&&temp.Id!=null"/>
          </el-select>
        </el-form-item>
        <el-form-item label="业委会" prop="VipOwnerId">
          <el-select v-model="temp.VipOwnerId" class="filter-item" placeholder="业委会">
            <el-option v-for="item in ywhlist" :key="item.Name" :label="item.Name" :value="item.Id" :disabled="temp.Id!=''&&temp.Id!=null"/>
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
    <el-dialog :visible.sync="dialogPvVisible" title="详情">
      <h4>姓名： <small>{{ voteDetailCon.Name }}</small></h4>
      <h4>申请理由：<small>{{ voteDetailCon.Reason }}</small></h4>
      <h4>小区名称：<small>{{ voteDetailCon.SmallDistrictName }}</small></h4>
      <h4>附件</h4>
      <ul>
        <li v-for="(a,d) in voteDetailCon.AnnexModels" :key="d">
          <div @click="$imgPreview(a.Url)">{{ a.CertificationConditionName }}</div>
          <div>
            <img :src="a.Url" alt="" style="width:100px" @click="$imgPreview(a.Url)">
            <!-- <imgPreview :data-img="imageList"></imgPreview> -->
            <!-- <img :src="a.Url" :data-img="imageList" style="width:100px" @click="$imgPreview"> -->
          </div>
        </li>
      </ul>
      <span slot="footer" class="dialog-footer">
        <el-button type="primary" @click="dialogPvVisible = false">{{ $t('table.confirm') }}</el-button>
      </span>
    </el-dialog>

    <el-dialog :visible.sync="dialogImg" title="图片预览" style="padding:0;margin:0">
      <div class="components-container" style="padding:0;margin:0">
        <div class="editor-container" style="padding:0;margin:0">
          <!-- <dnd-list :list1="list1" :list2="list2" :roleid="roleId" list1-title="已有权限" list2-title="未有权限"/> -->
          <img-preview :nowimg="nowimg"/>
        </div>
      </div>
      <!-- <span slot="footer" class="dialog-footer">
        <el-button type="primary" @click="dialogPvVisible = false">{{ $t('table.confirm') }}</el-button>
      </span> -->
    </el-dialog>
  </div>
</template>

<script>
import { mapGetters } from 'vuex'
import { createvVote, fetchListVote, updateVote, delVote, detailRz, getSmallList, successRenZheng, getYwhList } from '@/api/jdbGaoji'
import waves from '@/directive/waves' // Waves directive
import Pagination from '@/components/Pagination' // Secondary package based on el-pagination
import Dropzone from '@/components/Dropzone'
import imgPreview from '@/components/imgPreview'
// debugger;
export default {
  name: 'ComplexTable',
  components: { Pagination, Dropzone, imgPreview },
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
    var checkNumber = (rule, value, callback) => {
      if (!value) {
        return callback(new Error('竞选人数不能为空'))
      } else {
        const reg = /^[0-9]+$/
        // console.log(reg.test(value))
        if (reg.test(value)) {
          callback()
        } else {
          return callback(new Error('竞选人数必须是数字'))
        }
      }
    }
    return {
      dialogImg: false,
      nowimg: '',
      imageList: [
        'https://www.guoguoshequ.com//Upload/Announcement/95-16060G40F5.jpg',
        'https://www.guoguoshequ.com//Upload/Announcement/95-16060G40F5.jpg',
        'https://www.guoguoshequ.com//Upload/Announcement/95-16060G40F5.jpg'
      ],
      activeNames: ['1'],
      tableKey: 0,
      list: null,
      total: 0,
      typeList: [{
        VoteTypeValue: 'RecallProperty',
        Name: '发起倡议'
      }, {
        VoteTypeValue: 'Ordinary',
        Name: '普通投票'
      }, {
        VoteTypeValue: 'VipOwnerElection',
        Name: '业委会重组'
      }],
      listLoading: true,
      listQuery: {
        pageIndex: 1,
        pageSize: 20
      },
      temp: {
        Summary: '',
        Deadline: '',
        SmallDistrictId: '',
        VipOwnerId: ''
      },
      dialogFormVisible: false,
      dialogStatus: '',
      textMap: {
        update: '编辑业委会选举',
        create: '新建业委会选举'
      },
      rules: {
        Summary: [{ required: true, message: '摘要是必填的', trigger: 'blur' }],
        Deadline: [{ required: true, message: '时间是必选的', trigger: 'change' }],
        SmallDistrictId: [{ required: true, message: '小区是必填的', trigger: 'change' }],
        VipOwnerId: [{ required: true, message: '业委会是必填的', trigger: 'change' }],
        electionNumber: [{ required: true, trigger: 'blur', validator: checkNumber }]
      },
      voteDetailCon: '',
      dialogPvVisible: false,
      smalllist: '',
      ywhlist: [],
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
    this.getSmall()
    this.rootUrl = process.env.API_HOST
  },
  methods: {
    handleChange(val) {
      // console.log(val)
    },
    $imgPreview(url) {
      this.dialogImg = true
      this.nowimg = url
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
    getList(type) {
      if (type === 'first') {
        this.listLoading = false
      } else {
        this.listLoading = true
        fetchListVote(this.listQuery).then(response => {
          this.list = response.data.data.List
          // console.log(this.list)
          this.total = response.data.data.TotalCount
          setTimeout(() => {
            this.listLoading = false
          }, 1.5 * 1000)
        })
      }
    },
    getSmall(type) {
      getSmallList(this.$store.getters.loginuser.StreetOfficeId).then(response => {
        let smalllist = response.data.data.List
        smalllist = smalllist.filter(item => {
          item.check = false
          return item
        })
        this.smalllist = smalllist
        // this.listQuery.smallDistrictId = smalllist[0].Id
        this.getList('first')
      })
    },
    handleFilter() {
      this.listQuery.pageIndex = 1
      this.getList()
    },
    changeSmall() {
      this.temp.VipOwnerId = ''
      this.ywhlist = ''
      // console.log(this.temp.SmallDistrictId)
      getYwhList(this.temp.SmallDistrictId).then(response => {
        this.ywhlist = response.data.data
      })
    },
    successBtn(row) {
      successRenZheng(row.Id).then(response => {
        this.getList()
        this.$notify({
          title: '成功',
          message: '通过成功',
          type: 'success',
          duration: 2000
        })
      })
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
      detailRz(Id).then(response => {
        this.dialogPvVisible = true
        this.activeNames = ['1']
        this.voteDetailCon = response.data.data
      })
    },
    resetTemp() {
      this.temp = {
        Summary: '',
        Deadline: '',
        SmallDistrictId: '',
        VipOwnerId: ''
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


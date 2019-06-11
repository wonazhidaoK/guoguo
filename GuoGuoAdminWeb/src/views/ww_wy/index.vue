<template>
  <div class="app-container">
    <div class="filter-container">
      <el-input v-model="listQuery.Name" placeholder="名称" style="width: 200px;" class="filter-item" @keyup.enter.native="handleFilter"/>
      <el-input v-model="listQuery.Phone" placeholder="联系电话" style="width: 200px;" class="filter-item" @keyup.enter.native="handleFilter"/>
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
      <el-table-column label="名称" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.Name }}</span>
        </template>
      </el-table-column>
      <el-table-column label="联系电话" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.Phone }}</span>
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
          <el-button size="mini" type="success" @click="shopDetail(scope.row, scope.row.Id)">详情</el-button>
          <el-button v-if="scope.row.status!='deleted'" size="mini" type="danger" @click="delJdbBtn(scope.row, scope.row.Id)">{{ $t('table.logoutBtn') }}
          </el-button>
        </template>
      </el-table-column>
    </el-table>

    <pagination v-show="total>0" :total="total" :page.sync="listQuery.pageIndex" :limit.sync="listQuery.pageSize" @pagination="getList"/>

    <el-dialog :title="textMap[dialogStatus]" :visible.sync="dialogCreatShow">
      <el-form ref="dataForm" :rules="rules" :model="temp" label-position="left" label-width="120px" style="width: 100%;box-sizing:border-box; padding:0 50px;">

        <el-form-item label="名称" prop="Name">
          <el-input v-model="temp.Name"/>
        </el-form-item>
        <el-form-item label="联系电话" prop="Phone">
          <el-input v-model="temp.Phone"/>
        </el-form-item>
        <el-form-item label="地址" prop="Address">
          <el-input v-model="temp.Address"/>
        </el-form-item>
        <el-form-item label="描述" prop="Description">
          <el-input v-model="temp.Description"/>
        </el-form-item>
        <el-form-item label="Logo图片" prop="LogoImageUrl">
          <div class="editor-container">
            <!-- POST /api/uploadPropertyCompany -->
            <dropzone v-if="dialogCreatShow" id="myVueDropzone" :url="rootUrl+'/api/uploadPropertyCompany'" :default-img="temp.LogoImageUrl?[imgAjaxUrl+temp.LogoImageUrl]:''" @dropzone-fileAdded="addfile" @dropzone-error="imgError" @dropzone-removedFile="dropzoneR" @dropzone-success="dropzoneS" />
          </div>
        </el-form-item>
      </el-form>
      <div slot="footer" class="dialog-footer">
        <el-button @click="dialogCreatShow = false">{{ $t('table.cancel') }}</el-button>
        <el-button v-loading="createLoading" type="primary" @click="dialogStatus==='create'?createData():updateData()">{{ $t('table.confirm') }}</el-button>
      </div>
    </el-dialog>

    <el-dialog :visible.sync="dialogDetailVisible" title="详情">
      <ul class="shopDetail">
        <li class="clearfloat">
          <div>物业公司名</div>
          <div>{{ detailCon.Name }}</div>
        </li>
        <li class="clearfloat">
          <div>联系电话</div>
          <div>{{ detailCon.Phone }}</div>
        </li>
        <li class="clearfloat">
          <div>地址</div>
          <div>{{ detailCon.Address }}</div>
        </li>
        <li class="clearfloat">
          <div>描述</div>
          <div>{{ detailCon.Description }}</div>
        </li>
        <li class="clearfloat">
          <div>Logo图片</div>
          <div>
            <img :src="imgAjaxUrl+detailCon.LogoImageUrl" alt="">
          </div>
        </li>
      </ul>
      <span slot="footer" class="dialog-footer">
        <el-button type="primary" @click="dialogDetailVisible = false">{{ $t('table.confirm') }}</el-button>
      </span>
    </el-dialog>

  </div>
</template>

<script>
import { mapGetters } from 'vuex'
import { fetchList, createShop, updateShop, delJdbRequest, getDetail } from '@/api/wwWy'
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
    return {
      imgAjaxUrl: '',
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
        Phone: '',
        Address: '',
        Description: '',
        LogoImageUrl: ''
      },
      dialogCreatShow: false,
      dialogStatus: '',
      textMap: {
        update: '编辑物业公司',
        create: '新建物业公司'
      },
      rules: {
        Name: [{ required: true, message: '名称是必填的', trigger: 'blur' }],
        Address: [{ required: true, message: '地址是必填的', trigger: 'blur' }],
        Phone: [{ required: true, trigger: 'blur', validator: checkPhone }]
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
    this.rootUrl = process.env.API_HOST
  },
  methods: {
    addfile(file) {
      this.createLoading = true
    },
    imgError(file) {
      this.createLoading = false
    },
    dropzoneS(file) {
      // 上传
      const succesCon = JSON.parse(file.xhr.response)
      // 资质图片
      if (succesCon.code === '200') {
        this.temp.LogoImageUrl = succesCon.data.Url
        this.$message({ message: '上传成功', type: 'success' })
      } else {
        this.$message({ message: '上传失败', type: 'error' })
      }
      this.createLoading = false
    },
    dropzoneR(file) {
      this.temp.LogoImageUrl = ''
    },
    getList() {
      this.listLoading = true
      fetchList(this.listQuery).then(response => {
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
    handleModifyStatus(row, status) {
      this.$message({
        message: '操作成功',
        type: 'success'
      })
      row.status = status
    },
    shopDetail(row, Id) {
      this.imgAjaxUrl = this.rootUrl + '/Upload/'
      getDetail(Id).then(response => {
        this.dialogDetailVisible = true
        this.detailCon = response.data.data
      })
    },
    // 删除物业公司
    delJdbBtn(row, Id) {
      this.$confirm('确定要注销吗?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'delete'
      }).then(() => {
        delJdbRequest(Id).then(response => {
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
        Phone: '',
        Address: '',
        Description: '',
        LogoImageUrl: ''
      }
    },
    // 登记物业公司 按钮
    handleCreate() {
      this.imgAjaxUrl = ''
      this.createLoading = false
      this.resetTemp()
      this.dialogStatus = 'create'
      this.dialogCreatShow = true
      this.$nextTick(() => {
        this.$refs['dataForm'].clearValidate()
      })
    },
    // 新建物业公司
    createData() {
      this.$refs['dataForm'].validate((valid) => {
        if (valid) {
          this.num = this.num + 1
          if (this.num < 2) {
            this.createLoading = true
            createShop(this.temp).then((response) => {
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
      // 编辑物业公司
      // getDetail
      this.imgAjaxUrl = this.rootUrl + '/Upload/'
      getDetail(Id).then(response => {
        // this.dialogDetailVisible = true
        const detailCon = response.data.data
        detailCon.QualificationImageId = '未修改'
        detailCon.Id = Id
        this.createLoading = false
        this.temp = Object.assign({}, detailCon) // copy obj
        // this.temp.timestamp = new Date(this.temp.timestamp)
        this.dialogStatus = 'update'
        this.dialogCreatShow = true
        this.$nextTick(() => {
          this.$refs['dataForm'].clearValidate()
        })
      })
    },
    updateData() {
      this.$refs['dataForm'].validate((valid) => {
        if (valid) {
          const tempData = Object.assign({}, this.temp)
          this.num = this.num + 1
          if (this.num < 2) {
            this.createLoading = true
            updateShop(tempData).then(() => {
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

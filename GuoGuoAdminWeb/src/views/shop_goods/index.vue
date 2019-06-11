<template>
  <div class="app-container">
    <div class="filter-container">
      <el-input v-model="listQuery.Name" placeholder="名称" style="width: 200px;" class="filter-item" @keyup.enter.native="handleFilter"/>
      <el-input v-model="listQuery.barCode" placeholder="商品条形码" style="width: 200px;" class="filter-item" @keyup.enter.native="handleFilter"/>
      <el-select v-model="listQuery.typeId" placeholder="商品类别" clearable style="width: 200px" class="filter-item" @change="handleFilter()">
        <el-option v-for="item in typeList" :key="item.Name" :label="item.Name" :value="item.Id"/>
      </el-select>
      <el-select v-model="listQuery.salesTypeValue" placeholder="销售状态" clearable style="width: 200px" class="filter-item" @change="handleFilter()">
        <el-option v-for="item in goodType" :key="item.Name" :label="item.Name" :value="item.Value"/>
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
      <el-table-column label="商品名称" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.Name }}</span>
        </template>
      </el-table-column>
      <el-table-column label="商品条形码" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.BarCode }}</span>
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
      <el-table-column label="商品类别" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.TypeName }}</span>
        </template>
      </el-table-column>
      <el-table-column label="销售状态" align="center">
        <template slot-scope="scope">
          <!-- <span>{{ scope.row.SalesTypeName }}</span> -->
          <el-tag v-show="scope.row.SalesTypeValue=='Obtained'" type="danger"> 下架 </el-tag>
          <el-tag v-show="scope.row.SalesTypeValue=='Shelf'" type="warning"> 上架 </el-tag>
          <!-- <el-button v-show="scope.row.SalesTypeValue=='Obtained'" type="primary" size="mini" plain @click="goodsXj(scope.row.Id)">下架</el-button>
          <el-button v-show="scope.row.SalesTypeValue=='Shelf'" type="success" size="mini" plain @click="goodsSj(scope.row.Id)">上架</el-button> -->
        </template>
      </el-table-column>
      <el-table-column label="上/下架" align="center">
        <template slot-scope="scope">
          <el-button v-show="scope.row.SalesTypeValue=='Shelf'" type="danger" size="mini" round style="margin-left:0" @click="goodsXj(scope.row.Id)">下架</el-button>
          <el-button v-show="scope.row.SalesTypeValue=='Obtained'" type="warning" size="mini" round style="margin-left:0" @click="goodsSj(scope.row.Id)">上架</el-button>
        </template>
      </el-table-column>
      <el-table-column label="查看" align="center">
        <template slot-scope="scope">
          <el-button size="mini" type="success" @click="shopDetail(scope.row, scope.row.Id)">详情</el-button>
        </template>
      </el-table-column>
      <el-table-column :label="$t('table.actions')" align="center" class-name="small-padding fixed-width">
        <template slot-scope="scope">
          <el-button type="primary" size="mini" @click="handleUpdate(scope.row, scope.row.Id)">{{ $t('table.edit') }}</el-button>
          <el-button v-if="scope.row.status!='deleted'" size="mini" type="danger" @click="delJdbBtn(scope.row, scope.row.Id)">{{ $t('table.logoutBtn') }}
          </el-button>
        </template>
      </el-table-column>
    </el-table>

    <pagination v-show="total>0" :total="total" :page.sync="listQuery.pageIndex" :limit.sync="listQuery.pageSize" @pagination="getList"/>

    <el-dialog :title="textMap[dialogStatus]" :visible.sync="dialogCreatShow">
      <el-form ref="dataForm" :rules="rules" :model="temp" label-position="left" label-width="120px" style="width: 100%;box-sizing:border-box; padding:0 50px;">
        <el-form-item v-show="dialogStatus==='create'" label="条形码" prop="BarCode">
          <el-input v-model="temp.BarCode" style="width:80%;"/>
          <el-button v-waves class="filter-item" type="primary" icon="el-icon-search" style="width:15%;" @click="searchGoods">{{ $t('table.search') }}</el-button>
        </el-form-item>
        <el-form-item label="名称" prop="Name">
          <el-input v-model="temp.Name"/>
        </el-form-item>
        <el-form-item label="价格" prop="Price">
          <el-input v-model="temp.Price"/>
        </el-form-item>
        <el-form-item label="销售价" prop="DiscountPrice">
          <el-input v-model="temp.DiscountPrice"/>
        </el-form-item>
        <el-form-item label="库存数量" prop="CommodityStocks">
          <el-input v-model="temp.CommodityStocks"/>
        </el-form-item>
        <el-form-item label="商品描述" prop="Description">
          <el-input v-model="temp.Description"/>
        </el-form-item>
        <el-form-item label="类别" prop="TypeId">
          <el-select v-model="temp.TypeId" placeholder="类别" style="width:100%">
            <el-option v-for="item in typeList" :key="item.Name" :label="item.Name" :value="item.Id"/>
          </el-select>
        </el-form-item>
        <el-form-item label="排序" prop="Sort">
          <el-select v-model="temp.Sort" placeholder="排序" style="width:100%">
            <el-option v-for="item in sortList" :key="item" :label="item" :value="item"/>
          </el-select>
        </el-form-item>
        <el-form-item label="商品图片" prop="LogoImageId">
          <div class="editor-container">
            <dropzone v-if="dialogCreatShow" id="myVueDropzone" :url="rootUrl+'/api/uploadShopCommodity'" :default-img="temp.ImageUrl?[imgAjaxUrl+temp.ImageUrl]:''" @dropzone-fileAdded="addfile" @dropzone-error="imgError" @dropzone-removedFile="dropzoneR" @dropzone-success="dropzoneS" />
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
          <div>商品名称</div>
          <div>{{ detailCon.Name }}</div>
        </li>
        <li class="clearfloat">
          <div>商品条形码</div>
          <div>{{ detailCon.BarCode }}</div>
        </li>
        <li class="clearfloat">
          <div>价格</div>
          <div>{{ detailCon.Price }}</div>
        </li>
        <li class="clearfloat">
          <div>销售价</div>
          <div>{{ detailCon.DiscountPrice }}</div>
        </li>
        <li class="clearfloat">
          <div>排序</div>
          <div>{{ detailCon.Sort }}</div>
        </li>
        <li class="clearfloat">
          <div>库存数量</div>
          <div>{{ detailCon.CommodityStocks }}</div>
        </li>
        <li class="clearfloat">
          <div>描述</div>
          <div>{{ detailCon.Description }}</div>
        </li>
        <li class="clearfloat">
          <div>类别</div>
          <div>{{ detailCon.TypeName }}</div>
        </li>
        <li class="clearfloat">
          <div>销售状态</div>
          <div>{{ detailCon.SalesTypeName }}</div>
        </li>
        <li class="clearfloat">
          <div>商品图片</div>
          <div>
            <img :src="imgAjaxUrl+detailCon.ImageUrl" alt="">
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
// import { mapGetters } from 'vuex'
import { fetchList, createShop, updateShop, delRequest, getTypeList, getDetail, goodsXj, goodsSj, serachGoodsMes } from '@/api/shopGoods'
import waves from '@/directive/waves' // Waves directive
import Pagination from '@/components/Pagination' // Secondary package based on el-pagination
import Dropzone from '@/components/Dropzone'

export default {
  name: 'ComplexTable',
  components: { Pagination, Dropzone },
  directives: { waves },
  data() {
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
      temp: {
        BarCode: '',
        Name: '',
        Sort: '',
        Price: '',
        DiscountPrice: '',
        TypeId: '',
        CommodityStocks: '',
        Description: '',
        ImageUrl: ''
      },
      sortList: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10],
      dialogCreatShow: false,
      dialogStatus: '',
      textMap: {
        update: '编辑商品',
        create: '新建商品'
      },
      rules: {
        BarCode: [{ required: true, message: '商品条形码是必填的', trigger: 'blur' }],
        Name: [{ required: true, message: '商品名称是必填的', trigger: 'blur' }],
        Sort: [{ required: true, message: '排序是必选的', trigger: 'change' }],
        Price: [{ required: true, message: '商品价格是必填的', trigger: 'blur' }],
        DiscountPrice: [{ required: true, message: '商品销售价是必填的', trigger: 'blur' }],
        TypeId: [{ required: true, message: '商品类别是必填的', trigger: 'change' }]
      },
      createLoading: false,
      num: 0,
      typeList: [],
      goodType: [
        {
          Name: '上架',
          Value: 'Shelf'
        },
        {
          Name: '下架',
          Value: 'Obtained'
        }
      ]
    }
  },
  created() {
    this.getList()
    this.getType()
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
        this.temp.ImageUrl = succesCon.data.Url
        this.$message({ message: '上传成功', type: 'success' })
      } else {
        this.$message({ message: '上传失败', type: 'error' })
      }
      this.createLoading = false
    },
    dropzoneR(file) {
      this.temp.ImageUrl = ''
    },
    getList() {
      this.listLoading = true
      this.listQuery.shopId = this.$store.getters.loginuser.ShopId
      fetchList(this.listQuery).then(response => {
        this.list = response.data.data.List
        this.total = response.data.data.TotalCount
        setTimeout(() => {
          this.listLoading = false
        }, 1.5 * 1000)
      })
    },
    searchGoods() {
      if (this.temp.BarCode) {
        this.imgAjaxUrl = this.rootUrl + '/Upload/'
        serachGoodsMes(this.temp.BarCode).then(response => {
          this.temp.Name = response.data.data.Name
          this.temp.Price = response.data.data.Price
          this.temp.DiscountPrice = response.data.data.Price
          // this.temp.ImageUrl = response.data.data.ImageUrl ? [ this.rootUrl + '/Upload/' + response.data.data.ImageUrl ]:''
          this.temp.ImageUrl = response.data.data.ImageUrl
        })
      } else {
        this.$message({
          message: '条形码不能为空',
          type: 'error',
          duration: 3 * 1000
        })
      }
    },
    getType() {
      getTypeList(this.$store.getters.loginuser.ShopId).then(response => {
        this.typeList = response.data.data
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
    // 删除商品
    delJdbBtn(row, Id) {
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
    goodsXj(Id) {
      this.$confirm('确定要下架吗?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'delete'
      }).then(() => {
        goodsXj(Id).then(response => {
          this.getList()
          this.$notify({
            title: '成功',
            message: '下架成功',
            type: 'success',
            duration: 2000
          })
        })
      }).catch(() => {
      })
    },
    goodsSj(Id) {
      this.$confirm('确定要上架吗?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'delete'
      }).then(() => {
        goodsSj(Id).then(response => {
          this.getList()
          this.$notify({
            title: '成功',
            message: '上架成功',
            type: 'success',
            duration: 2000
          })
        })
      }).catch(() => {
      })
    },
    shopDetail(row, Id) {
      this.imgAjaxUrl = this.rootUrl + '/Upload/'
      getDetail(Id).then(response => {
        this.dialogDetailVisible = true
        this.detailCon = response.data.data
      })
    },
    resetTemp() {
      this.temp = {
        BarCode: '',
        Name: '',
        Sort: '',
        Price: '',
        DiscountPrice: '',
        TypeId: '',
        CommodityStocks: '',
        Description: '',
        ImageUrl: ''
      }
    },
    // 登记商品 按钮
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
    // 新建商品
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
      this.imgAjaxUrl = this.rootUrl + '/Upload/'
      getDetail(Id).then(response => {
        // this.dialogDetailVisible = true
        const detailCon = response.data.data
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
  width: 20%;
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

<template>
  <div class="app-container">
    <div class="filter-container">
      <el-input v-model="listQuery.number" placeholder="订单编号" style="width: 200px;" class="filter-item" @keyup.enter.native="handleFilter"/>
      <el-select v-model="listQuery.orderStatusValue" placeholder="订单状态" clearable style="width: 200px" class="filter-item" @change="handleFilter()">
        <el-option v-for="item in typeList" :key="item.id" :label="item.name" :value="item.id"/>
      </el-select>
      <el-button v-waves class="filter-item" type="primary" icon="el-icon-search" @click="handleFilter">{{ $t('table.search') }}</el-button>
    </div>

    <el-table
      v-loading="listLoading"
      :key="tableKey"
      :data="list"
      border
      fit
      highlight-current-row
      style="width: 100%;">
      <el-table-column label="编号" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.Number }}</span>
        </template>
      </el-table-column>
      <el-table-column label="订单创建时间" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.CreateTime }}</span>
        </template>
      </el-table-column>
      <el-table-column label="订单状态" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.OrderStatusName }}</span>
        </template>
      </el-table-column>
      <el-table-column label="订单金额" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.PaymentPrice }}</span>
        </template>
      </el-table-column>
      <el-table-column label="商品数量" align="center">
        <template slot-scope="scope">
          <span>{{ scope.row.ShopCommodityCount }}</span>
        </template>
      </el-table-column>
      <el-table-column :label="$t('table.actions')" align="center" class-name="small-padding fixed-width">
        <template slot-scope="scope">
          <el-button v-if="scope.row.IsBtnDisplay" type="warning" size="mini" @click="sendShop(scope.row, scope.row.Id)"> 发货 </el-button>
          <el-button v-if="scope.row.IsAcceptBtnDisplay" type="primary" size="mini" @click="jdShop(scope.row, scope.row.Id)"> 接单 </el-button>
          <el-button size="mini" type="success" @click="orderDetail(scope.row, scope.row.Id)"> 详情 </el-button>
        </template>
      </el-table-column>
    </el-table>

    <pagination v-show="total>0" :total="total" :page.sync="listQuery.pageIndex" :limit.sync="listQuery.pageSize" @pagination="getList"/>

    <el-dialog :visible.sync="dialogDetailVisible" title="详情">
      <ul class="shopDetail">
        <li class="clearfloat">
          <div>订单状态</div>
          <div>{{ detailCon.OrderStatusName }}</div>
        </li>
        <li class="clearfloat">
          <div>订单编号</div>
          <div>{{ detailCon.Number }}</div>
        </li>
        <li class="clearfloat">
          <div>配送物业</div>
          <div>{{ detailCon.DeliveryName }}</div>
        </li>
        <li class="clearfloat">
          <div>物业电话</div>
          <div>{{ detailCon.DeliveryPhone }}</div>
        </li>
        <li class="clearfloat">
          <div>收货姓名</div>
          <div>{{ detailCon.ReceiverName }}</div>
        </li>
        <li class="clearfloat">
          <div>收货电话</div>
          <div>{{ detailCon.ReceiverPhone }}</div>
        </li>
        <li class="clearfloat">
          <div>收货地址</div>
          <div>{{ detailCon.Address }}</div>
        </li>
        <li class="clearfloat">
          <div>商家名称</div>
          <div>{{ detailCon.ShopName }}</div>
        </li>
        <li class="clearfloat">
          <div>商品总价</div>
          <div>￥ {{ detailCon.ShopCommodityPrice }}</div>
        </li>
        <li class="clearfloat">
          <div>商品数量</div>
          <div>{{ detailCon.ShopCommodityCount }}</div>
        </li>
        <li class="clearfloat">
          <div>配送费</div>
          <div>￥ {{ detailCon.Postage }}</div>
        </li>
        <li class="clearfloat">
          <div>订单总金额</div>
          <div>￥ {{ detailCon.PaymentPrice }}</div>
        </li>
        <li class="clearfloat">
          <div>商品列表</div>
          <div>
            <el-table
              v-loading="listLoading"
              :key="tableKey"
              :data="detailCon.List"
              border
              fit
              highlight-current-row
              style="width: 100%;">
              <el-table-column label="名称" align="center">
                <template slot-scope="scope">
                  <span>{{ scope.row.Name }}</span>
                </template>
              </el-table-column>
              <el-table-column label="单价" align="center">
                <template slot-scope="scope">
                  <span>￥ {{ scope.row.DiscountPrice }}</span>
                </template>
              </el-table-column>
              <el-table-column label="数量" align="center">
                <template slot-scope="scope">
                  <span>{{ scope.row.CommodityCount }}</span>
                </template>
              </el-table-column>
              <el-table-column label="商品图片" align="center">
                <template slot-scope="scope">
                  <img :src="imgAjaxUrl+scope.row.ImageUrl" alt="" @click="$imgPreview(imgAjaxUrl+scope.row.ImageUrl)">
                </template>
              </el-table-column>
            </el-table>
          </div>
        </li>
      </ul>
      <span slot="footer" class="dialog-footer">
        <el-button type="warning" @click="printBtn">打印</el-button>
        <el-button type="primary" @click="dialogDetailVisible = false">{{ $t('table.confirm') }}</el-button>
      </span>
    </el-dialog>
    <el-dialog :visible.sync="dialogImg" title="图片预览" style="padding:0;margin:0">
      <div class="components-container" style="padding:0;margin:0">
        <div class="editor-container" style="padding:0;margin:0">
          <img-preview :nowimg="nowimg"/>
        </div>
      </div>
    </el-dialog>
  </div>
</template>

<script>
// import { mapGetters } from 'vuex'
import { fetchList, sendShopRequest, getDetail, jdShopRequest } from '@/api/shopOrder'
import waves from '@/directive/waves' // Waves directive
import Pagination from '@/components/Pagination' // Secondary package based on el-pagination
import Dropzone from '@/components/Dropzone'
import imgPreview from '@/components/imgPreview'
import { getLodop } from '@/api/LodopFuncs'

export default {
  name: 'ComplexTable',
  components: { Pagination, Dropzone, imgPreview },
  directives: { waves },
  data() {
    return {
      dialogImg: false,
      nowimg: '',
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
        Name: '',
        Sort: ''
      },
      sortList: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10],
      dialogCreatShow: false,
      dialogStatus: '',
      textMap: {
        update: '编辑商铺',
        create: '新建商铺'
      },
      rules: {
        Name: [{ required: true, message: '商铺名称是必填的', trigger: 'blur' }],
        Sort: [{ required: true, message: '排序是必选的', trigger: 'change' }]
      },
      createLoading: false,
      num: 0,
      typeList: [
        {
          id: 'WaitingSend',
          name: '待配送'
        },
        {
          id: 'WaitingTake',
          name: '待配货'
        },
        {
          id: 'WaitingReceive',
          name: '待收货'
        },
        {
          id: 'Finish',
          name: '已完成'
        },
        {
          id: 'WaitingAccept',
          name: '待接单'
        }
      ],
      imgAjaxUrl: ''
    }
  },
  created() {
    this.getList()
    this.imgAjaxUrl = process.env.API_HOST + '/Upload/'
    var that = this
    window.refrushorderlist = function(){
      that.getList()
    }
    console.log('PrinterName',this.$store.getters.loginuser.PrinterName )
  },
  beforeDestroy(){
    window.refrushorderlist = null
  },
  methods: {
    $imgPreview(url) {
      this.dialogImg = true
      this.nowimg = url
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
    orderDetail(row, Id) {
      getDetail(Id).then(response => {
        this.dialogDetailVisible = true
        this.detailCon = response.data.data
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
    // 发货
    sendShop(row, Id) {
      this.$confirm('确定要发货吗?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'delete'
      }).then(() => {
        sendShopRequest(Id).then(response => {
          this.getList()
          this.$notify({
            title: '发货成功',
            message: '配送员已成功取到商品',
            type: 'success',
            duration: 2000
          })
        })
      }).catch(() => {
      })
    },
    // 接单
    jdShop(row, Id) {
      this.$confirm('确定要接单吗?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'delete'
      }).then(() => {
        jdShopRequest(Id).then(response => {
          this.getList()
          getDetail(Id).then(response => {
            this.detailCon = response.data.data
            if (this.$store.getters.loginuser.PrinterName) {
              // 打印
              const LODOP = getLodop()
              console.log(this.$store.getters.loginuser.PrinterName)
              // 设置默认打印机
              // LODOP.SET_PRINTER_INDEXA(this.$store.getters.loginuser.PrinterName, true)
              // 小票上边距
              var hPos = 2
              // 小票宽度
              var pageWidth = '58mm'
              var pageWidth2 = '54mm'
              // 小票行距
              var rowHeight = 15
              // 初始化
              LODOP.PRINT_INIT('')
              // this.detailCon 订单信息
              LODOP.ADD_PRINT_TEXT(hPos, 1, pageWidth2, 40 * Math.ceil(this.detailCon.ShopName.length / 9), this.detailCon.ShopName)
              LODOP.SET_PRINT_STYLEA(0, 'Alignment', 2);
              // LODOP.SET_PRINT_STYLEA(0, 'itemtype', 1)
              LODOP.SET_PRINT_STYLEA(0, 'fontsize', 12)
              LODOP.SET_PRINT_STYLEA(0, 'bold', 1)

              hPos += 40
              LODOP.ADD_PRINT_TEXT(hPos, 1, pageWidth2, rowHeight, '[订单时间]' + this.detailCon.CreateTime)
              LODOP.SET_PRINT_STYLEA(0, 'Alignment', 2);
              // LODOP.SET_PRINT_STYLEA(0, 'itemtype', 6)
              LODOP.SET_PRINT_STYLEA(0, 'fontsize', 8)
              LODOP.SET_PRINT_STYLEA(0, 'FontColor', '#000')
              LODOP.SET_PRINT_STYLEA(0, 'FontName', '黑体')
              hPos += 25
              LODOP.ADD_PRINT_TEXT(hPos, 1, pageWidth2, rowHeight, '[订单编号]' + this.detailCon.Number)
              LODOP.SET_PRINT_STYLEA(0, 'Alignment', 2);
              LODOP.SET_PRINT_STYLEA(0, 'fontsize', 8)
              LODOP.SET_PRINT_STYLEA(0, 'FontColor', '#000')
              LODOP.SET_PRINT_STYLEA(0, 'FontName', '黑体')
              hPos += 25

              LODOP.ADD_PRINT_LINE(hPos, 2, hPos, pageWidth2, 4, 1)
              hPos += 5
              for (var i = 0; i < this.detailCon.List.length; i++) {
                LODOP.ADD_PRINT_TEXT(hPos, 1, pageWidth2, 28 * Math.ceil(this.detailCon.List[i].Name.length / 9), this.detailCon.List[i].Name)
                LODOP.SET_PRINT_STYLEA(0, 'fontsize', 14)
                LODOP.SET_PRINT_STYLEA(0, 'FontColor', '#000')
                LODOP.SET_PRINT_STYLEA(0, 'FontName', '黑体')
                LODOP.SET_PRINT_STYLEA(0, 'bold', 1)
                // LODOP.SET_PRINT_STYLEA(0, 'TextNeatRow', true)
                hPos += (30 * Math.ceil(this.detailCon.List[i].Name.length / 9))
                LODOP.ADD_PRINT_TEXT(hPos, 1, pageWidth2, 30, ' x' + this.detailCon.List[i].CommodityCount + ' ￥' + this.detailCon.List[i].DiscountPrice)
                LODOP.SET_PRINT_STYLEA(0, 'fontsize', 14)
                LODOP.SET_PRINT_STYLEA(0, 'FontName', '黑体')
                LODOP.SET_PRINT_STYLEA(0, 'FontColor', '#000')
                hPos += 30
              }
              hPos += 10
              LODOP.ADD_PRINT_TEXT(hPos, 1, pageWidth2, rowHeight, '总价：' + this.detailCon.PaymentPrice + '元(配送费' + this.detailCon.Postage + '元)')
              LODOP.SET_PRINT_STYLEA(0, 'fontsize', 10)
              LODOP.SET_PRINT_STYLEA(0, 'FontColor', '#000')
              LODOP.SET_PRINT_STYLEA(0, 'FontName', '黑体')
              LODOP.SET_PRINT_STYLEA(0, 'bold', 1)
              hPos += 22
              LODOP.ADD_PRINT_LINE(hPos, 2, hPos, pageWidth2, 4, 1)
              hPos += 10
              LODOP.ADD_PRINT_TEXT(hPos, 1, pageWidth2, rowHeight, '[客户姓名]' + this.detailCon.ReceiverName)
              LODOP.SET_PRINT_STYLEA(0, 'fontsize', 10)
              LODOP.SET_PRINT_STYLEA(0, 'bold', 1)
              hPos += 22
              LODOP.ADD_PRINT_TEXT(hPos, 1, pageWidth2, rowHeight, '[客户电话]' + this.detailCon.ReceiverPhone)
              LODOP.SET_PRINT_STYLEA(0, 'fontsize', 10)
              LODOP.SET_PRINT_STYLEA(0, 'bold', 1)
              hPos += 22
              LODOP.ADD_PRINT_TEXT(hPos, 1, pageWidth2, 25 * Math.ceil((Number(this.detailCon.Address.length) + Number(4)) / 16), '[收货地址]' + this.detailCon.Address)
              LODOP.SET_PRINT_STYLEA(0, 'fontsize', 10)
              LODOP.SET_PRINT_STYLEA(0, 'bold', 1)
              hPos += (25*Math.ceil((Number(this.detailCon.Address.length) + Number(4)) / 14))
              LODOP.ADD_PRINT_TEXT(hPos, 1, pageWidth2, rowHeight, ' ')
              // 初始化打印页的规格
              LODOP.SET_PRINT_PAGESIZE(3, pageWidth, 70, '')
              // LODOP.PREVIEW()
              if (LODOP.SET_PRINTER_INDEXA(this.$store.getters.loginuser.PrinterName)){
                //开始打印
                LODOP.PRINT();
              }
            } else {
              this.$message({
                message: '暂无可使用打印机，请去商铺中心设置打印机名称',
                type: 'error',
                duration: 3 * 1000
              })
            }
          })
          this.$notify({
            title: '接单成功',
            message: '请准备货品，配送员很快会来取货呦',
            type: 'success',
            duration: 2000
          })
        })
      }).catch(() => {
      })
    },
    printBtn() {
      if (this.$store.getters.loginuser.PrinterName) {
        // 打印
        const LODOP = getLodop()
        console.log(this.$store.getters.loginuser.PrinterName)
        // 设置默认打印机
        // LODOP.SET_PRINTER_INDEXA(this.$store.getters.loginuser.PrinterName, true)
        // 小票上边距
        var hPos = 2
        // 小票宽度
        var pageWidth = '58mm'
        var pageWidth2 = '54mm'
        // 小票行距
        var rowHeight = 15
        // 初始化
        LODOP.PRINT_INIT('')
        // this.detailCon 订单信息
        LODOP.ADD_PRINT_TEXT(hPos, 1, pageWidth2, 40 * Math.ceil(this.detailCon.ShopName.length / 9), this.detailCon.ShopName)
        LODOP.SET_PRINT_STYLEA(0, 'Alignment', 2);
        // LODOP.SET_PRINT_STYLEA(0, 'itemtype', 1)
        LODOP.SET_PRINT_STYLEA(0, 'fontsize', 12)
        LODOP.SET_PRINT_STYLEA(0, 'bold', 1)

        hPos += 40
        LODOP.ADD_PRINT_TEXT(hPos, 1, pageWidth2, rowHeight, '[订单时间]' + this.detailCon.CreateTime)
        LODOP.SET_PRINT_STYLEA(0, 'Alignment', 2);
        // LODOP.SET_PRINT_STYLEA(0, 'itemtype', 6)
        LODOP.SET_PRINT_STYLEA(0, 'fontsize', 8)
        LODOP.SET_PRINT_STYLEA(0, 'FontColor', '#000')
        LODOP.SET_PRINT_STYLEA(0, 'FontName', '黑体')
        hPos += 25
        LODOP.ADD_PRINT_TEXT(hPos, 1, pageWidth2, rowHeight, '[订单编号]' + this.detailCon.Number)
        LODOP.SET_PRINT_STYLEA(0, 'Alignment', 2);
        LODOP.SET_PRINT_STYLEA(0, 'fontsize', 8)
        LODOP.SET_PRINT_STYLEA(0, 'FontColor', '#000')
        LODOP.SET_PRINT_STYLEA(0, 'FontName', '黑体')
        hPos += 25

        LODOP.ADD_PRINT_LINE(hPos, 2, hPos, pageWidth2, 4, 1)
        hPos += 5
        for (var i = 0; i < this.detailCon.List.length; i++) {
          LODOP.ADD_PRINT_TEXT(hPos, 1, pageWidth2, 28 * Math.ceil(this.detailCon.List[i].Name.length / 9), this.detailCon.List[i].Name)
          LODOP.SET_PRINT_STYLEA(0, 'fontsize', 14)
          LODOP.SET_PRINT_STYLEA(0, 'FontColor', '#000')
          LODOP.SET_PRINT_STYLEA(0, 'FontName', '黑体')
          LODOP.SET_PRINT_STYLEA(0, 'bold', 1)
          // LODOP.SET_PRINT_STYLEA(0, 'TextNeatRow', true)
          hPos += (30 * Math.ceil(this.detailCon.List[i].Name.length / 9))
          LODOP.ADD_PRINT_TEXT(hPos, 1, pageWidth2, 30, ' x' + this.detailCon.List[i].CommodityCount + ' ￥' + this.detailCon.List[i].DiscountPrice)
          LODOP.SET_PRINT_STYLEA(0, 'fontsize', 14)
          LODOP.SET_PRINT_STYLEA(0, 'FontName', '黑体')
          LODOP.SET_PRINT_STYLEA(0, 'FontColor', '#000')
          hPos += 30
        }
        hPos += 10
        LODOP.ADD_PRINT_TEXT(hPos, 1, pageWidth2, rowHeight, '总价：' + this.detailCon.PaymentPrice + '元(配送费' + this.detailCon.Postage + '元)')
        LODOP.SET_PRINT_STYLEA(0, 'fontsize', 10)
        LODOP.SET_PRINT_STYLEA(0, 'FontColor', '#000')
        LODOP.SET_PRINT_STYLEA(0, 'FontName', '黑体')
        LODOP.SET_PRINT_STYLEA(0, 'bold', 1)
        hPos += 22
        LODOP.ADD_PRINT_LINE(hPos, 2, hPos, pageWidth2, 4, 1)
        hPos += 10
        LODOP.ADD_PRINT_TEXT(hPos, 1, pageWidth2, rowHeight, '[客户姓名]' + this.detailCon.ReceiverName)
        LODOP.SET_PRINT_STYLEA(0, 'fontsize', 10)
        LODOP.SET_PRINT_STYLEA(0, 'bold', 1)
        hPos += 22
        LODOP.ADD_PRINT_TEXT(hPos, 1, pageWidth2, rowHeight, '[客户电话]' + this.detailCon.ReceiverPhone)
        LODOP.SET_PRINT_STYLEA(0, 'fontsize', 10)
        LODOP.SET_PRINT_STYLEA(0, 'bold', 1)
        hPos += 22
        LODOP.ADD_PRINT_TEXT(hPos, 1, pageWidth2, 25 * Math.ceil((Number(this.detailCon.Address.length) + Number(4)) / 16), '[收货地址]' + this.detailCon.Address)
        LODOP.SET_PRINT_STYLEA(0, 'fontsize', 10)
        LODOP.SET_PRINT_STYLEA(0, 'bold', 1)
        hPos += (25*Math.ceil((Number(this.detailCon.Address.length) + Number(4)) / 14))
        LODOP.ADD_PRINT_TEXT(hPos, 1, pageWidth2, rowHeight, ' ')
        // 初始化打印页的规格
        LODOP.SET_PRINT_PAGESIZE(3, pageWidth, 70, '')
        // LODOP.PREVIEW()
        if (LODOP.SET_PRINTER_INDEXA(this.$store.getters.loginuser.PrinterName)){
          //开始打印
          LODOP.PRINT();
        }
      } else {
        this.$message({
          message: '暂无可使用打印机，请去商铺中心设置打印机名称',
          type: 'error',
          duration: 3 * 1000
        })
      }
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

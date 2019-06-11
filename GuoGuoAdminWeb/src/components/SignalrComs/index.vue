<template>
  <div>
    <el-dialog :visible.sync="dialogVisible" title="订单详情">
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
      <div slot="footer" class="dialog-footer">
        <el-button @click="dialogVisible = false">{{ $t('table.cancel') }}</el-button>
        <el-button v-loading="createLoading" v-if="detailCon.OrderStatusValue=='WaitingAccept'&&loginuser.DepartmentValue=='Shop'" type="primary" @click="jdShop">接单</el-button>
        <el-button v-loading="createLoading" v-if="detailCon.OrderStatusValue=='WaitingSend'&&loginuser.DepartmentValue=='Property'" type="primary" @click="getGoods">取货</el-button>
        <el-button v-loading="createLoading" v-if="detailCon.OrderStatusValue=='WaitingTake'&&loginuser.DepartmentValue=='Shop'" type="primary" @click="fhGoods">发货</el-button>
      </div>
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
import { mapGetters } from 'vuex'
import { getDetail, jdShopRequest, getGoodsRequest, fhGoodsRequest } from '@/api/orderAll'
import imgPreview from '@/components/imgPreview'
import { getLodop } from '@/api/LodopFuncs'
export default {
  components: { imgPreview },
  props: {
    apiurl: {
      type: String,
      required: true
    },
    source: {
      type: String,
      default: '1'
    },
    comid: {
      type: String,
      default: ''
    },
    eid: {
      type: String,
      default: '1'
    }
  },
  data() {
    return {
      dialogVisible: false, // 是否展示出弹出层
      temp: {},
      list: [],
      chat: null,
      listLoading: false,
      tableKey: 'orderinfo',
      createLoading: false,
      detailCon: [],
      orderID: '',
      dialogImg: false,
      nowimg: ''
    }
  },
  computed: {
    ...mapGetters([
      'loginuser'
    ])
    // this.$store.getters.province
  },
  mounted() {
    console.log('comid', this.comid)
    var that = this
    if (this.comid !== '') {
      this.chat = this.initconnect()
    }
    this.imgAjaxUrl = this.apiurl + '/Upload/'
  },
  methods: {
    $imgPreview(url) {
      this.dialogImg = true
      this.nowimg = url
    },
    initconnect() {
      var self = this
      const hub = jQuery.hubConnection(self.apiurl)
      const proxy = hub.createHubProxy('SignalRServerHub')
      proxy.on('getorderinfo', function(message) {
        console.log('message', message)
        self.$notify({
          title: message.CreateOperationTime,
          dangerouslyUseHTMLString: true,
          message: '<strong>您有新的订单</strong>',
          duration: 0,
          onClick() {
            const that = this
            getDetail(message.Id).then(response => {
              self.orderID = message.Id
              self.detailCon = response.data.data
              self.dialogVisible = true
              that.close()
            })
          }
        })
        if (window.refrushorderlist) {
          window.refrushorderlist()
        }
      })
      hub.start().done((connection) => {
        console.log('Now connected, connection ID=' + connection.id)
        // proxy.init();
        //  1 物业 2商朝
        // PropertyCompanyId  物业公司Id
        // shopId shang
        proxy.invoke('sendloginmsg', { source: this.source, companyID: this.comid, employeeid: this.eid })
      }).fail(() => {
        console.log('Could not connect')
      })
      hub.error(function(error) {
        console.log('SignalR error: ' + error)
      })
      hub.connectionSlow(function() {
        console.log('We are currently experiencing difficulties with the connection.')
      })
      hub.disconnected(function() {
        console.log('disconnected')
      })
      return proxy
    },
    getinfo() {

    },
    handleShow() {

    },
    // 接单
    jdShop() {
      this.$confirm('确定要接单吗?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'delete'
      }).then(() => {
        jdShopRequest(this.orderID).then(response => {
          if (window.refrushorderlist) {
            window.refrushorderlist()
          }
          this.$notify({
            title: '接单成功',
            message: '请准备货品，配送员很快会来取货呦',
            type: 'success',
            duration: 2000
          })
          this.dialogVisible = false
          // 打印
          const LODOP = getLodop()
          if (this.loginuser.PrinterName) {
            // LODOP.SET_PRINT_MODE(this.loginuser.PrinterName, true)
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
              console.log(Math.ceil(this.detailCon.List[i].Name.length / 9))
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
      }).catch(() => {
      })
    },
    // 取货
    getGoods() {
      this.$confirm('确定取货吗?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'delete'
      }).then(() => {
        getGoodsRequest(this.orderID).then(response => {
          if (window.refrushorderlist) {
            window.refrushorderlist()
          }
          this.dialogVisible = false
          this.$notify({
            title: '状态更改成功',
            message: '配送员已出发',
            type: 'success',
            duration: 2000
          })
        })
      }).catch(() => {
      })
    },
    fhGoods() {
      this.$confirm('确定发货吗?', '提示', {
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        type: 'delete'
      }).then(() => {
        fhGoodsRequest(this.orderID).then(response => {
          if (window.refrushorderlist) {
            window.refrushorderlist()
          }
          this.dialogVisible = false
          this.$notify({
            title: '状态更改成功',
            message: '货品已成功发出',
            type: 'success',
            duration: 2000
          })
        })
      }).catch(() => {
      })
    }
  }
}
</script>

<style scoped>
.shopDetail {
  padding: 0;
  margin: 0;
}
.shopDetail > li {
  list-style: none;
  border-bottom: 1px solid #ddd;
  font-size: 16px;
  box-sizing: border-box;
}
.shopDetail > li > div {
  width: 15%;
  float: left;
  padding: 10px;
}
.shopDetail > li > div:last-child {
  width: 70%;
}
.shopDetail > li > div img {
  width: 100%;
}
.clearfloat:after {
  display: block;
  clear: both;
  content: "";
  visibility: hidden;
  height: 0
}
.clearfloat {
zoom: 1
}
</style>

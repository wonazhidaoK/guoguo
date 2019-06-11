<template>
  <el-form ref="dataForm" :rules="rules" :model="userfrom">
    <el-form-item label="商铺名称" prop="Name">
      <el-input v-model="userfrom.Name" />
    </el-form-item>
    <el-form-item label="商铺手机号" prop="PhoneNumber">
      <el-input v-model="userfrom.PhoneNumber" />
    </el-form-item>
    <el-form-item label="商铺地址" prop="Address">
      <el-input v-model="userfrom.Address" />
    </el-form-item>
    <!-- PrinterName -->
    <el-form-item label="打印机" prop="PrinterName">
      <el-select v-model="userfrom.PrinterName">
        <el-option v-for="item in dyjlist" :key="item.Name" :label="item.Name" :value="item.Name"/>
      </el-select>
    </el-form-item>
    <el-form-item label="商铺描述">
      <el-input v-model="userfrom.Description" />
    </el-form-item>
    <el-form-item label="商铺类别">
      <el-select v-model="userfrom.MerchantCategoryValue" style="width:100%">
        <el-option v-for="item in typelist" :key="item.Name" :label="item.Name" :value="item.Value" />
      </el-select>
    </el-form-item>
    <el-form-item label="Logo图片">
      <br>
      <div class="editor-container">
        <dropzone id="myVueDropzone2" :url="rootUrl+'/api/uploadShop'" :default-img="userfrom.showImg" @dropzone-fileAdded="addfile" @dropzone-error="imgError" @dropzone-removedFile="dropzoneR" @dropzone-success="dropzoneS" />
      </div>
    </el-form-item>
    <el-form-item>
      <el-button v-loading="createLoading" type="primary" @click="submit">提交</el-button>
    </el-form-item>
    <select id="printer" style="display:none" />
  </el-form>
</template>

<script>
import { updateShop } from '@/api/shopinfo'
import Dropzone from '@/components/Dropzone'
import { getLodop } from '@/api/LodopFuncs'

export default {
  components: { Dropzone },
  props: {
    userfrom: {
      type: Object,
      default: () => {
        return {
          PhoneNumber: '',
          Address: '',
          MerchantCategoryValue: '',
          Description: '',
          LogoImageId: '',
          LogoImageUrl: '',
          Name: '',
          PrinterName: '',
          showImg: ''
        }
      }
    },
    typelist: {
      type: Array,
      default: () => {
        return []
      }
    }
  },
  inject: ['getDetail'],
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
      createLoading: false,
      rules: {
        Name: [{ required: true, message: '商家名称是必填的', trigger: 'blur' }],
        Address: [{ required: true, message: '地址是必填的', trigger: 'blur' }],
        PhoneNumber: [{ required: true, trigger: 'blur', validator: checkPhone }],
        PrinterName: [{ required: true, message: '打印机是必选的', trigger: 'change' }]
      },
      rootUrl: '',
      dyjlist: []
    }
  },
  mounted() {
    this.rootUrl = process.env.API_HOST
    console.log(this.rootUrl)
    this.$nextTick(() => {
      this.$refs['dataForm'].clearValidate()
    })
    console.log(this.userfrom.LogoImageUrl)
    var that = this
    setTimeout(function() {
      const LODOP = getLodop()
      if (!LODOP) {
        return
      }
      LODOP.Create_Printer_List(document.getElementById('printer'))
      $('#printer option').each(function(i, item) {
        console.log(i,item)
        // that.dyjlist.push({ Name: $(item).text() })
        that.dyjlist.push({ Name: $(item).text() })
      })
    }, 1000)
  },
  methods: {
    submit() {
      console.log(this.userfrom)
      this.$refs['dataForm'].validate((valid) => {
        if (valid) {
          updateShop(this.userfrom).then(response => {
            this.$store.getters.loginuser.PrinterName = this.userfrom.PrinterName
            this.getDetail()
            this.$message({
              message: '编辑成功',
              type: 'success',
              duration: 5 * 1000
            })
            // this.images.push(response.data.data.LogoImageUrl)
          })
        }
      })
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
      // 资质图片
      if (succesCon.code === '200') {
        this.userfrom.LogoImageUrl = succesCon.data.Url
        this.$message({ message: '上传成功', type: 'success' })
      } else {
        this.$message({ message: '上传失败', type: 'error' })
      }
      this.createLoading = false
    },
    dropzoneR(file) {
      this.userfrom.LogoImageUrl = ''
    }

  }
}
</script>

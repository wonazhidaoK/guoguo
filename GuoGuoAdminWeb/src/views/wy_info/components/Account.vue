<template>
  <el-form ref="dataForm" :rules="rules" :model="userfrom">
    <el-form-item label="物业名称" prop="Name">
      <el-input v-model="userfrom.Name" />
    </el-form-item>
    <el-form-item label="物业手机号" prop="Phone">
      <el-input v-model="userfrom.Phone" />
    </el-form-item>
    <el-form-item label="物业地址" prop="Address">
      <el-input v-model="userfrom.Address" />
    </el-form-item>
    <el-form-item label="物业描述">
      <el-input v-model="userfrom.Description" />
    </el-form-item>
    <el-form-item label="Logo图片">
      <br>
      <div class="editor-container">
        <dropzone id="myVueDropzone2" :url="rootUrl+'/api/uploadPropertyCompany'" :default-img="userfrom.showImg" @dropzone-fileAdded="addfile" @dropzone-error="imgError" @dropzone-removedFile="dropzoneR" @dropzone-success="dropzoneS" />
      </div>
    </el-form-item>
    <el-form-item>
      <el-button v-loading="createLoading" type="primary" @click="submit">提交</el-button>
    </el-form-item>
  </el-form>
</template>

<script>
import { updateWy } from '@/api/wyinfo'
import Dropzone from '@/components/Dropzone'

export default {
  components: { Dropzone },
  props: {
    userfrom: {
      type: Object,
      default: () => {
        return {
          Address: '',
          CreateTime: '',
          Description: '',
          Id: '',
          LogoImageUrl: '',
          Name: '',
          Phone: '',
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
        Address: [{ required: true, message: '地址是必填的', trigger: 'blur' }],
        Name: [{ required: true, message: '物业名称是必填的', trigger: 'blur' }],
        Phone: [{ required: true, trigger: 'blur', validator: checkPhone }]
      },
      rootUrl: ''
    }
  },
  beforeUpdate() {
    this.rootUrl = process.env.API_HOST
    this.$nextTick(() => {
      this.$refs['dataForm'].clearValidate()
    })
    console.log(this.userfrom.LogoImageUrl)
  },
  methods: {
    submit() {
      this.$refs['dataForm'].validate((valid) => {
        if (valid) {
          updateWy(this.userfrom).then(response => {
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

<template>
  <el-form ref="dataForm" :rules="rules" :model="userfrom">
    <el-form-item label="用户名称" prop="Name">
      <el-input v-model="userfrom.Name" />
    </el-form-item>
    <el-form-item label="密码" prop="Password">
      <el-input v-model="userfrom.Password" />
    </el-form-item>
    <el-form-item label="手机号" prop="PhoneNumber">
      <el-input v-model="userfrom.PhoneNumber" />
    </el-form-item>
    <el-form-item>
      <el-button v-loading="createLoading" type="primary" @click="submit">提交</el-button>
    </el-form-item>
  </el-form>
</template>

<script>
import { updateWyUser } from '@/api/wyinfo'
import Dropzone from '@/components/Dropzone'

export default {
  components: { Dropzone },
  props: {
    userfrom: {
      type: Object,
      default: () => {
        return {
          Id: '',
          Name: '',
          Password: '',
          PhoneNumber: ''
        }
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
        Password: [{ required: true, message: '密码是必填的', trigger: 'blur' }],
        Name: [{ required: true, message: '用户名称是必填的', trigger: 'blur' }],
        PhoneNumber: [{ required: true, trigger: 'blur', validator: checkPhone }]
      }
    }
  },
  created() {
    this.rootUrl = process.env.API_HOST
    this.$nextTick(() => {
      this.$refs['dataForm'].clearValidate()
    })
  },
  methods: {
    submit() {
      console.log(this.userfrom)
      this.$refs['dataForm'].validate((valid) => {
        if (valid) {
          updateWyUser(this.userfrom).then(response => {
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
        this.userfrom.LogoImageId = succesCon.data.Id
        this.$message({ message: '上传成功', type: 'success' })
      } else {
        this.$message({ message: '上传失败', type: 'error' })
      }
      this.createLoading = false
    },
    dropzoneR(file) {
      this.temp.LogoImageId = ''
    }

  }
}
</script>

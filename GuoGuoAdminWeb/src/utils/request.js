import axios from 'axios'
import { Message, MessageBox } from 'element-ui'
import store from '@/store'
import { getToken } from '@/utils/auth'
var root = process.env.API_HOST
// create an axios instance
const service = axios.create({
  baseURL: root, // api 的 base_url
  // baseURL: '/apis', // api 的 base_url
  timeout: 5000// request timeout
})

// request interceptor
service.interceptors.request.use(
  config => {
    // config.headers['Content-Type'] = 'application/json'
    // config.headers['Content-Type'] = 'application/x-www-form-urlencoded'
    config.headers['Access-Control-Allow-Origin'] = '*'
    // Do something before request is sent
    if (store.getters.token) {
      // 让每个请求携带token-- ['X-Token']为自定义key 请根据实际情况自行修改
      config.headers['Authorization'] = getToken()
    }
    return config
  },
  error => {
    // Do something with request error
    // console.log(error) // for debug
    Promise.reject(error)
  }
)

// response interceptor
service.interceptors.response.use(
  // response => response,
  /**
   * 下面的注释为通过在response里，自定义code来标示请求状态
   * 当code返回如下情况则说明权限有问题，登出并返回到登录页
   * 如想通过 xmlhttprequest 来状态码标识 逻辑可写在下面error中
   * 以下代码均为样例，请结合自生需求加以修改，若不需要，则可删除
   */
  response => {
    const res = response.data
    // console.log(res)
    if (res.code !== '200') {
      Message({
        message: res.message,
        type: 'error',
        duration: 2 * 1000
      })
      // 500 token无效
      if (res.code === '500') {
        // 请自行在引入 MessageBox
        MessageBox.confirm('登录超时，可以取消继续留在该页面，或者重新登录', '确定登出', {
          confirmButtonText: '重新登录',
          // cancelButtonText: '取消',
          type: 'warning'
        }).then(() => {
          store.dispatch('FedLogOut').then(() => {
            location.reload() // 为了重新实例化vue-router对象 避免bug
          })
        })
      }
      return Promise.reject('error')
      // return response
    } else {
      return response
    }
  },
  error => {
    // console.log('err' + error) // for debug
    Message({
      message: error.message,
      type: 'error',
      duration: 2 * 1000
    })
    return Promise.reject(error)
  }
)

export default service

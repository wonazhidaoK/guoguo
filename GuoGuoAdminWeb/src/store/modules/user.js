// import { loginByUsername, logout, getUserInfo } from '@/api/login'
import { getUserInfo, resetUserInfo } from '@/api/login'
import { getToken, setToken, removeToken } from '@/utils/auth'

const user = {
  state: {
    user: '',
    status: '',
    code: '',
    token: getToken(),
    name: '',
    avatar: '',
    introduction: '',
    roles: [],
    province: '',
    city: '',
    region: '',
    setting: {
      articlePlatform: []
    }
  },

  mutations: {
    SET_CODE: (state, code) => {
      state.code = code
    },
    SET_TOKEN: (state, token) => {
      state.token = token
    },
    SET_INTRODUCTION: (state, introduction) => {
      state.introduction = introduction
    },
    SET_SETTING: (state, setting) => {
      state.setting = setting
    },
    SET_STATUS: (state, status) => {
      state.status = status
    },
    SET_NAME: (state, name) => {
      state.name = name
    },
    SET_AVATAR: (state, avatar) => {
      state.avatar = avatar
    },
    SET_ROLES: (state, roles) => {
      state.roles = roles
    },
    SET_PROVINCE: (state, province) => {
      state.province = province
    },
    SET_CITY: (state, city) => {
      state.city = city
    },
    SET_REGION: (state, region) => {
      state.region = region
    },
    SET_LOGINUSERINFO: (state, loginuser) => {
      state.loginuser = loginuser
    }
  },

  actions: {
    // 用户名登录
    // LoginByUsername({ commit }, userInfo) {
    //   const username = userInfo.username.trim()
    //   return new Promise((resolve, reject) => {
    //     loginByUsername(username, userInfo.password).then(response => {
    //       const data = response.data
    //       commit('SET_TOKEN', data.token)
    //       setToken(response.data.token)
    //       resolve(data)
    //     }).catch(error => {
    //       reject(error)
    //     })
    //   })
    // },

    // 获取用户信息
    GetUserInfo({ commit, state }, userInfo) {
      // console.log(state)
      // console.log(userInfo.username)
      const Name = userInfo.username.trim()
      return new Promise((resolve, reject) => {
        // getUserInfo(userInfo.Name, userInfo.Pwd).then(response => {
        getUserInfo(Name, userInfo.password).then(response => {
          // console.log(response.data)
          // 由于mockjs 不支持自定义状态码只能这样hack
          // if (!response.data) {
          //   reject('Verification failed, please login again.')
          // }
          var con = response.data
          // console.log(con)
          if (con.code === '200') {
            if (con.data.Roles && con.data.Roles.length > 0) { // 验证返回的roles是否是一个非空数组
              // console.log(con.data.Roles)
              commit('SET_ROLES', con.data.Roles)
              // router.addRoutes(store.getters.addRouters)
            } else {
              reject('您没有登录系统的权限')
            }
            commit('SET_NAME', con.data.Name)
            commit('SET_AVATAR', con.data.avatar)
            commit('SET_TOKEN', con.data.token)
            commit('SET_PROVINCE', con.data.State)
            commit('SET_CITY', con.data.City)
            commit('SET_REGION', con.data.Region)
            commit('SET_LOGINUSERINFO', con.data)

            setToken(con.data.token)
            // console.log(con.data.State)
            // commit('SET_INTRODUCTION', data.introduction)
            resolve(response)
          } else {
            reject(con.message)
          }
        }).catch(error => {
          reject(error)
        })
      })
    },

    ResetUserInfo({ commit }) {
      return new Promise((resolve, reject) => {
        resetUserInfo().then(response => {
          // console.log(response.data)
          var con = response.data
          if (con.code === '200') {
            if (con.data.Roles && con.data.Roles.length > 0) { // 验证返回的roles是否是一个非空数组
              // console.log(con.data.Roles)
              commit('SET_ROLES', con.data.Roles)
            } else {
              reject('您没有登录系统的权限')
            }
            commit('SET_NAME', con.data.Name)
            commit('SET_AVATAR', con.data.avatar)
            commit('SET_PROVINCE', con.data.State)
            commit('SET_CITY', con.data.City)
            commit('SET_REGION', con.data.Region)

            // console.log(con.data.SmallDistrictId)
            commit('SET_LOGINUSERINFO', con.data)

            resolve(response)
          } else {
            reject(con.message)
          }
        }).catch(error => {
          reject(error)
        })
      })
    },

    // 登出
    LogOut({ commit, state }) {
      return new Promise((resolve, reject) => {
        // logout(state.token).then(() => {
        commit('SET_TOKEN', '')
        commit('SET_ROLES', [])
        removeToken()
        resolve()
        // }).catch(error => {
        // reject(error)
        // })
      })
    },

    // 前端 登出
    FedLogOut({ commit }) {
      return new Promise(resolve => {
        commit('SET_TOKEN', '')
        removeToken()
        resolve()
      })
    },

    // 动态修改权限
    ChangeRoles({ commit, dispatch }, role) {
      return new Promise(resolve => {
        commit('SET_TOKEN', role)
        setToken(role)
        getUserInfo(role).then(response => {
          const data = response.data
          commit('SET_ROLES', data.roles)
          commit('SET_NAME', data.name)
          commit('SET_AVATAR', data.avatar)
          commit('SET_INTRODUCTION', data.introduction)
          dispatch('GenerateRoutes', data) // 动态修改权限后 重绘侧边菜单
          resolve()
        })
      })
    }
  }
}

export default user

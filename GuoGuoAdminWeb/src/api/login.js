import request from '@/utils/request'

export function logout() {
  return request({
    url: '/login/logout',
    method: 'post'
  })
}

export function getUserInfo(Name, Pwd) {
  const data = {
    Name,
    Pwd
  }
  return request({
    url: '/user/Login',
    method: 'post',
    data
  })
}

export function resetUserInfo() {
  return request({
    url: '/user/LoginToken',
    method: 'get'
  })
}

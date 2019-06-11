import request from '@/utils/request'

// 物业详情
export function getWyDetail(Id) {
  return request({
    url: '/propertyCompany/get',
    params: { Id }
  })
}

// 账号详情
export function getWyUserDetail(Id) {
  return request({
    url: '/user/Get',
    params: { Id }
  })
}
// 编辑物业
export function updateWy(data) {
  return request({
    url: '/propertyCompany/update',
    method: 'post',
    data
  })
}

// 编辑物业zhanghao
export function updateWyUser(data) {
  return request({
    url: '/user/updatePropertyUser',
    method: 'post',
    data
  })
}

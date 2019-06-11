
import request from '@/utils/request'

// 新建 条件POSTPOST POST /vipOwnerCertificationCondition/add
export function createTiaojian(data) {
  return request({
    url: '/vipOwnerCertificationCondition/add',
    method: 'post',
    data
  })
}

// 获取 条件列表
export function fetchList() {
  return request({
    url: '/vipOwnerCertificationCondition/getList',
    method: 'get'
  })
}

// 删除GET /announcement/delete
export function delVote(Id) {
  return request({
    url: '//delete',
    params: { Id }
  })
}

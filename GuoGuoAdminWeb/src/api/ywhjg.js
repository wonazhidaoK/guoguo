import request from '@/utils/request'

// 新建街道办
export function createZhineng(data) {
  return request({
    url: '/vipOwnerStructure/add',
    method: 'post',
    data
  })
}

// 获取街道办列表
export function fetchList(query) {
  return request({
    url: '/vipOwnerStructure/getAll',
    method: 'get',
    params: query
  })
}

// 删除街道办
export function delRequest(Id) {
  return request({
    url: '/vipOwnerStructure/delete',
    params: { Id }
  })
}

// 编辑街道办
export function update(data) {
  return request({
    url: '/vipOwnerStructure/update',
    method: 'post',
    data
  })
}

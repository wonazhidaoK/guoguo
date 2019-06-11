import request from '@/utils/request'

// 新建POST /propertyCompany/add
export function createShop(data) {
  return request({
    url: '/propertyCompany/add',
    method: 'post',
    data
  })
}

// 获取列表GET /propertyCompany/getAllForPage
export function fetchList(query) {
  return request({
    url: '/propertyCompany/getAllForPage',
    method: 'get',
    params: query
  })
}

// 删除GET /propertyCompany/delete
export function delJdbRequest(Id) {
  return request({
    url: '/propertyCompany/delete',
    params: { Id }
  })
}

// shop/get 详情GET /propertyCompany/get
export function getDetail(Id) {
  return request({
    url: '/propertyCompany/get',
    params: { Id }
  })
}

// 编辑POST /propertyCompany/update
export function updateShop(data) {
  return request({
    url: '/propertyCompany/update',
    method: 'post',
    data
  })
}

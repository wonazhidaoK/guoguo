import request from '@/utils/request'

// 获取类型
export function fetchTypeList() {
  return request({
    url: '/merchantCategory/getAll',
    method: 'get'
  })
}

// shop/get 详情
export function getShopDetail(Id) {
  return request({
    url: '/shop/get',
    params: { Id }
  })
}

// 编辑商铺
export function updateShop(data) {
  return request({
    url: '/shop/update',
    method: 'post',
    data
  })
}

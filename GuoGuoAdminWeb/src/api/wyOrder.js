import request from '@/utils/request'

// 获取订单列表
export function fetchList(query) {
  return request({
    url: '/order/getAllForProperty',
    method: 'get',
    params: query
  })
}

// 详情
export function getDetail(Id) {
  return request({
    url: '/order/get',
    method: 'get',
    params: { Id }
  })
}

// 获取商家列表
export function fetchShopUserList() {
  return request({
    url: '/smallDistrictShop/getList',
    method: 'get'
  })
}

// 取货
export function getGoodsRequest(Id) {
  return request({
    url: '/order/onSend',
    method: 'post',
    data: { Id }
  })
}

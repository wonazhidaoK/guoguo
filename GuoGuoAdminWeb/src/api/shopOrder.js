import request from '@/utils/request'

// 获取订单列表
export function fetchList(query) {
  return request({
    url: '/order/getAllForMerchant',
    method: 'get',
    params: query
  })
}

// 发货
export function sendShopRequest(Id) {
  return request({
    url: '/order/onReceive',
    method: 'post',
    data: { Id }
  })
}

// jdShopRequest 接单
export function jdShopRequest(Id) {
  return request({
    url: '/order/onAccept',
    method: 'post',
    data: { Id }
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

import request from '@/utils/request'

// 详情
export function getDetail(Id) {
  return request({
    url: '/order/get',
    params: { Id }
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

// 取货
export function getGoodsRequest(Id) {
  return request({
    url: '/order/onSend',
    method: 'post',
    data: { Id }
  })
}

// fhGoodsRequest 发货
export function fhGoodsRequest(Id) {
  return request({
    url: '/order/onReceive',
    method: 'post',
    data: { Id }
  })
}

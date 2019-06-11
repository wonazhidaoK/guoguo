import request from '@/utils/request'

// 新建商品类别
export function createShop(data) {
  return request({
    url: '/goodsType/add',
    method: 'post',
    data
  })
}

// 获取商铺列表
export function fetchList(query) {
  return request({
    url: '/goodsType/getAllForPage',
    method: 'get',
    params: query
  })
}

// 删除商铺
export function delRequest(Id) {
  return request({
    url: '/goodsType/delete',
    params: { Id }
  })
}

// 编辑商铺POST /goodsType/update
export function updateShop(data) {
  return request({
    url: '/goodsType/update',
    method: 'post',
    data
  })
}

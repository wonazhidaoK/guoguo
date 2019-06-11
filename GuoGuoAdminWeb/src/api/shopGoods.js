import request from '@/utils/request'

// 新建商品类别
export function createShop(data) {
  return request({
    url: '/shopCommodity/add',
    method: 'post',
    data
  })
}

// GET /platformCommodity/getForBarCode
export function serachGoodsMes(barCode) {
  return request({
    url: '/platformCommodity/getForBarCode',
    method: 'get',
    params: { barCode }
  })
}

// 获取商铺列表GET /shopCommodity/getAllForPage
export function fetchList(query) {
  return request({
    url: '/shopCommodity/getAllForPage',
    method: 'get',
    params: query
  })
}

// 商品类别
export function getTypeList(shopId) {
  return request({
    url: '/goodsType/getList',
    method: 'get',
    params: { shopId }
  })
}

// 商品i详情GET /shopCommodity/get
export function getDetail(id) {
  return request({
    url: '/shopCommodity/get',
    method: 'get',
    params: { id }
  })
}

// 删除商铺GET /shopCommodity/delete
export function delRequest(Id) {
  return request({
    url: '/shopCommodity/delete',
    params: { Id }
  })
}

export function goodsXj(id) {
  return request({
    url: '/shopCommodity/obtained',
    method: 'post',
    data: { id }
  })
}

export function goodsSj(id) {
  return request({
    url: '/shopCommodity/shelf',
    method: 'post',
    data: { id }
  })
}

// 编辑商铺POST POST /shopCommodity/update
export function updateShop(data) {
  return request({
    url: '/shopCommodity/update',
    method: 'post',
    data
  })
}

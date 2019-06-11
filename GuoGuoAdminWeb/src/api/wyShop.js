import request from '@/utils/request'

// 获取平台商铺列表
export function getPlatformShopList(data) {
  return request({
    url: '/shop/getList',
    method: 'get',
    data
  })
}
// 获取小区商铺列表
export function fetchList(query) {
  return request({
    url: 'smallDistrictShop/getAllForPage',
    method: 'get',
    params: query
  })
}
// 查询未选择商户列表
export function getSmallDistrictShopNotSelected(data) {
  return request({
    url: '/smallDistrictShop/getListForNotSelected',
    method: 'get',
    data
  })
}

// POST /user/addShopUser
// 新建小区商铺
export function createSmallDistrictShop(data) {
  return request({
    url: '/smallDistrictShop/add',
    method: 'post',
    data
  })
}

// 新建小区商铺
export function updateSmallDistrictShop(data) {
  return request({
    url: '/smallDistrictShop/update',
    method: 'post',
    data
  })
}

// 删除小区商铺
export function delSmallDistrictShop(Id) {
  return request({
    url: '/smallDistrictShop/delete',
    params: { Id },
    method: 'get'
  })
}

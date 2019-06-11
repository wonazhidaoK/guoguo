import request from '@/utils/request'

// 新建商铺
export function createShop(data) {
  return request({
    url: '/shop/add',
    method: 'post',
    data
  })
}

// POST /user/addShopUser
// 新建商铺账号
export function createShopUser(data) {
  return request({
    url: '/user/addShopUser',
    method: 'post',
    data
  })
}

// 商铺角色权限
export function fetRoleList() {
  return request({
    url: '/role/getAllForShop',
    method: 'get'
  })
}

// 获取商铺列表
export function fetchList(query) {
  return request({
    url: '/shop/getAll',
    method: 'get',
    params: query
  })
}

// fetchShopUserList 获取商铺账号列表
export function fetchShopUserList(query) {
  return request({
    url: '/user/getAllShopUser',
    method: 'get',
    params: query
  })
}

// 获取类型
export function fetchTypeList() {
  return request({
    url: '/merchantCategory/getAll',
    method: 'get'
  })
}

// fetchTShopSelectList 获取商家列表 用来新建商铺账号
export function fetchTShopSelectList() {
  return request({
    url: '/shop/getList',
    method: 'get'
  })
}

// 删除商铺
export function delJdbRequest(Id) {
  return request({
    url: '/shop/delete',
    params: { Id }
  })
}

// 删除商铺账号
export function delShopUserRequest(Id) {
  return request({
    url: '/user/delete',
    params: { Id }
  })
}

// shop/get 详情
export function getDetail(Id) {
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

// 编辑商铺账号POST /user/updateShopUser
export function updateShopUser(data) {
  return request({
    url: '/user/updateShopUser',
    method: 'post',
    data
  })
}

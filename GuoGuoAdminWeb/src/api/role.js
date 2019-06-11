import request from '@/utils/request'

// 获取部门列表GET /department/getAll
export function fetchListBm() {
  return request({
    url: '/department/getAll',
    method: 'get'
  })
}

// 新建角色
export function createRole(data) {
  return request({
    url: '/role/add',
    method: 'post',
    data
  })
}

// 获取角色列表
export function fetchList(query) {
  return request({
    url: '/role/getAll',
    method: 'get',
    params: query
  })
}

// 删除角色
export function delRequest(Id) {
  return request({
    url: '/role/delete',
    params: { Id }
  })
}

// 编辑角色
export function update(data) {
  return request({
    url: '/role/update',
    method: 'post',
    data
  })
}

// 获取该用户的菜单权限
export function getRoleMenu(roleId) {
  return request({
    url: '/roleMenu/getRoleMenus',
    method: 'get',
    params: { roleId }
  })
}

// 获取所有菜单
export function getMenu(DepartmentValue) {
  return request({
    url: '/menu/getAll',
    method: 'get',
    params: { DepartmentValue }
  })
}

// POST /roleMenu/add
export function addMenuRole(RoleId, MenuId) {
  const data = {
    RoleId,
    MenuId
  }
  return request({
    url: '/roleMenu/add',
    method: 'post',
    data
  })
}

// POST /roleMenu/addGET /roleMenu/delete
export function delMenuRole(RoleId, MenuId) {
  const data = {
    RoleId,
    MenuId
  }
  return request({
    url: '/roleMenu/delete',
    method: 'post',
    data
  })
}

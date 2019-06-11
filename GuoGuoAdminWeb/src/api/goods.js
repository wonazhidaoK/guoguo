import request from '@/utils/request'

//  获取平台商品 分页
// {
//   "Name": "string",
//     "BarCode": "string",
//       "PageIndex": 0,
//         "PageSize": 0
// }
export function getAllForPage(data) {
  return request({
    url: '/platformCommodity/getAllForPage',
    method: 'post',
    data
  })
}

// 删除平台商品
export function del(Id) {
  return request({
    url: '/platformCommodity/delete',
    method: 'post',
    data: { Id }
  })
}

// 添加平台商品
// {
// "Id": "string",
// "BarCode": "string",
// "Name": "string",
// "ImageUrl": "string",
// "Price": 0
// }
export function add(data) {
  return request({
    url: '/platformCommodity/add',
    method: 'post',
    data
  })
}

// 修改平台商品
// {
// "Id": "string",
// "BarCode": "string",
// "Name": "string",
// "ImageUrl": "string",
// "Price": 0
// }
export function update(data) {
  return request({
    url: '/platformCommodity/update',
    method: 'post',
    data
  })
}

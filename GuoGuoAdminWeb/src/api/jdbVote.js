
import request from '@/utils/request'

// 小区
export function getSmallList(StreetOfficeId) {
  return request({
    url: '/smallDistrict/getAllForStreetOfficeId',
    params: { StreetOfficeId }
  })
}

//  手动干预/vote/updateCalculationMethod
export function handFn(Id) {
  return request({
    url: '/vote/updateCalculationMethod',
    params: { Id }
  })
}

// 建议列表
export function fetchJyList(query) {
  return request({
    url: '/voteRecord/getFeedback',
    params: query
  })
}

// 新建投票POST /vote/addVoteForStreetOffice
export function createvVote(p) {
  const data = {
    'Title': p.Title,
    'Summary': p.Summary,
    'Deadline': p.Deadline,
    'SmallDistrict': p.SmallDistrict,
    'VoteTypeValue': p.VoteTypeValue,
    'List': [
      {
        'Title': p.Question,
        'List': [
          {
            'Describe': '同意'
          }, {
            'Describe': '不同意'
          }
        ]
      }
    ],
    'AnnexId': p.AnnexId
  }
  return request({
    url: '/vote/addVoteForStreetOffice',
    method: 'post',
    data
  })
}

// 获取公告列表GETGETGET /vote/getAllForStreetOffice
export function fetchListVote(query) {
  return request({
    url: '/vote/getAllForStreetOffice',
    method: 'get',
    params: query
  })
}

// 编辑toupiao
export function updateVote(data) {
  return request({
    url: '//update',
    method: 'post',
    data
  })
}

// 删除投票GET /announcement/delete
export function delVote(Id) {
  return request({
    url: '//delete',
    params: { Id }
  })
}

// GET /vote/get
export function detailVote(Id) {
  return request({
    url: '/vote/get',
    params: { Id }
  })
}

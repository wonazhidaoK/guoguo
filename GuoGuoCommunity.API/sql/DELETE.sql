USE [GuoGuoCommunityTest]
GO

--公告附件表
DELETE FROM AnnouncementAnnexes

--公告表
DELETE FROM Announcements

--楼宇表
DELETE FROM Buildings

--楼宇单元表
DELETE FROM BuildingUnits

--社区表
DELETE FROM Communities

--投诉附件表

DELETE FROM ComplaintAnnexes

--投诉附件表
DELETE FROM ComplaintFollowUps

--投诉表
DELETE FROM Complaints

--投诉状态变更表
DELETE FROM ComplaintStatusChangeRecordings

--投诉类型表
DELETE FROM ComplaintTypes

--身份证识别记录
DELETE FROM IDCardPhotoRecords

--业户表
DELETE FROM Industries

--业主申请记录附件表
DELETE FROM OwnerCertificationAnnexes

--业主申请记录表
DELETE FROM OwnerCertificationRecords

--业主表
DELETE FROM Owners

--小区表
DELETE FROM SmallDistricts

--站内信附件表
DELETE FROM StationLetterAnnexes

--站内信浏览记录表
DELETE FROM StationLetterBrowseRecords

--站内信表
DELETE FROM StationLetters

--街道办表
DELETE FROM StreetOffices

--上传表
DELETE FROM Uploads

--用户表
DELETE FROM Users 
WHERE NAME !='admin' OR NAME IS NULL

--高级认证申请记录表
DELETE FROM VipOwnerApplicationRecords

--高级认证附件表
DELETE FROM VipOwnerCertificationAnnexes

--高级认证申请条件表
DELETE FROM VipOwnerCertificationConditions

--高级认证记录表
DELETE FROM VipOwnerCertificationRecords

--业委会表
DELETE FROM VipOwners

--业委会架构表
DELETE FROM VipOwnerStructures

--投票附件表
DELETE FROM VoteAnnexes

--投票关联业委会表
DELETE FROM VoteAssociationVipOwners

--投票问题选项表
DELETE FROM VoteQuestionOptions

--投票问题表
DELETE FROM VoteQuestions

--投票记录详情表
DELETE FROM VoteRecordDetails

--投票记录表
DELETE FROM VoteRecords

--投票结果记录表
DELETE FROM VoteResultRecords

--投票表
DELETE FROM Votes

--微信用户表
--DELETE FROM WeiXinUsers

--角色菜单表
--DELETE FROM Role_Menu

--角色表
--DELETE FROM User_Role
GO



USE [GuoGuoCommunityTest]
GO

--���渽����
DELETE FROM AnnouncementAnnexes

--�����
DELETE FROM Announcements

--¥���
DELETE FROM Buildings

--¥�Ԫ��
DELETE FROM BuildingUnits

--������
DELETE FROM Communities

--Ͷ�߸�����

DELETE FROM ComplaintAnnexes

--Ͷ�߸�����
DELETE FROM ComplaintFollowUps

--Ͷ�߱�
DELETE FROM Complaints

--Ͷ��״̬�����
DELETE FROM ComplaintStatusChangeRecordings

--Ͷ�����ͱ�
DELETE FROM ComplaintTypes

--���֤ʶ���¼
DELETE FROM IDCardPhotoRecords

--ҵ����
DELETE FROM Industries

--ҵ�������¼������
DELETE FROM OwnerCertificationAnnexes

--ҵ�������¼��
DELETE FROM OwnerCertificationRecords

--ҵ����
DELETE FROM Owners

--С����
DELETE FROM SmallDistricts

--վ���Ÿ�����
DELETE FROM StationLetterAnnexes

--վ���������¼��
DELETE FROM StationLetterBrowseRecords

--վ���ű�
DELETE FROM StationLetters

--�ֵ����
DELETE FROM StreetOffices

--�ϴ���
DELETE FROM Uploads

--�û���
DELETE FROM Users 
WHERE NAME !='admin' OR NAME IS NULL

--�߼���֤�����¼��
DELETE FROM VipOwnerApplicationRecords

--�߼���֤������
DELETE FROM VipOwnerCertificationAnnexes

--�߼���֤����������
DELETE FROM VipOwnerCertificationConditions

--�߼���֤��¼��
DELETE FROM VipOwnerCertificationRecords

--ҵί���
DELETE FROM VipOwners

--ҵί��ܹ���
DELETE FROM VipOwnerStructures

--ͶƱ������
DELETE FROM VoteAnnexes

--ͶƱ����ҵί���
DELETE FROM VoteAssociationVipOwners

--ͶƱ����ѡ���
DELETE FROM VoteQuestionOptions

--ͶƱ�����
DELETE FROM VoteQuestions

--ͶƱ��¼�����
DELETE FROM VoteRecordDetails

--ͶƱ��¼��
DELETE FROM VoteRecords

--ͶƱ�����¼��
DELETE FROM VoteResultRecords

--ͶƱ��
DELETE FROM Votes

--΢���û���
--DELETE FROM WeiXinUsers

--��ɫ�˵���
--DELETE FROM Role_Menu

--��ɫ��
--DELETE FROM User_Role
GO



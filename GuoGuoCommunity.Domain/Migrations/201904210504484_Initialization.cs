namespace GuoGuoCommunity.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initialization : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AnnouncementAnnexes",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        AnnouncementId = c.String(),
                        AnnexContent = c.String(),
                        AnnexId = c.String(),
                        CreateOperationUserId = c.String(),
                        CreateOperationTime = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Announcements",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Title = c.String(),
                        Summary = c.String(),
                        Content = c.String(),
                        SmallDistrictArray = c.String(),
                        DepartmentName = c.String(),
                        DepartmentValue = c.String(),
                        StreetOfficeId = c.String(),
                        StreetOfficeName = c.String(),
                        CommunityId = c.String(),
                        CommunityName = c.String(),
                        SmallDistrictId = c.String(),
                        SmallDistrictName = c.String(),
                        OwnerCertificationId = c.String(),
                        CreateOperationUserId = c.String(),
                        CreateOperationTime = c.DateTimeOffset(precision: 7),
                        LastOperationUserId = c.String(),
                        LastOperationTime = c.DateTimeOffset(precision: 7),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedTime = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Buildings",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(),
                        SmallDistrictId = c.String(nullable: false),
                        SmallDistrictName = c.String(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedTime = c.DateTimeOffset(precision: 7),
                        LastOperationUserId = c.String(),
                        LastOperationTime = c.DateTimeOffset(precision: 7),
                        CreateOperationUserId = c.String(),
                        CreateOperationTime = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BuildingUnits",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        UnitName = c.String(nullable: false),
                        NumberOfLayers = c.Int(nullable: false),
                        BuildingId = c.String(nullable: false),
                        LastOperationUserId = c.String(),
                        LastOperationTime = c.DateTimeOffset(precision: 7),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedTime = c.DateTimeOffset(precision: 7),
                        CreateOperationUserId = c.String(),
                        CreateOperationTime = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Communities",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        State = c.String(nullable: false),
                        City = c.String(nullable: false),
                        Region = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        StreetOfficeId = c.String(nullable: false),
                        StreetOfficeName = c.String(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedTime = c.DateTimeOffset(precision: 7),
                        LastOperationUserId = c.String(),
                        LastOperationTime = c.DateTimeOffset(precision: 7),
                        CreateOperationUserId = c.String(),
                        CreateOperationTime = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ComplaintAnnexes",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        ComplaintId = c.String(),
                        ComplaintFollowUpId = c.String(),
                        AnnexContent = c.String(),
                        AnnexId = c.String(),
                        CreateOperationUserId = c.String(),
                        CreateOperationTime = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ComplaintFollowUps",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        ComplaintId = c.String(),
                        Description = c.String(),
                        OperationDepartmentName = c.String(),
                        OperationDepartmentValue = c.String(),
                        OwnerCertificationId = c.String(),
                        Aappeal = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedTime = c.DateTimeOffset(precision: 7),
                        LastOperationUserId = c.String(),
                        LastOperationTime = c.DateTimeOffset(precision: 7),
                        CreateOperationUserId = c.String(),
                        CreateOperationTime = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Complaints",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Description = c.String(),
                        ComplaintTypeId = c.String(),
                        ComplaintTypeName = c.String(),
                        DepartmentName = c.String(),
                        DepartmentValue = c.String(),
                        OwnerCertificationId = c.String(),
                        ClosedTime = c.DateTimeOffset(precision: 7),
                        ExpiredTime = c.DateTimeOffset(precision: 7),
                        ProcessUpTime = c.DateTimeOffset(precision: 7),
                        IsInvalid = c.String(),
                        StatusName = c.String(),
                        StatusValue = c.String(),
                        StreetOfficeId = c.String(),
                        StreetOfficeName = c.String(),
                        CommunityId = c.String(),
                        CommunityName = c.String(),
                        SmallDistrictId = c.String(),
                        SmallDistrictName = c.String(),
                        OperationDepartmentName = c.String(),
                        OperationDepartmentValue = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedTime = c.DateTimeOffset(precision: 7),
                        LastOperationUserId = c.String(),
                        LastOperationTime = c.DateTimeOffset(precision: 7),
                        CreateOperationUserId = c.String(),
                        CreateOperationTime = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ComplaintTypes",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        Level = c.String(nullable: false),
                        InitiatingDepartmentName = c.String(nullable: false),
                        InitiatingDepartmentValue = c.String(nullable: false),
                        ProcessingPeriod = c.Int(nullable: false),
                        ComplaintPeriod = c.Int(nullable: false),
                        LastOperationUserId = c.String(),
                        LastOperationTime = c.DateTimeOffset(precision: 7),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedTime = c.DateTimeOffset(precision: 7),
                        CreateOperationUserId = c.String(),
                        CreateOperationTime = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.IDCardPhotoRecords",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        ApplicationRecordId = c.String(),
                        OwnerCertificationAnnexId = c.String(),
                        PhotoBase64 = c.String(),
                        Message = c.String(),
                        CreateOperationUserId = c.String(),
                        CreateOperationTime = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Industries",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(),
                        NumberOfLayers = c.Int(nullable: false),
                        Acreage = c.String(),
                        Oriented = c.String(),
                        BuildingId = c.String(nullable: false),
                        BuildingName = c.String(),
                        BuildingUnitId = c.String(),
                        BuildingUnitName = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedTime = c.DateTimeOffset(precision: 7),
                        LastOperationUserId = c.String(),
                        LastOperationTime = c.DateTimeOffset(precision: 7),
                        CreateOperationUserId = c.String(),
                        CreateOperationTime = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Menus",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(),
                        Key = c.String(),
                        DepartmentName = c.String(),
                        DepartmentValue = c.String(),
                        CreateOperationUserId = c.String(),
                        CreateOperationTime = c.DateTimeOffset(precision: 7),
                        LastOperationUserId = c.String(),
                        LastOperationTime = c.DateTimeOffset(precision: 7),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedTime = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OwnerCertificationAnnexes",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        OwnerCertificationAnnexTypeValue = c.String(),
                        ApplicationRecordId = c.String(),
                        AnnexContent = c.String(),
                        AnnexId = c.String(),
                        CreateOperationUserId = c.String(),
                        CreateOperationTime = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OwnerCertificationRecords",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        UserId = c.String(),
                        StreetOfficeId = c.String(),
                        StreetOfficeName = c.String(),
                        CommunityId = c.String(),
                        CommunityName = c.String(),
                        SmallDistrictId = c.String(),
                        SmallDistrictName = c.String(),
                        BuildingId = c.String(),
                        BuildingName = c.String(),
                        BuildingUnitId = c.String(),
                        BuildingUnitName = c.String(),
                        IndustryId = c.String(),
                        IndustryName = c.String(),
                        OwnerId = c.String(),
                        OwnerName = c.String(),
                        CertificationTime = c.DateTimeOffset(precision: 7),
                        CertificationResult = c.String(),
                        CertificationStatusName = c.String(),
                        CertificationStatusValue = c.String(),
                        IsInvalid = c.Boolean(nullable: false),
                        LastOperationUserId = c.String(),
                        LastOperationTime = c.DateTimeOffset(precision: 7),
                        CreateOperationUserId = c.String(),
                        CreateOperationTime = c.DateTimeOffset(precision: 7),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedTime = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Owners",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(),
                        Birthday = c.String(),
                        Gender = c.String(),
                        PhoneNumber = c.String(),
                        IDCard = c.String(),
                        IndustryId = c.String(),
                        IndustryName = c.String(),
                        OwnerCertificationRecordId = c.String(),
                        IsLegalize = c.Boolean(nullable: false),
                        LastOperationUserId = c.String(),
                        LastOperationTime = c.DateTimeOffset(precision: 7),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedTime = c.DateTimeOffset(precision: 7),
                        CreateOperationUserId = c.String(),
                        CreateOperationTime = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Role_Menu",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        RolesId = c.String(),
                        MenuId = c.String(),
                        CreateOperationUserId = c.String(),
                        CreateOperationTime = c.DateTimeOffset(precision: 7),
                        LastOperationUserId = c.String(),
                        LastOperationTime = c.DateTimeOffset(precision: 7),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedTime = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SmallDistricts",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        State = c.String(nullable: false),
                        City = c.String(nullable: false),
                        Region = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        StreetOfficeId = c.String(nullable: false),
                        StreetOfficeName = c.String(nullable: false),
                        CommunityId = c.String(nullable: false),
                        CommunityName = c.String(nullable: false),
                        IsInvalid = c.String(),
                        IsElection = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedTime = c.DateTimeOffset(precision: 7),
                        LastOperationUserId = c.String(),
                        LastOperationTime = c.DateTimeOffset(precision: 7),
                        CreateOperationUserId = c.String(),
                        CreateOperationTime = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.StationLetterAnnexes",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        StationLetterId = c.String(),
                        AnnexContent = c.String(),
                        AnnexId = c.String(),
                        CreateOperationUserId = c.String(),
                        CreateOperationTime = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.StationLetterBrowseRecords",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        StationLetterId = c.String(),
                        CreateOperationUserId = c.String(),
                        CreateOperationTime = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.StationLetters",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Title = c.String(),
                        Summary = c.String(),
                        Content = c.String(),
                        SmallDistrictArray = c.String(),
                        StreetOfficeId = c.String(),
                        StreetOfficeName = c.String(),
                        CreateOperationUserId = c.String(),
                        CreateOperationTime = c.DateTimeOffset(precision: 7),
                        LastOperationUserId = c.String(),
                        LastOperationTime = c.DateTimeOffset(precision: 7),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedTime = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ComplaintStatusChangeRecordings",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        OldStatus = c.String(),
                        NewStatus = c.String(),
                        ComplaintFollowUpId = c.String(),
                        ComplaintId = c.String(),
                        CreateOperationUserId = c.String(),
                        CreateOperationTime = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.StreetOffices",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        State = c.String(nullable: false),
                        City = c.String(nullable: false),
                        Region = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedTime = c.DateTimeOffset(precision: 7),
                        LastOperationUserId = c.String(),
                        LastOperationTime = c.DateTimeOffset(precision: 7),
                        CreateOperationUserId = c.String(),
                        CreateOperationTime = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Uploads",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Agreement = c.String(),
                        Host = c.String(),
                        Domain = c.String(),
                        Directory = c.String(),
                        File = c.String(),
                        LastOperationUserId = c.String(),
                        LastOperationTime = c.DateTimeOffset(precision: 7),
                        CreateOperationUserId = c.String(),
                        CreateOperationTime = c.DateTimeOffset(precision: 7),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedTime = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.User_Role",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        DepartmentName = c.String(),
                        DepartmentValue = c.String(),
                        CreateOperationUserId = c.String(),
                        CreateOperationTime = c.DateTimeOffset(precision: 7),
                        LastOperationUserId = c.String(),
                        LastOperationTime = c.DateTimeOffset(precision: 7),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedTime = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        State = c.String(),
                        City = c.String(),
                        Region = c.String(),
                        StreetOfficeId = c.String(),
                        StreetOfficeName = c.String(),
                        CommunityId = c.String(),
                        CommunityName = c.String(),
                        SmallDistrictId = c.String(),
                        SmallDistrictName = c.String(),
                        DepartmentName = c.String(),
                        DepartmentValue = c.String(),
                        RoleId = c.String(),
                        RoleName = c.String(),
                        Account = c.String(),
                        Password = c.String(),
                        Name = c.String(),
                        PhoneNumber = c.String(),
                        OpenId = c.String(),
                        UnionId = c.String(),
                        RefreshToken = c.String(),
                        CreateOperationUserId = c.String(),
                        CreateOperationTime = c.DateTimeOffset(precision: 7),
                        LastOperationUserId = c.String(),
                        LastOperationTime = c.DateTimeOffset(precision: 7),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedTime = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VipOwnerApplicationRecords",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        UserId = c.String(),
                        StructureId = c.String(),
                        StructureName = c.String(),
                        Reason = c.String(),
                        IsInvalid = c.Boolean(nullable: false),
                        IsAdopt = c.Boolean(nullable: false),
                        SmallDistrictId = c.String(),
                        SmallDistrictName = c.String(),
                        Name = c.String(),
                        OwnerCertificationId = c.String(),
                        VoteId = c.String(),
                        VoteQuestionId = c.String(),
                        VoteQuestionOptionId = c.String(),
                        LastOperationUserId = c.String(),
                        LastOperationTime = c.DateTimeOffset(precision: 7),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedTime = c.DateTimeOffset(precision: 7),
                        CreateOperationUserId = c.String(),
                        CreateOperationTime = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VipOwnerCertificationAnnexes",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        CertificationConditionId = c.String(),
                        ApplicationRecordId = c.String(),
                        AnnexContent = c.String(),
                        AnnexId = c.String(),
                        CreateOperationUserId = c.String(),
                        CreateOperationTime = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VipOwnerCertificationConditions",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        TypeName = c.String(),
                        TypeValue = c.String(),
                        CreateOperationUserId = c.String(),
                        CreateOperationTime = c.DateTimeOffset(precision: 7),
                        LastOperationUserId = c.String(),
                        LastOperationTime = c.DateTimeOffset(precision: 7),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedTime = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VipOwnerCertificationRecords",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        VipOwnerId = c.String(),
                        VipOwnerName = c.String(),
                        VipOwnerStructureId = c.String(),
                        VipOwnerStructureName = c.String(),
                        UserId = c.String(),
                        IsInvalid = c.Boolean(nullable: false),
                        OwnerCertificationId = c.String(),
                        VoteId = c.String(),
                        LastOperationUserId = c.String(),
                        LastOperationTime = c.DateTimeOffset(precision: 7),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedTime = c.DateTimeOffset(precision: 7),
                        CreateOperationUserId = c.String(),
                        CreateOperationTime = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VipOwners",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(),
                        RemarkName = c.String(),
                        SmallDistrictId = c.String(nullable: false),
                        SmallDistrictName = c.String(nullable: false),
                        IsValid = c.Boolean(nullable: false),
                        IsElection = c.Boolean(nullable: false),
                        LastOperationUserId = c.String(),
                        LastOperationTime = c.DateTimeOffset(precision: 7),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedTime = c.DateTimeOffset(precision: 7),
                        CreateOperationUserId = c.String(),
                        CreateOperationTime = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VipOwnerStructures",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Weights = c.String(),
                        IsReview = c.Boolean(nullable: false),
                        Description = c.String(),
                        LastOperationUserId = c.String(),
                        LastOperationTime = c.DateTimeOffset(precision: 7),
                        CreateOperationUserId = c.String(),
                        CreateOperationTime = c.DateTimeOffset(precision: 7),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedTime = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VoteAnnexes",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        VoteId = c.String(),
                        AnnexContent = c.String(),
                        AnnexId = c.String(),
                        CreateOperationUserId = c.String(),
                        CreateOperationTime = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VoteAssociationVipOwners",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        VoteId = c.String(),
                        VipOwnerId = c.String(),
                        ElectionNumber = c.Int(nullable: false),
                        CreateOperationUserId = c.String(),
                        CreateOperationTime = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VoteQuestionOptions",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        VoteId = c.String(),
                        VoteQuestionId = c.String(),
                        Describe = c.String(),
                        Votes = c.Int(nullable: false),
                        LastOperationUserId = c.String(),
                        LastOperationTime = c.DateTimeOffset(precision: 7),
                        CreateOperationUserId = c.String(),
                        CreateOperationTime = c.DateTimeOffset(precision: 7),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedTime = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VoteQuestions",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        VoteId = c.String(),
                        Title = c.String(),
                        OptionMode = c.String(),
                        LastOperationUserId = c.String(),
                        LastOperationTime = c.DateTimeOffset(precision: 7),
                        CreateOperationUserId = c.String(),
                        CreateOperationTime = c.DateTimeOffset(precision: 7),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedTime = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VoteRecordDetails",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        VoteId = c.String(),
                        VoteQuestionId = c.String(),
                        VoteQuestionOptionId = c.String(),
                        LastOperationUserId = c.String(),
                        LastOperationTime = c.DateTimeOffset(precision: 7),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedTime = c.DateTimeOffset(precision: 7),
                        CreateOperationUserId = c.String(),
                        CreateOperationTime = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VoteRecords",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        VoteId = c.String(),
                        Feedback = c.String(),
                        OwnerCertificationId = c.String(),
                        CreateOperationUserId = c.String(),
                        CreateOperationTime = c.DateTimeOffset(precision: 7),
                        LastOperationUserId = c.String(),
                        LastOperationTime = c.DateTimeOffset(precision: 7),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedTime = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VoteResultRecords",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        VoteId = c.String(),
                        CalculationMethodValue = c.String(),
                        CalculationMethodName = c.String(),
                        ResultValue = c.String(),
                        ResultName = c.String(),
                        VoteQuestionId = c.String(),
                        ShouldParticipateCount = c.Int(),
                        ActualParticipateCount = c.Int(nullable: false),
                        LastOperationUserId = c.String(),
                        LastOperationTime = c.DateTimeOffset(precision: 7),
                        CreateOperationUserId = c.String(),
                        CreateOperationTime = c.DateTimeOffset(precision: 7),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedTime = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Votes",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Title = c.String(),
                        Summary = c.String(),
                        Deadline = c.DateTimeOffset(nullable: false, precision: 7),
                        SmallDistrictArray = c.String(),
                        DepartmentName = c.String(),
                        DepartmentValue = c.String(),
                        CalculationMethodValue = c.String(),
                        CalculationMethodName = c.String(),
                        StatusValue = c.String(),
                        StatusName = c.String(),
                        VoteTypeValue = c.String(),
                        VoteTypeName = c.String(),
                        StreetOfficeId = c.String(),
                        StreetOfficeName = c.String(),
                        CommunityId = c.String(),
                        CommunityName = c.String(),
                        SmallDistrictId = c.String(),
                        SmallDistrictName = c.String(),
                        OwnerCertificationId = c.String(),
                        LastOperationUserId = c.String(),
                        LastOperationTime = c.DateTimeOffset(precision: 7),
                        CreateOperationUserId = c.String(),
                        CreateOperationTime = c.DateTimeOffset(precision: 7),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedTime = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.WeiXinUsers",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Subscribe_scene = c.String(),
                        Tagid_list = c.String(),
                        Groupid = c.String(),
                        Remark = c.String(),
                        Unionid = c.String(),
                        Subscribe_time = c.String(),
                        Headimgurl = c.String(),
                        Country = c.String(),
                        Province = c.String(),
                        City = c.String(),
                        Language = c.String(),
                        Sex = c.Int(nullable: false),
                        Nickname = c.String(),
                        OpenId = c.String(),
                        Subscribe = c.Int(nullable: false),
                        Qr_scene = c.Int(nullable: false),
                        Qr_scene_str = c.String(),
                        CreateOperationUserId = c.String(),
                        CreateOperationTime = c.DateTimeOffset(precision: 7),
                        LastOperationUserId = c.String(),
                        LastOperationTime = c.DateTimeOffset(precision: 7),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedTime = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.WeiXinUsers");
            DropTable("dbo.Votes");
            DropTable("dbo.VoteResultRecords");
            DropTable("dbo.VoteRecords");
            DropTable("dbo.VoteRecordDetails");
            DropTable("dbo.VoteQuestions");
            DropTable("dbo.VoteQuestionOptions");
            DropTable("dbo.VoteAssociationVipOwners");
            DropTable("dbo.VoteAnnexes");
            DropTable("dbo.VipOwnerStructures");
            DropTable("dbo.VipOwners");
            DropTable("dbo.VipOwnerCertificationRecords");
            DropTable("dbo.VipOwnerCertificationConditions");
            DropTable("dbo.VipOwnerCertificationAnnexes");
            DropTable("dbo.VipOwnerApplicationRecords");
            DropTable("dbo.Users");
            DropTable("dbo.User_Role");
            DropTable("dbo.Uploads");
            DropTable("dbo.Tests");
            DropTable("dbo.StreetOffices");
            DropTable("dbo.ComplaintStatusChangeRecordings");
            DropTable("dbo.StationLetters");
            DropTable("dbo.StationLetterBrowseRecords");
            DropTable("dbo.StationLetterAnnexes");
            DropTable("dbo.SmallDistricts");
            DropTable("dbo.Role_Menu");
            DropTable("dbo.Owners");
            DropTable("dbo.OwnerCertificationRecords");
            DropTable("dbo.OwnerCertificationAnnexes");
            DropTable("dbo.Menus");
            DropTable("dbo.Industries");
            DropTable("dbo.IDCardPhotoRecords");
            DropTable("dbo.ComplaintTypes");
            DropTable("dbo.Complaints");
            DropTable("dbo.ComplaintFollowUps");
            DropTable("dbo.ComplaintAnnexes");
            DropTable("dbo.Communities");
            DropTable("dbo.BuildingUnits");
            DropTable("dbo.Buildings");
            DropTable("dbo.Announcements");
            DropTable("dbo.AnnouncementAnnexes");
        }
    }
}

namespace GuoGuoCommunity.Domain.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class initialization : DbMigration
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
                        NumberOfLayers = c.String(),
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
                "dbo.ComplaintTypes",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(),
                        Level = c.String(nullable: false),
                        InitiatingDepartmentName = c.String(nullable: false),
                        InitiatingDepartmentValue = c.String(nullable: false),
                        ProcessingPeriod = c.String(nullable: false),
                        ComplaintPeriod = c.String(nullable: false),
                        LastOperationUserId = c.String(),
                        LastOperationTime = c.DateTimeOffset(precision: 7),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedTime = c.DateTimeOffset(precision: 7),
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
                        NumberOfLayers = c.String(),
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
                        CertificationTime = c.String(),
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
                        Openid = c.String(),
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
            DropTable("dbo.ComplaintTypes");
            DropTable("dbo.Communities");
            DropTable("dbo.BuildingUnits");
            DropTable("dbo.Buildings");
            DropTable("dbo.Announcements");
            DropTable("dbo.AnnouncementAnnexes");
        }
    }
}

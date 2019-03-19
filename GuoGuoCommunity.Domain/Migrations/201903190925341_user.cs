namespace GuoGuoCommunity.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class user : DbMigration
    {
        public override void Up()
        {
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
            
            AddColumn("dbo.OwnerCertificationRecords", "StreetOfficeId", c => c.String());
            AddColumn("dbo.OwnerCertificationRecords", "StreetOfficeName", c => c.String());
            AddColumn("dbo.OwnerCertificationRecords", "CommunityId", c => c.String());
            AddColumn("dbo.OwnerCertificationRecords", "CommunityName", c => c.String());
            AddColumn("dbo.OwnerCertificationRecords", "SmallDistrictId", c => c.String());
            AddColumn("dbo.OwnerCertificationRecords", "SmallDistrictName", c => c.String());
            AddColumn("dbo.OwnerCertificationRecords", "BuildingId", c => c.String());
            AddColumn("dbo.OwnerCertificationRecords", "BuildingName", c => c.String());
            AddColumn("dbo.OwnerCertificationRecords", "BuildingUnitId", c => c.String());
            AddColumn("dbo.OwnerCertificationRecords", "BuildingUnitName", c => c.String());
            AddColumn("dbo.OwnerCertificationRecords", "IndustryId", c => c.String());
            AddColumn("dbo.OwnerCertificationRecords", "IndustryName", c => c.String());
            AddColumn("dbo.OwnerCertificationRecords", "OwnerName", c => c.String());
            AddColumn("dbo.OwnerCertificationRecords", "IsInvalid", c => c.Boolean(nullable: false));
            AddColumn("dbo.User_Role", "Description", c => c.String());
            AddColumn("dbo.User_Role", "DepartmentName", c => c.String());
            AddColumn("dbo.User_Role", "DepartmentValue", c => c.String());
            AddColumn("dbo.Users", "State", c => c.String());
            AddColumn("dbo.Users", "City", c => c.String());
            AddColumn("dbo.Users", "Region", c => c.String());
            AddColumn("dbo.Users", "StreetOfficeId", c => c.String());
            AddColumn("dbo.Users", "StreetOfficeName", c => c.String());
            AddColumn("dbo.Users", "CommunityId", c => c.String());
            AddColumn("dbo.Users", "CommunityName", c => c.String());
            AddColumn("dbo.Users", "SmallDistrictId", c => c.String());
            AddColumn("dbo.Users", "SmallDistrictName", c => c.String());
            AddColumn("dbo.Users", "DepartmentName", c => c.String());
            AddColumn("dbo.Users", "DepartmentValue", c => c.String());
            AddColumn("dbo.VipOwnerApplicationRecords", "SmallDistrictId", c => c.String());
            AddColumn("dbo.VipOwnerApplicationRecords", "SmallDistrictName", c => c.String());
            AddColumn("dbo.VipOwnerApplicationRecords", "Name", c => c.String());
            AddColumn("dbo.VipOwnerCertificationAnnexes", "AnnexContent", c => c.String());
            DropColumn("dbo.OwnerCertificationRecords", "IsValid");
            DropColumn("dbo.Users", "IsOwner");
            DropColumn("dbo.Users", "IsVipOwner");
            DropColumn("dbo.VipOwnerCertificationAnnexes", "UploadId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.VipOwnerCertificationAnnexes", "UploadId", c => c.String());
            AddColumn("dbo.Users", "IsVipOwner", c => c.String());
            AddColumn("dbo.Users", "IsOwner", c => c.String());
            AddColumn("dbo.OwnerCertificationRecords", "IsValid", c => c.String());
            DropColumn("dbo.VipOwnerCertificationAnnexes", "AnnexContent");
            DropColumn("dbo.VipOwnerApplicationRecords", "Name");
            DropColumn("dbo.VipOwnerApplicationRecords", "SmallDistrictName");
            DropColumn("dbo.VipOwnerApplicationRecords", "SmallDistrictId");
            DropColumn("dbo.Users", "DepartmentValue");
            DropColumn("dbo.Users", "DepartmentName");
            DropColumn("dbo.Users", "SmallDistrictName");
            DropColumn("dbo.Users", "SmallDistrictId");
            DropColumn("dbo.Users", "CommunityName");
            DropColumn("dbo.Users", "CommunityId");
            DropColumn("dbo.Users", "StreetOfficeName");
            DropColumn("dbo.Users", "StreetOfficeId");
            DropColumn("dbo.Users", "Region");
            DropColumn("dbo.Users", "City");
            DropColumn("dbo.Users", "State");
            DropColumn("dbo.User_Role", "DepartmentValue");
            DropColumn("dbo.User_Role", "DepartmentName");
            DropColumn("dbo.User_Role", "Description");
            DropColumn("dbo.OwnerCertificationRecords", "IsInvalid");
            DropColumn("dbo.OwnerCertificationRecords", "OwnerName");
            DropColumn("dbo.OwnerCertificationRecords", "IndustryName");
            DropColumn("dbo.OwnerCertificationRecords", "IndustryId");
            DropColumn("dbo.OwnerCertificationRecords", "BuildingUnitName");
            DropColumn("dbo.OwnerCertificationRecords", "BuildingUnitId");
            DropColumn("dbo.OwnerCertificationRecords", "BuildingName");
            DropColumn("dbo.OwnerCertificationRecords", "BuildingId");
            DropColumn("dbo.OwnerCertificationRecords", "SmallDistrictName");
            DropColumn("dbo.OwnerCertificationRecords", "SmallDistrictId");
            DropColumn("dbo.OwnerCertificationRecords", "CommunityName");
            DropColumn("dbo.OwnerCertificationRecords", "CommunityId");
            DropColumn("dbo.OwnerCertificationRecords", "StreetOfficeName");
            DropColumn("dbo.OwnerCertificationRecords", "StreetOfficeId");
            DropTable("dbo.OwnerCertificationAnnexes");
        }
    }
}

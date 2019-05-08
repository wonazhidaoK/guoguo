namespace GuoGuoCommunity.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updataOwnerCertificationRecord : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.OwnerCertificationRecords", "IndustryId", c => c.Guid(nullable: false));
            AlterColumn("dbo.OwnerCertificationRecords", "OwnerId", c => c.Guid());
            CreateIndex("dbo.OwnerCertificationRecords", "IndustryId");
            CreateIndex("dbo.OwnerCertificationRecords", "OwnerId");
            AddForeignKey("dbo.OwnerCertificationRecords", "IndustryId", "dbo.Industries", "Id", cascadeDelete: true);
            AddForeignKey("dbo.OwnerCertificationRecords", "OwnerId", "dbo.Owners", "Id");
            DropColumn("dbo.OwnerCertificationRecords", "StreetOfficeId");
            DropColumn("dbo.OwnerCertificationRecords", "StreetOfficeName");
            DropColumn("dbo.OwnerCertificationRecords", "CommunityId");
            DropColumn("dbo.OwnerCertificationRecords", "CommunityName");
            DropColumn("dbo.OwnerCertificationRecords", "SmallDistrictId");
            DropColumn("dbo.OwnerCertificationRecords", "SmallDistrictName");
            DropColumn("dbo.OwnerCertificationRecords", "BuildingId");
            DropColumn("dbo.OwnerCertificationRecords", "BuildingName");
            DropColumn("dbo.OwnerCertificationRecords", "BuildingUnitId");
            DropColumn("dbo.OwnerCertificationRecords", "BuildingUnitName");
            DropColumn("dbo.OwnerCertificationRecords", "IndustryName");
            DropColumn("dbo.OwnerCertificationRecords", "OwnerName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OwnerCertificationRecords", "OwnerName", c => c.String());
            AddColumn("dbo.OwnerCertificationRecords", "IndustryName", c => c.String());
            AddColumn("dbo.OwnerCertificationRecords", "BuildingUnitName", c => c.String());
            AddColumn("dbo.OwnerCertificationRecords", "BuildingUnitId", c => c.String());
            AddColumn("dbo.OwnerCertificationRecords", "BuildingName", c => c.String());
            AddColumn("dbo.OwnerCertificationRecords", "BuildingId", c => c.String());
            AddColumn("dbo.OwnerCertificationRecords", "SmallDistrictName", c => c.String());
            AddColumn("dbo.OwnerCertificationRecords", "SmallDistrictId", c => c.String());
            AddColumn("dbo.OwnerCertificationRecords", "CommunityName", c => c.String());
            AddColumn("dbo.OwnerCertificationRecords", "CommunityId", c => c.String());
            AddColumn("dbo.OwnerCertificationRecords", "StreetOfficeName", c => c.String());
            AddColumn("dbo.OwnerCertificationRecords", "StreetOfficeId", c => c.String());
            DropForeignKey("dbo.OwnerCertificationRecords", "OwnerId", "dbo.Owners");
            DropForeignKey("dbo.OwnerCertificationRecords", "IndustryId", "dbo.Industries");
            DropIndex("dbo.OwnerCertificationRecords", new[] { "OwnerId" });
            DropIndex("dbo.OwnerCertificationRecords", new[] { "IndustryId" });
            AlterColumn("dbo.OwnerCertificationRecords", "OwnerId", c => c.String());
            AlterColumn("dbo.OwnerCertificationRecords", "IndustryId", c => c.String());
        }
    }
}

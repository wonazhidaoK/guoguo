namespace GuoGuoCommunity.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateComplaint : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Complaints", "ComplaintTypeId", c => c.Guid(nullable: false));
            AlterColumn("dbo.Complaints", "OwnerCertificationId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Complaints", "ComplaintTypeId");
            CreateIndex("dbo.Complaints", "OwnerCertificationId");
            AddForeignKey("dbo.Complaints", "ComplaintTypeId", "dbo.ComplaintTypes", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Complaints", "OwnerCertificationId", "dbo.OwnerCertificationRecords", "Id", cascadeDelete: true);
            DropColumn("dbo.Complaints", "ComplaintTypeName");
            DropColumn("dbo.Complaints", "StreetOfficeId");
            DropColumn("dbo.Complaints", "StreetOfficeName");
            DropColumn("dbo.Complaints", "CommunityId");
            DropColumn("dbo.Complaints", "CommunityName");
            DropColumn("dbo.Complaints", "SmallDistrictId");
            DropColumn("dbo.Complaints", "SmallDistrictName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Complaints", "SmallDistrictName", c => c.String());
            AddColumn("dbo.Complaints", "SmallDistrictId", c => c.String());
            AddColumn("dbo.Complaints", "CommunityName", c => c.String());
            AddColumn("dbo.Complaints", "CommunityId", c => c.String());
            AddColumn("dbo.Complaints", "StreetOfficeName", c => c.String());
            AddColumn("dbo.Complaints", "StreetOfficeId", c => c.String());
            AddColumn("dbo.Complaints", "ComplaintTypeName", c => c.String());
            DropForeignKey("dbo.Complaints", "OwnerCertificationId", "dbo.OwnerCertificationRecords");
            DropForeignKey("dbo.Complaints", "ComplaintTypeId", "dbo.ComplaintTypes");
            DropIndex("dbo.Complaints", new[] { "OwnerCertificationId" });
            DropIndex("dbo.Complaints", new[] { "ComplaintTypeId" });
            AlterColumn("dbo.Complaints", "OwnerCertificationId", c => c.String());
            AlterColumn("dbo.Complaints", "ComplaintTypeId", c => c.String());
        }
    }
}

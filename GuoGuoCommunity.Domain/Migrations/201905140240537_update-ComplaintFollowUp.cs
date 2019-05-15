namespace GuoGuoCommunity.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateComplaintFollowUp : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ComplaintFollowUps", "ComplaintId", c => c.Guid(nullable: false));
            AlterColumn("dbo.ComplaintFollowUps", "OwnerCertificationId", c => c.Guid());
            CreateIndex("dbo.ComplaintFollowUps", "ComplaintId");
            CreateIndex("dbo.ComplaintFollowUps", "OwnerCertificationId");
            AddForeignKey("dbo.ComplaintFollowUps", "ComplaintId", "dbo.Complaints", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ComplaintFollowUps", "OwnerCertificationId", "dbo.OwnerCertificationRecords", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ComplaintFollowUps", "OwnerCertificationId", "dbo.OwnerCertificationRecords");
            DropForeignKey("dbo.ComplaintFollowUps", "ComplaintId", "dbo.Complaints");
            DropIndex("dbo.ComplaintFollowUps", new[] { "OwnerCertificationId" });
            DropIndex("dbo.ComplaintFollowUps", new[] { "ComplaintId" });
            AlterColumn("dbo.ComplaintFollowUps", "OwnerCertificationId", c => c.String());
            AlterColumn("dbo.ComplaintFollowUps", "ComplaintId", c => c.String());
        }
    }
}

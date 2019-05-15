namespace GuoGuoCommunity.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateComplaintAnnex : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ComplaintAnnexes", "ComplaintId", c => c.Guid(nullable: false));
            AlterColumn("dbo.ComplaintAnnexes", "ComplaintFollowUpId", c => c.Guid());
            AlterColumn("dbo.ComplaintAnnexes", "AnnexId", c => c.Guid(nullable: false));
            CreateIndex("dbo.ComplaintAnnexes", "ComplaintId");
            CreateIndex("dbo.ComplaintAnnexes", "ComplaintFollowUpId");
            CreateIndex("dbo.ComplaintAnnexes", "AnnexId");
            AddForeignKey("dbo.ComplaintAnnexes", "ComplaintId", "dbo.Complaints", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ComplaintAnnexes", "ComplaintFollowUpId", "dbo.ComplaintFollowUps", "Id");
            AddForeignKey("dbo.ComplaintAnnexes", "AnnexId", "dbo.Uploads", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ComplaintAnnexes", "AnnexId", "dbo.Uploads");
            DropForeignKey("dbo.ComplaintAnnexes", "ComplaintFollowUpId", "dbo.ComplaintFollowUps");
            DropForeignKey("dbo.ComplaintAnnexes", "ComplaintId", "dbo.Complaints");
            DropIndex("dbo.ComplaintAnnexes", new[] { "AnnexId" });
            DropIndex("dbo.ComplaintAnnexes", new[] { "ComplaintFollowUpId" });
            DropIndex("dbo.ComplaintAnnexes", new[] { "ComplaintId" });
            AlterColumn("dbo.ComplaintAnnexes", "AnnexId", c => c.String());
            AlterColumn("dbo.ComplaintAnnexes", "ComplaintFollowUpId", c => c.String());
            AlterColumn("dbo.ComplaintAnnexes", "ComplaintId", c => c.String());
        }
    }
}

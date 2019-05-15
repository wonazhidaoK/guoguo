namespace GuoGuoCommunity.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateComplaintAnnex2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ComplaintAnnexes", "ComplaintId", "dbo.Complaints");
            DropIndex("dbo.ComplaintAnnexes", new[] { "ComplaintId" });
            DropColumn("dbo.ComplaintAnnexes", "ComplaintId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ComplaintAnnexes", "ComplaintId", c => c.Guid(nullable: false));
            CreateIndex("dbo.ComplaintAnnexes", "ComplaintId");
            AddForeignKey("dbo.ComplaintAnnexes", "ComplaintId", "dbo.Complaints", "Id", cascadeDelete: true);
        }
    }
}

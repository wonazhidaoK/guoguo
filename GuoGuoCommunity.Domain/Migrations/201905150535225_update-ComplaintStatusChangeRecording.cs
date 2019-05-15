namespace GuoGuoCommunity.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateComplaintStatusChangeRecording : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ComplaintStatusChangeRecordings", "ComplaintFollowUpId", c => c.Guid(nullable: false));
            CreateIndex("dbo.ComplaintStatusChangeRecordings", "ComplaintFollowUpId");
            AddForeignKey("dbo.ComplaintStatusChangeRecordings", "ComplaintFollowUpId", "dbo.ComplaintFollowUps", "Id", cascadeDelete: true);
            DropColumn("dbo.ComplaintStatusChangeRecordings", "ComplaintId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ComplaintStatusChangeRecordings", "ComplaintId", c => c.String());
            DropForeignKey("dbo.ComplaintStatusChangeRecordings", "ComplaintFollowUpId", "dbo.ComplaintFollowUps");
            DropIndex("dbo.ComplaintStatusChangeRecordings", new[] { "ComplaintFollowUpId" });
            AlterColumn("dbo.ComplaintStatusChangeRecordings", "ComplaintFollowUpId", c => c.String());
        }
    }
}

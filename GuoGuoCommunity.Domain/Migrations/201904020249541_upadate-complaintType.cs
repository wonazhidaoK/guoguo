namespace GuoGuoCommunity.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class upadatecomplaintType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Complaints", "ComplaintTypeName", c => c.String());
            AlterColumn("dbo.ComplaintTypes", "ProcessingPeriod", c => c.Int(nullable: false));
            AlterColumn("dbo.ComplaintTypes", "ComplaintPeriod", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ComplaintTypes", "ComplaintPeriod", c => c.String(nullable: false));
            AlterColumn("dbo.ComplaintTypes", "ProcessingPeriod", c => c.String(nullable: false));
            DropColumn("dbo.Complaints", "ComplaintTypeName");
        }
    }
}

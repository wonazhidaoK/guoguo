namespace GuoGuoCommunity.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VipOwners", "IsValid", c => c.Boolean(nullable: false));
            AlterColumn("dbo.ComplaintTypes", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.ComplaintTypes", "Level", c => c.String(nullable: false));
            AlterColumn("dbo.ComplaintTypes", "InitiatingDepartmentName", c => c.String(nullable: false));
            AlterColumn("dbo.ComplaintTypes", "InitiatingDepartmentValue", c => c.String(nullable: false));
            AlterColumn("dbo.ComplaintTypes", "ProcessingPeriod", c => c.String(nullable: false));
            AlterColumn("dbo.ComplaintTypes", "ComplaintPeriod", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ComplaintTypes", "ComplaintPeriod", c => c.String());
            AlterColumn("dbo.ComplaintTypes", "ProcessingPeriod", c => c.String());
            AlterColumn("dbo.ComplaintTypes", "InitiatingDepartmentValue", c => c.String());
            AlterColumn("dbo.ComplaintTypes", "InitiatingDepartmentName", c => c.String());
            AlterColumn("dbo.ComplaintTypes", "Level", c => c.String());
            AlterColumn("dbo.ComplaintTypes", "Name", c => c.String());
            DropColumn("dbo.VipOwners", "IsValid");
        }
    }
}

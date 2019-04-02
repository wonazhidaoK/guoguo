namespace GuoGuoCommunity.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateComplaint : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ComplaintFollowUps", "OwnerCertificationId", c => c.String());
            AddColumn("dbo.Complaints", "OperationDepartmentName", c => c.String());
            AddColumn("dbo.Complaints", "OperationDepartmentValue", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Complaints", "OperationDepartmentValue");
            DropColumn("dbo.Complaints", "OperationDepartmentName");
            DropColumn("dbo.ComplaintFollowUps", "OwnerCertificationId");
        }
    }
}

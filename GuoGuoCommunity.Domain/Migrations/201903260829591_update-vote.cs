namespace GuoGuoCommunity.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatevote : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Votes", "DepartmentName", c => c.String());
            AddColumn("dbo.Votes", "DepartmentValue", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Votes", "DepartmentValue");
            DropColumn("dbo.Votes", "DepartmentName");
        }
    }
}

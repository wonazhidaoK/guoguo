namespace GuoGuoCommunity.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateMenu : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Menus", "DepartmentName", c => c.String());
            AddColumn("dbo.Menus", "DepartmentValue", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Menus", "DepartmentValue");
            DropColumn("dbo.Menus", "DepartmentName");
        }
    }
}

namespace GuoGuoCommunity.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class roleMenuisDisplayed : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Role_Menu", "IsDisplayed", c => c.Boolean(nullable: false));
            DropColumn("dbo.Role_Menu", "IsDisplay");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Role_Menu", "IsDisplay", c => c.Boolean(nullable: false));
            DropColumn("dbo.Role_Menu", "IsDisplayed");
        }
    }
}

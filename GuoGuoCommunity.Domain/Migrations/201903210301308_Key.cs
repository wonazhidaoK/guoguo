namespace GuoGuoCommunity.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Key : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Menus", "Key", c => c.String());
            DropColumn("dbo.Menus", "Kay");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Menus", "Kay", c => c.String());
            DropColumn("dbo.Menus", "Key");
        }
    }
}

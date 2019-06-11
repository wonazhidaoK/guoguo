namespace GuoGuoCommunity.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateshop : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Shops", "UserId", "dbo.Users");
            DropForeignKey("dbo.GoodsTypes", "ShopId", "dbo.Shops");
            DropIndex("dbo.Shops", new[] { "UserId" });
            DropPrimaryKey("dbo.Shops");
            AddColumn("dbo.Shops", "Id", c => c.Guid(nullable: false, identity: true));
            AddColumn("dbo.Shops", "PhoneNumber", c => c.String());
            AddColumn("dbo.Users", "ShopId", c => c.Guid());
            AddPrimaryKey("dbo.Shops", "Id");
            CreateIndex("dbo.Users", "ShopId");
            AddForeignKey("dbo.Users", "ShopId", "dbo.Shops", "Id");
            AddForeignKey("dbo.GoodsTypes", "ShopId", "dbo.Shops", "Id", cascadeDelete: true);
            DropColumn("dbo.Shops", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Shops", "UserId", c => c.Guid(nullable: false));
            DropForeignKey("dbo.GoodsTypes", "ShopId", "dbo.Shops");
            DropForeignKey("dbo.Users", "ShopId", "dbo.Shops");
            DropIndex("dbo.Users", new[] { "ShopId" });
            DropPrimaryKey("dbo.Shops");
            DropColumn("dbo.Users", "ShopId");
            DropColumn("dbo.Shops", "PhoneNumber");
            DropColumn("dbo.Shops", "Id");
            AddPrimaryKey("dbo.Shops", "UserId");
            CreateIndex("dbo.Shops", "UserId");
            AddForeignKey("dbo.GoodsTypes", "ShopId", "dbo.Shops", "UserId", cascadeDelete: true);
            AddForeignKey("dbo.Shops", "UserId", "dbo.Users", "Id");
        }
    }
}

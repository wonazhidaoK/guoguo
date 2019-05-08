namespace GuoGuoCommunity.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateOwner : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Owners", "IndustryId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Owners", "IndustryId");
            AddForeignKey("dbo.Owners", "IndustryId", "dbo.Industries", "Id", cascadeDelete: true);
            DropColumn("dbo.Owners", "IndustryName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Owners", "IndustryName", c => c.String());
            DropForeignKey("dbo.Owners", "IndustryId", "dbo.Industries");
            DropIndex("dbo.Owners", new[] { "IndustryId" });
            AlterColumn("dbo.Owners", "IndustryId", c => c.String());
        }
    }
}

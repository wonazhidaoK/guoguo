namespace GuoGuoCommunity.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateIndustry : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Industries", "BuildingUnitId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Industries", "BuildingUnitId");
            AddForeignKey("dbo.Industries", "BuildingUnitId", "dbo.BuildingUnits", "Id", cascadeDelete: true);
            DropColumn("dbo.Industries", "BuildingId");
            DropColumn("dbo.Industries", "BuildingName");
            DropColumn("dbo.Industries", "BuildingUnitName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Industries", "BuildingUnitName", c => c.String());
            AddColumn("dbo.Industries", "BuildingName", c => c.String());
            AddColumn("dbo.Industries", "BuildingId", c => c.String(nullable: false));
            DropForeignKey("dbo.Industries", "BuildingUnitId", "dbo.BuildingUnits");
            DropIndex("dbo.Industries", new[] { "BuildingUnitId" });
            AlterColumn("dbo.Industries", "BuildingUnitId", c => c.String());
        }
    }
}

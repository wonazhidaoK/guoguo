namespace GuoGuoCommunity.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateBuildingUnit : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.BuildingUnits", "BuildingId", c => c.Guid(nullable: false));
            CreateIndex("dbo.BuildingUnits", "BuildingId");
            AddForeignKey("dbo.BuildingUnits", "BuildingId", "dbo.Buildings", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BuildingUnits", "BuildingId", "dbo.Buildings");
            DropIndex("dbo.BuildingUnits", new[] { "BuildingId" });
            AlterColumn("dbo.BuildingUnits", "BuildingId", c => c.String(nullable: false));
        }
    }
}

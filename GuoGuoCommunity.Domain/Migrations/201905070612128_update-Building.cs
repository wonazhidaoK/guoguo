namespace GuoGuoCommunity.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateBuilding : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Buildings", "SmallDistrictId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Buildings", "SmallDistrictId");
            AddForeignKey("dbo.Buildings", "SmallDistrictId", "dbo.SmallDistricts", "Id", cascadeDelete: true);
            DropColumn("dbo.Buildings", "SmallDistrictName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Buildings", "SmallDistrictName", c => c.String(nullable: false));
            DropForeignKey("dbo.Buildings", "SmallDistrictId", "dbo.SmallDistricts");
            DropIndex("dbo.Buildings", new[] { "SmallDistrictId" });
            AlterColumn("dbo.Buildings", "SmallDistrictId", c => c.String(nullable: false));
        }
    }
}

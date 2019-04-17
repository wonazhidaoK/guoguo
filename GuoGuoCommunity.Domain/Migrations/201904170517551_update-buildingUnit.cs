namespace GuoGuoCommunity.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatebuildingUnit : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.BuildingUnits", "NumberOfLayers", c => c.Int(nullable: false));
            AlterColumn("dbo.Industries", "NumberOfLayers", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Industries", "NumberOfLayers", c => c.String());
            AlterColumn("dbo.BuildingUnits", "NumberOfLayers", c => c.String());
        }
    }
}

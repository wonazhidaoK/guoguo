namespace GuoGuoCommunity.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateVipOwner : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.VipOwners", "SmallDistrictId", c => c.Guid(nullable: false));
            CreateIndex("dbo.VipOwners", "SmallDistrictId");
            AddForeignKey("dbo.VipOwners", "SmallDistrictId", "dbo.SmallDistricts", "Id", cascadeDelete: true);
            DropColumn("dbo.VipOwners", "SmallDistrictName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.VipOwners", "SmallDistrictName", c => c.String(nullable: false));
            DropForeignKey("dbo.VipOwners", "SmallDistrictId", "dbo.SmallDistricts");
            DropIndex("dbo.VipOwners", new[] { "SmallDistrictId" });
            AlterColumn("dbo.VipOwners", "SmallDistrictId", c => c.String(nullable: false));
        }
    }
}

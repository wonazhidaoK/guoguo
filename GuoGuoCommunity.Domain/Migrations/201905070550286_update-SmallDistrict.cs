namespace GuoGuoCommunity.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateSmallDistrict : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SmallDistricts", "CommunityId", c => c.Guid(nullable: false));
            CreateIndex("dbo.SmallDistricts", "CommunityId");
            AddForeignKey("dbo.SmallDistricts", "CommunityId", "dbo.Communities", "Id", cascadeDelete: true);
            DropColumn("dbo.SmallDistricts", "StreetOfficeId");
            DropColumn("dbo.SmallDistricts", "StreetOfficeName");
            DropColumn("dbo.SmallDistricts", "CommunityName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SmallDistricts", "CommunityName", c => c.String(nullable: false));
            AddColumn("dbo.SmallDistricts", "StreetOfficeName", c => c.String(nullable: false));
            AddColumn("dbo.SmallDistricts", "StreetOfficeId", c => c.String(nullable: false));
            DropForeignKey("dbo.SmallDistricts", "CommunityId", "dbo.Communities");
            DropIndex("dbo.SmallDistricts", new[] { "CommunityId" });
            AlterColumn("dbo.SmallDistricts", "CommunityId", c => c.String(nullable: false));
        }
    }
}

namespace GuoGuoCommunity.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateCommunity : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Communities", "StreetOfficeId", c => c.Guid(nullable: false));
            CreateIndex("dbo.Communities", "StreetOfficeId");
            AddForeignKey("dbo.Communities", "StreetOfficeId", "dbo.StreetOffices", "Id", cascadeDelete: true);
            DropColumn("dbo.Communities", "StreetOfficeName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Communities", "StreetOfficeName", c => c.String(nullable: false));
            DropForeignKey("dbo.Communities", "StreetOfficeId", "dbo.StreetOffices");
            DropIndex("dbo.Communities", new[] { "StreetOfficeId" });
            AlterColumn("dbo.Communities", "StreetOfficeId", c => c.String(nullable: false));
        }
    }
}

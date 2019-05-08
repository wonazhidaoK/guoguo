namespace GuoGuoCommunity.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_SmallDistrict : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.SmallDistricts", "IsInvalid");
            DropColumn("dbo.SmallDistricts", "IsElection");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SmallDistricts", "IsElection", c => c.Boolean(nullable: false));
            AddColumn("dbo.SmallDistricts", "IsInvalid", c => c.String());
        }
    }
}

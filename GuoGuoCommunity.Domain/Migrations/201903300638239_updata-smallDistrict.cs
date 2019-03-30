namespace GuoGuoCommunity.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatasmallDistrict : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SmallDistricts", "IsInvalid", c => c.String());
            AddColumn("dbo.SmallDistricts", "IsElection", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SmallDistricts", "IsElection");
            DropColumn("dbo.SmallDistricts", "IsInvalid");
        }
    }
}

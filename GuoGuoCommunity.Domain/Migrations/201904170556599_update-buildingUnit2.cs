namespace GuoGuoCommunity.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatebuildingUnit2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Owners", "OwnerCertificationRecordId", c => c.String());
            AddColumn("dbo.Owners", "IsLegalize", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Owners", "IsLegalize");
            DropColumn("dbo.Owners", "OwnerCertificationRecordId");
        }
    }
}

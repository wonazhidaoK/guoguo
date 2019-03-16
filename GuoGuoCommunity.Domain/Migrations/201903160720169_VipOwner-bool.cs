namespace GuoGuoCommunity.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VipOwnerbool : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.VipOwnerApplicationRecords", "IsInvalid", c => c.Boolean(nullable: false));
            AlterColumn("dbo.VipOwnerCertificationRecords", "IsInvalid", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.VipOwnerCertificationRecords", "IsInvalid", c => c.String());
            AlterColumn("dbo.VipOwnerApplicationRecords", "IsInvalid", c => c.String());
        }
    }
}

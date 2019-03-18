namespace GuoGuoCommunity.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VipOwnerApplicationRecordaddIsAdopt : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.VipOwnerApplicationRecords", "IsAdopt", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.VipOwnerApplicationRecords", "IsAdopt");
        }
    }
}

namespace GuoGuoCommunity.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateOwnerCertificationAnnex : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.OwnerCertificationAnnexes", "ApplicationRecordId", c => c.Guid(nullable: false));
            CreateIndex("dbo.OwnerCertificationAnnexes", "ApplicationRecordId");
            AddForeignKey("dbo.OwnerCertificationAnnexes", "ApplicationRecordId", "dbo.OwnerCertificationRecords", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OwnerCertificationAnnexes", "ApplicationRecordId", "dbo.OwnerCertificationRecords");
            DropIndex("dbo.OwnerCertificationAnnexes", new[] { "ApplicationRecordId" });
            AlterColumn("dbo.OwnerCertificationAnnexes", "ApplicationRecordId", c => c.String());
        }
    }
}

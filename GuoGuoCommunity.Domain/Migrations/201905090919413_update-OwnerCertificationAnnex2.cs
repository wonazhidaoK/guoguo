namespace GuoGuoCommunity.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateOwnerCertificationAnnex2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.OwnerCertificationAnnexes", "AnnexId", c => c.Guid(nullable: false));
            CreateIndex("dbo.OwnerCertificationAnnexes", "AnnexId");
            AddForeignKey("dbo.OwnerCertificationAnnexes", "AnnexId", "dbo.Uploads", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OwnerCertificationAnnexes", "AnnexId", "dbo.Uploads");
            DropIndex("dbo.OwnerCertificationAnnexes", new[] { "AnnexId" });
            AlterColumn("dbo.OwnerCertificationAnnexes", "AnnexId", c => c.String());
        }
    }
}

namespace GuoGuoCommunity.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateownerCertificationRecord : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.OwnerCertificationRecords", "CertificationTime", c => c.DateTimeOffset(precision: 7));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.OwnerCertificationRecords", "CertificationTime", c => c.String());
        }
    }
}

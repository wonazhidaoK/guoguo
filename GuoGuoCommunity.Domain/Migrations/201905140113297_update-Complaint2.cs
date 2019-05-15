namespace GuoGuoCommunity.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateComplaint2 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Complaints", name: "OwnerCertificationId", newName: "OwnerCertificationRecordId");
            RenameIndex(table: "dbo.Complaints", name: "IX_OwnerCertificationId", newName: "IX_OwnerCertificationRecordId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Complaints", name: "IX_OwnerCertificationRecordId", newName: "IX_OwnerCertificationId");
            RenameColumn(table: "dbo.Complaints", name: "OwnerCertificationRecordId", newName: "OwnerCertificationId");
        }
    }
}

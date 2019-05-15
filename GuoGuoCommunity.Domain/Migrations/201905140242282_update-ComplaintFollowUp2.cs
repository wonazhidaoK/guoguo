namespace GuoGuoCommunity.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateComplaintFollowUp2 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.ComplaintFollowUps", name: "OwnerCertificationId", newName: "OwnerCertificationRecordId");
            RenameIndex(table: "dbo.ComplaintFollowUps", name: "IX_OwnerCertificationId", newName: "IX_OwnerCertificationRecordId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.ComplaintFollowUps", name: "IX_OwnerCertificationRecordId", newName: "IX_OwnerCertificationId");
            RenameColumn(table: "dbo.ComplaintFollowUps", name: "OwnerCertificationRecordId", newName: "OwnerCertificationId");
        }
    }
}

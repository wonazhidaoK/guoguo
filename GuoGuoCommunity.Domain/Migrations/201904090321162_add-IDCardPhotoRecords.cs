namespace GuoGuoCommunity.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addIDCardPhotoRecords : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.IDCardPhotoRecords",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        ApplicationRecordId = c.String(),
                        OwnerCertificationAnnexId = c.String(),
                        PhotoBase64 = c.String(),
                        Message = c.String(),
                        CreateOperationUserId = c.String(),
                        CreateOperationTime = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.IDCardPhotoRecords");
        }
    }
}

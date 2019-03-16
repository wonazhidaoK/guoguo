namespace GuoGuoCommunity.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VipOwner : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Uploads",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Agreement = c.String(),
                        Host = c.String(),
                        Domain = c.String(),
                        Directory = c.String(),
                        File = c.String(),
                        LastOperationUserId = c.String(),
                        LastOperationTime = c.DateTimeOffset(precision: 7),
                        CreateOperationUserId = c.String(),
                        CreateOperationTime = c.DateTimeOffset(precision: 7),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedTime = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VipOwnerApplicationRecords",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        UserId = c.String(),
                        StructureId = c.String(),
                        StructureName = c.String(),
                        Reason = c.String(),
                        IsInvalid = c.String(),
                        LastOperationUserId = c.String(),
                        LastOperationTime = c.DateTimeOffset(precision: 7),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedTime = c.DateTimeOffset(precision: 7),
                        CreateOperationUserId = c.String(),
                        CreateOperationTime = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VipOwnerCertificationAnnexes",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        CertificationConditionId = c.String(),
                        ApplicationRecordId = c.String(),
                        UploadId = c.String(),
                        CreateOperationUserId = c.String(),
                        CreateOperationTime = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VipOwnerCertificationConditions",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        TypeName = c.String(),
                        TypeValue = c.String(),
                        CreateOperationUserId = c.String(),
                        CreateOperationTime = c.DateTimeOffset(precision: 7),
                        LastOperationUserId = c.String(),
                        LastOperationTime = c.DateTimeOffset(precision: 7),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedTime = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VipOwnerCertificationRecords",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        VipOwnerId = c.String(),
                        VipOwnerName = c.String(),
                        VipOwnerStructureId = c.String(),
                        VipOwnerStructureName = c.String(),
                        UserId = c.String(),
                        IsInvalid = c.String(),
                        LastOperationUserId = c.String(),
                        LastOperationTime = c.DateTimeOffset(precision: 7),
                        IsDeleted = c.Boolean(nullable: false),
                        DeletedTime = c.DateTimeOffset(precision: 7),
                        CreateOperationUserId = c.String(),
                        CreateOperationTime = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.VipOwnerCertificationRecords");
            DropTable("dbo.VipOwnerCertificationConditions");
            DropTable("dbo.VipOwnerCertificationAnnexes");
            DropTable("dbo.VipOwnerApplicationRecords");
            DropTable("dbo.Uploads");
        }
    }
}

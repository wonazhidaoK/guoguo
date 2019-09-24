namespace GuoGuoCommunity.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateShoppingTrolleysUser : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ShoppingTrolleys", "OwnerCertificationRecordId", "dbo.OwnerCertificationRecords");
            DropIndex("dbo.ShoppingTrolleys", new[] { "OwnerCertificationRecordId" });
            AlterColumn("dbo.ShoppingTrolleys", "CreateOperationUserId", c => c.Guid(nullable: false));
            CreateIndex("dbo.ShoppingTrolleys", "CreateOperationUserId");
            AddForeignKey("dbo.ShoppingTrolleys", "CreateOperationUserId", "dbo.Users", "Id", cascadeDelete: true);
            DropColumn("dbo.ShoppingTrolleys", "OwnerCertificationRecordId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ShoppingTrolleys", "OwnerCertificationRecordId", c => c.Guid(nullable: false));
            DropForeignKey("dbo.ShoppingTrolleys", "CreateOperationUserId", "dbo.Users");
            DropIndex("dbo.ShoppingTrolleys", new[] { "CreateOperationUserId" });
            AlterColumn("dbo.ShoppingTrolleys", "CreateOperationUserId", c => c.String());
            CreateIndex("dbo.ShoppingTrolleys", "OwnerCertificationRecordId");
            AddForeignKey("dbo.ShoppingTrolleys", "OwnerCertificationRecordId", "dbo.OwnerCertificationRecords", "Id", cascadeDelete: true);
        }
    }
}

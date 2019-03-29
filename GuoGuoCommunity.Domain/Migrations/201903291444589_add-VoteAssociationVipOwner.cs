namespace GuoGuoCommunity.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addVoteAssociationVipOwner : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.VoteAssociationVipOwners",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        VoteId = c.String(),
                        VipOwnerId = c.String(),
                        CreateOperationUserId = c.String(),
                        CreateOperationTime = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Announcements", "OwnerCertificationId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Announcements", "OwnerCertificationId");
            DropTable("dbo.VoteAssociationVipOwners");
        }
    }
}

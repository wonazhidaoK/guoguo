using GuoGuoCommunity.Domain.Models;
using System.Data.Entity.Migrations;
namespace GuoGuoCommunity.Domain.Migrations
{

    internal sealed class Configuration : DbMigrationsConfiguration<GuoGuoCommunityContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(GuoGuoCommunityContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            context.Tests.AddOrUpdate(x => x.Id,
                new Test() { Id = 1, Name = "Jane Austen" },
                new Test() { Id = 2, Name = "Charles Dickens" }
                );
        }
    }
}

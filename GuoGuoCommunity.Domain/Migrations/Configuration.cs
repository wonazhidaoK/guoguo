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
            //This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.

            context.Tests.AddOrUpdate(x => x.Name,
                new Test() { Id = 1, Name = "Jane Austen" },
                new Test() { Id = 2, Name = "Charles Dickens" }
                );
            context.Menus.AddOrUpdate(x => x.Name,
                new Menu() { Name = "��ҵ��̨-ҵ����Ϣ", Key = "wy_house_info" },
                //new Menu() { Name = "��ҵ��̨-��֤����", Key = "wy_rz" },
                //new Menu() { Name = "��ҵ��̨-ҵ����֤", Key = "wy_rz_yz" },
                new Menu() { Name = "��ҵ��̨-����", Key = "wy_notice" },
                
                new Menu() { Name = "��ҵ��̨-ͶƱ", Key = "wy_vote" },
                new Menu() { Name = "��ҵ��̨-Ͷ��", Key = "wy_complaint" },
                new Menu() { Name = "��ҵ��̨-վ����", Key = "wy_letter" },
                new Menu() { Name = "�ֵ����̨-����", Key = "jdb_notice" },
                new Menu() { Name = "�ֵ���-�߼���֤", Key = "jdb_rz_gj" },
                new Menu() { Name = "�ֵ����̨-ͶƱ", Key = "jdb_vote" },
                new Menu() { Name = "�ֵ����̨-Ͷ��", Key = "jdb_complaint" },
                new Menu() { Name = "�ֵ����̨-վ����", Key = "jdb_letter" },
                new Menu() { Name = "ϵͳ����Ա", Key = "authorityMax" }
                );
            context.Users.AddOrUpdate(x => x.Name,
                new User()
                {
                    Name = "admin",
                    Password = "123456",
                    PhoneNumber = "13888888888",
                    State = "������",
                    City = "������",
                    Region = "������"
                });
            context.VipOwnerCertificationConditions.AddOrUpdate(x => x.Title,
                new VipOwnerCertificationCondition()
                {
                    Title = "�޷���֤��",
                    Description = "Ͻ�����ɳ��������޷���֤��",
                    TypeName = "ͼƬ",
                    TypeValue = "Image",
                },
                new VipOwnerCertificationCondition()
                {
                    Title = "��λ������",
                    Description = "ҵ���ṩ�����ڵ�λ������",
                    TypeName = "ͼƬ",
                    TypeValue = "Image",
                },
                new VipOwnerCertificationCondition()
                {
                    Title = "��ҵ�ɷ�Ʊ��",
                    Description = "�ṩҵ���������ҵ�ѽɷ�Ʊ��",
                    TypeName = "ͼƬ",
                    TypeValue = "Image",
                });
        }
    }
}

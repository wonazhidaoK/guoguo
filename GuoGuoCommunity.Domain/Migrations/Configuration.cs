using GuoGuoCommunity.Domain.Models;
using GuoGuoCommunity.Domain.Models.Enum;
using System;
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
            try
            {
                //This method will be called after migrating to the latest version.

                //  You can use the DbSet<T>.AddOrUpdate() helper extension method
                //  to avoid creating duplicate seed data.

                context.Tests.AddOrUpdate(x => x.Name,
                    new Test() { Id = 1, Name = "Jane Austen" },
                    new Test() { Id = 2, Name = "Charles Dickens" }
                    );
                context.Menus.AddOrUpdate(x => x.Name,
                    new Menu() { Name = "��ҵ��̨-ҵ����Ϣ", Key = "wy_house_info", DepartmentValue = Department.WuYe.Value, DepartmentName = Department.WuYe.Name },
                    //new Menu() { Name = "��ҵ��̨-��֤����", Key = "wy_rz" },
                    //new Menu() { Name = "��ҵ��̨-ҵ����֤", Key = "wy_rz_yz" },
                    new Menu() { Name = "��ҵ��̨-����", Key = "wy_notice", DepartmentValue = Department.WuYe.Value, DepartmentName = Department.WuYe.Name },
                    new Menu() { Name = "��ҵ��̨-ͶƱ", Key = "wy_vote", DepartmentValue = Department.WuYe.Value, DepartmentName = Department.WuYe.Name },
                    new Menu() { Name = "��ҵ��̨-Ͷ��", Key = "wy_complaint", DepartmentValue = Department.WuYe.Value, DepartmentName = Department.WuYe.Name },
                    new Menu() { Name = "��ҵ��̨-վ����", Key = "wy_letter", DepartmentValue = Department.WuYe.Value, DepartmentName = Department.WuYe.Name },
                    new Menu() { Name = "�ֵ����̨-����", Key = "jdb_notice", DepartmentValue = Department.JieDaoBan.Value, DepartmentName = Department.JieDaoBan.Name },
                    new Menu() { Name = "�ֵ���-�߼���֤", Key = "jdb_rz_gj", DepartmentValue = Department.JieDaoBan.Value, DepartmentName = Department.JieDaoBan.Name },
                    new Menu() { Name = "�ֵ����̨-ͶƱ", Key = "jdb_vote", DepartmentValue = Department.JieDaoBan.Value, DepartmentName = Department.JieDaoBan.Name },
                    new Menu() { Name = "�ֵ����̨-Ͷ��", Key = "jdb_complaint", DepartmentValue = Department.JieDaoBan.Value, DepartmentName = Department.JieDaoBan.Name },
                    new Menu() { Name = "�ֵ����̨-վ����", Key = "jdb_letter", DepartmentValue = Department.JieDaoBan.Value, DepartmentName = Department.JieDaoBan.Name },
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
                        Region = "������",
                        Account = "admin"
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
                context.ComplaintTypes.AddOrUpdate(x =>new { x.Name ,x.InitiatingDepartmentValue},
                    new ComplaintType()
                    {
                        Name = "������������",
                        ComplaintPeriod = 15,
                        Description = "��ˮ�����ѡ��³���¥���Ȳ��������Ͻ��Ŵ���б�����治ƽ������װʩ��������",
                        InitiatingDepartmentName = "ҵ��",
                        InitiatingDepartmentValue = "Owner",
                        Level = "8",
                        ProcessingPeriod = 7,
                        // Id = Guid.Parse("11e99bb9-505b-e911-80e8-9e79ab1b7f85")
                    }, new ComplaintType()
                    {
                        Name = "�����ռ䡢������������",
                        ComplaintPeriod = 15,
                        Description = "��˨��װӰ��ҵ��ͨ�С�¥��������խ������ͨ����ͨ����������¥���Ĳ�����Ʋ�������ˮ������",
                        InitiatingDepartmentName = "ҵ��",
                        InitiatingDepartmentValue = "Owner",
                        Level = "7",
                        ProcessingPeriod = 7,
                        //Id = Guid.Parse("21e99bb9-505b-e911-80e8-9e79ab1b7f85")
                    }, new ComplaintType()
                    {
                        Name = "�豸��ʩ��������ʩ����",
                        ComplaintPeriod = 15,
                        Description = "���ݳ������ء����ݡ����ˡ�ͣ���շѵ�բ��װ��б���ϡ����е���խ���г��������е����ֿ���С����Χ����ʽ",
                        InitiatingDepartmentName = "ҵ��",
                        InitiatingDepartmentValue = "Owner",
                        Level = "6",
                        ProcessingPeriod = 7,
                        //Id = Guid.Parse("31e99bb9-505b-e911-80e8-9e79ab1b7f85")
                    }, new ComplaintType()
                    {
                        Name = "������ơ��̻�����",
                        ComplaintPeriod = 15,
                        Description = "������¶��ֲ�ﳤ�ƽϲ������ˮ�������̵���Χ�����º��ڹ����Ѷ����ӡ�����ˮ������ʾ��־��ˮ����ȳ���1.8�ס���������Ӱ�����е�����ľ�����ѡ���ͺ�",
                        InitiatingDepartmentName = "ҵ��",
                        InitiatingDepartmentValue = "Owner",
                        Level = "5",
                        ProcessingPeriod = 7,
                        // Id = Guid.Parse("41e99bb9-505b-e911-80e8-9e79ab1b7f85")
                    }, new ComplaintType()
                    {
                        Name = "С�����ܵ�������Ⱦ",
                        ComplaintPeriod = 15,
                        Description = "��ʩ����װ�ޡ��������ˡ�ҵ�����衢��������������",
                        InitiatingDepartmentName = "ҵ��",
                        InitiatingDepartmentValue = "Owner",
                        Level = "4",
                        ProcessingPeriod = 7,
                        //Id = Guid.Parse("51e99bb9-505b-e911-80e8-9e79ab1b7f85")
                    }, new ComplaintType()
                    {
                        Name = "��ҵ����Ѽ�������������",
                        ComplaintPeriod = 15,
                        Description = "��Ϊ���ú����ܲ��Եȡ����ݷѺͶ��μ�ѹ�ѹ��ߡ���ˮ�ѹ��ߡ�װ�޸�����ø�",
                        InitiatingDepartmentName = "ҵ��ίԱ��",
                        InitiatingDepartmentValue = "VipOwner",
                        Level = "3",
                        ProcessingPeriod = 7,
                        // Id = Guid.Parse("61e99bb9-505b-e911-80e8-9e79ab1b7f85")
                    }, new ComplaintType()
                    {
                        Name = "��ҵ���񲻵�λ",
                        ComplaintPeriod = 15,
                        Description = "��ò���ȱ�������������������ҵ��˵����̬�ȶ��ӡ���װ���������ˡ�����ҵ������λ˯������λ�ϴ�ţ���졢����Ϊ�����ó�Ƥ",
                        InitiatingDepartmentName = "ҵ��ίԱ��",
                        InitiatingDepartmentValue = "VipOwner",
                        Level = "2",
                        ProcessingPeriod = 7,
                        // Id = Guid.Parse("71e99bb9-505b-e911-80e8-9e79ab1b7f85")
                    }, new ComplaintType()
                    {
                        Name = "��ҵ����͸��",
                        ComplaintPeriod = 15,
                        Description = "���á����̡���׼����ŵ����������Ӫ��Ϳ�ˣ���֧����ʾ���������",
                        InitiatingDepartmentName = "ҵ��ίԱ��",
                        InitiatingDepartmentValue = "VipOwner",
                        Level = "1",
                        ProcessingPeriod = 7,
                        // Id = Guid.Parse("81e99bb9-505b-e911-80e8-9e79ab1b7f85")
                    });
            }
            catch (Exception e)
            {
                throw new NotImplementedException(e.Message);
            }
        }
    }
}

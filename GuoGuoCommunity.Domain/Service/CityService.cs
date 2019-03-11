using GuoGuoCommunity.Domain.Abstractions;
using GuoGuoCommunity.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GuoGuoCommunity.Domain.Service
{
    public class CityService : ICityService
    {
        string filename = "D:/project/GuoGuo/GuoGuoCommunity/GuoGuoCommunity.Domain/Models/city/Area_List.xml";
        string a = AppDomain.CurrentDomain.BaseDirectory;
        string ass=  ConfigurationManager.AppSettings["cityXml"].ToString();
        //string aa = Environment.CurrentDirectory;
        //cityXml

        public async Task<List<CityDto>> GetCity(CityDto dto)
        {
           
            var currentDirectory = Directory.GetCurrentDirectory();
            var purchaseOrderFilepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "/city/Area_List.xml");
            XElement purchaseOrder = XElement.Load($"{filename}");

            var stateXElement = (from item in purchaseOrder.Elements("CountryRegion")
                                 where (string)item.Attribute("Name") == "中国"
                                 select item).First();
            var city = (from item in stateXElement.Elements("State")
                        where (string)item.Attribute("Name") == dto.Name
                        select item.Descendants("City")).First().Select(x => new CityDto
                        {
                            Code = (string)x.Attribute("Code"),
                            Name = (string)x.Attribute("Name"),
                        }).ToList();

            return city;
        }

        public async Task<List<CityDto>> GetRegion(RegionDto dto)
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var purchaseOrderFilepath = Path.Combine(currentDirectory, filename);
            XElement purchaseOrder = XElement.Load($"{filename}");
            var stateXElement = (from item in purchaseOrder.Elements("CountryRegion")
                                 where (string)item.Attribute("Name") == "中国"
                                 select item).First();
            var cityXElement = (from item in stateXElement.Elements("State")
                         where (string)item.Attribute("Name") == dto.CityDto.Name
                         select item).First();
            var region = (from item in cityXElement.Elements("City")
                          where (string)item.Attribute("Name") == dto.Name
                          select item.Descendants("Region")).First().Select(x => new CityDto
                          {
                              Code = (string)x.Attribute("Code"),
                              Name = (string)x.Attribute("Name"),
                          }).ToList();
            return region;
        }


        public async Task<List<CityDto>> GetState()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var purchaseOrderFilepath = Path.Combine(currentDirectory, filename);
            XElement purchaseOrder = XElement.Load($"{filename}");
            var state = (from item in purchaseOrder.Elements("CountryRegion")
                           where (string)item.Attribute("Name") == "中国"
                           select item.Descendants("State")).First().Select(x => new CityDto
                           {
                               Code = (string)x.Attribute("Code"),
                               Name = (string)x.Attribute("Name"),
                           }).ToList();
            return state;
        }
    }
}

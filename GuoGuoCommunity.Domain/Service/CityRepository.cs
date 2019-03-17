using GuoGuoCommunity.Domain.Abstractions;
using GuoGuoCommunity.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GuoGuoCommunity.Domain.Service
{
    public class CityRepository : ICityRepository
    {
        readonly string filename =AppDomain.CurrentDomain.BaseDirectory+ConfigurationManager.AppSettings["cityXml"].ToString();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<List<CityDto>> GetCity(CityDto dto)
        {
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

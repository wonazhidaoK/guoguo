using GuoGuoCommunity.Domain.Abstractions;
using GuoGuoCommunity.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace GuoGuoCommunity.Domain.Service
{
    public class CityRepository : ICityRepository
    {
        readonly string filename =AppDomain.CurrentDomain.BaseDirectory+ConfigurationManager.AppSettings["cityXml"].ToString();
        readonly string appFilename = AppDomain.CurrentDomain.BaseDirectory + ConfigurationManager.AppSettings["appCityXml"].ToString();
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



        public static T DESerializer<T>(string strXML) where T : class
        {
            try
            {
                using (StringReader sr = new StringReader(strXML))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    return serializer.Deserialize(sr) as T;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<ModelCountryState>> Linkage(RegionDto dto)
        {
            
            var aa =await GetState();
            List<ModelCountryState> modelCountryState = new List<ModelCountryState>();
            foreach (var item in aa)
            {
                var bb = await GetCity(item);
                List<Citys> City = new List<Citys>();
                foreach (var item2 in bb)
                {
                    List<string> Region = new List<string>();
                    var cc = await GetRegion(new RegionDto {
                         CityDto=item,
                          Name=item2.Name
                    });
                    foreach (var item3 in cc)
                    {
                        Region.Add( item3.Name );
                    }
                    City.Add(new Citys {  Name= item2.Name ,  Area= Region });
                }
                modelCountryState.Add(new ModelCountryState {City= City , Name = item .Name});
            }
            
            return modelCountryState ;
        }


    }

    public class CountryRegions
    {
        //public string CountryRegion { get; set; }
        public List<ModelCountryState> State { get; set; }
    }
    public class ModelCountryState
    {
        public string Name { get; set; }

        public List<Citys> City { get; set; }
    }
    public class Citys
    {
        public string Name { get; set; }

        public List<string> Area { get; set; }
    }

    public class Regions
    {
        public string Region { get; set; }
    }
}

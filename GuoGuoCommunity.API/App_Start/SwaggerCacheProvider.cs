﻿using Swashbuckle.Swagger;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace GuoGuoCommunity.API
{
    /// <summary>
    /// swagger显示控制器的描述
    /// </summary>
    public class SwaggerCacheProvider : ISwaggerProvider
    {
        private readonly ISwaggerProvider _swaggerProvider;
        private static readonly ConcurrentDictionary<string, SwaggerDocument> _cache = new ConcurrentDictionary<string, SwaggerDocument>();
        private readonly string _xml;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="swaggerProvider"></param>
        /// <param name="xml">xml文档路径</param>
        public SwaggerCacheProvider(ISwaggerProvider swaggerProvider, string xml)
        {
            _swaggerProvider = swaggerProvider;
            _xml = xml;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rootUrl"></param>
        /// <param name="apiVersion"></param>
        /// <returns></returns>
        public SwaggerDocument GetSwagger(string rootUrl, string apiVersion)
        {

            var cacheKey = string.Format("{0}_{1}", rootUrl, apiVersion);
            //只读取一次
            if (!_cache.TryGetValue(cacheKey, out SwaggerDocument srcDoc))
            {
                srcDoc = _swaggerProvider.GetSwagger(rootUrl, apiVersion);

                srcDoc.vendorExtensions = new Dictionary<string, object> { { "ControllerDesc", GetControllerDesc() } };
                _cache.TryAdd(cacheKey, srcDoc);
            }
            return srcDoc;
        }

        /// <summary>
        /// 从API文档中读取控制器描述
        /// </summary>
        /// <returns>所有控制器描述</returns>
        public ConcurrentDictionary<string, string> GetControllerDesc()
        {
            string xmlpath = _xml;
            ConcurrentDictionary<string, string> controllerDescDict = new ConcurrentDictionary<string, string>();
            if (File.Exists(xmlpath))
            {
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(xmlpath);
                string[] arrPath;
                int cCount = "Controller".Length;
                foreach (XmlNode node in xmldoc.SelectNodes("//member"))
                {
                    string type = node.Attributes["name"].Value;
                    if (type.StartsWith("T:"))
                    {
                        //控制器
                        arrPath = type.Split('.');
                        int length = arrPath.Length;
                        string controllerName = arrPath[length - 1];
                        if (controllerName.EndsWith("Controller"))
                        {
                            //获取控制器注释
                            XmlNode summaryNode = node.SelectSingleNode("summary");
                            string key = controllerName.Remove(controllerName.Length - cCount, cCount);
                            if (summaryNode != null && !string.IsNullOrEmpty(summaryNode.InnerText) && !controllerDescDict.ContainsKey(key))
                            {
                                controllerDescDict.TryAdd(key, summaryNode.InnerText.Trim());
                            }
                        }
                    }
                }
            }
            return controllerDescDict;
        }
    }
}
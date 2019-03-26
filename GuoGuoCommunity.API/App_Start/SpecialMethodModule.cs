using System;
using System.Web;

namespace GuoGuoCommunity.API
{
    /// <summary>
    /// 
    /// </summary>
    public class SpecialMethodModule : IHttpModule
    {
        /// <summary>
        /// 
        /// </summary>
        public SpecialMethodModule() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        public void Init(HttpApplication app)
        {
            app.BeginRequest += new EventHandler(this.BeginRequest);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="e"></param>
        public void BeginRequest(object resource, EventArgs e)
        {
            HttpApplication app = resource as HttpApplication;
            HttpContext context = app.Context;
            if (context.Request.HttpMethod.ToUpper() == "OPTIONS")
            {
                context.Response.StatusCode = 200;
                context.Response.End();
            }
        }
    }
}
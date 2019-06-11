using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GuoGuoCommunity.API.Common
{
    /// <summary>
    /// 
    /// </summary>
    [HubName("SignalRServerHub")]
    public class SignalRServerHub : Hub
    {
        /// <summary>
        /// 客户端列表
        /// </summary>
        public static IHubCallerConnectionContext<dynamic> ClientList;

        /// <summary>
        /// 连接id list
        /// </summary>
        public static Dictionary<string, string> ConnectionIds = new Dictionary<string, string>();

        /// <summary>
        /// 服务对象
        /// </summary>
        public static HubCallerContext ServerContext;

        /// <summary>
        /// 检查用户登录状态
        /// </summary>
        /// <param name="shareID">用户进场值</param>
        /// <returns></returns>
        private static int checklogininfo(string shareID)
        {
            if (ServerContext == null)
            {
                return 0;
            }
            else
            {
                //如果已有
                if (ConnectionIds.ContainsKey(shareID))
                {
                    ConnectionIds[shareID] = ServerContext.ConnectionId;
                }
                else
                {
                    ConnectionIds.Add(shareID, ServerContext.ConnectionId);
                }
                return 1;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public class Model
        {
            /// <summary>
            /// 1:物业 2:商超
            /// </summary>
            public string source { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public string companyID { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public string employeeid { get; set; }
        }

        /// <summary>
        /// 登录必调用 方法  用于存userlist 
        /// </summary>
        public void sendloginmsg(Model model)
        {
            //调用所有客户端的sendMessage方法  
            //Clients.All.sendMessage(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), message);
            var obj = ConnectionIds.FirstOrDefault(a => chkemployeeid(a.Key, model.companyID, model.employeeid));
            if (!string.IsNullOrEmpty(obj.Key))
            {
                //存在连接发送一个断开方法
                ClientList.Client(obj.Value).sendfocusoffline();
            }
            //检查登录信息（需传参数）
            checklogininfo(model.source + "@" + model.companyID + "@" + model.employeeid);

            //修改数据库当前用户登录状态TODO：

            Dictionary<string, string> resultlist = ConnectionIds.Where(a => chkcompanyID(a.Key, model.companyID)).ToDictionary(b => b.Key, c => c.Value);
            if (resultlist.Count > 0)
            {
                foreach (var item in resultlist)
                {
                    ClientList.Client(item.Value).customerloginstatemessage(new { employeeid = model.employeeid, isonline = true });
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public static bool chkcompanyID(string key, string companyID)
        {
            bool r = false;
            if (key.Contains('@') && key.Split('@').Length == 3)
            {
                string tmpstr = key.Split('@')[1];

                if (tmpstr == companyID)
                {
                    r = true;
                }
            }
            return r;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="companyID"></param>
        /// <param name="pemployeeid"></param>
        /// <returns></returns>
        public static bool chkemployeeid(string key, string companyID, string pemployeeid)
        {
            bool r = false;
            if (key.Contains('@') && key.Split('@').Length == 3)
            {
                string tmppemployeeid = key.Split('@').Last();
                string tmpcompanyID = key.Split('@')[1];
                string tmpsource = key.Split('@').First();
                if (tmppemployeeid == pemployeeid.ToString())
                {
                    r = true;
                }
            }
            return r;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override Task OnConnected()
        {
            ClientList = Clients;
            ServerContext = Context;
            return base.OnConnected();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stopCalled"></param>
        /// <returns></returns>
        public override Task OnDisconnected(bool stopCalled)
        {
            ClientList = Clients;
            ServerContext = Context;
            if (ConnectionIds.Count > 0)
            {
                KeyValuePair<string, string> tmpconnect = ConnectionIds.FirstOrDefault(a => a.Value == Context.ConnectionId);
                if (tmpconnect.Key != null)
                {
                    int employeeid = Convert.ToInt32(tmpconnect.Key.Split('@').Last());
                    string companyID = tmpconnect.Key.Split('@').First();

                    //离线调用数据库更新用户登录状态TODO：
                    //

                    ConnectionIds.Remove(tmpconnect.Key);//删除连接 list 中 当前退出的客户端


                    Dictionary<string, string> resultlist = ConnectionIds.Where(a => chkcompanyID(a.Key, companyID)).ToDictionary(b => b.Key, c => c.Value);
                    if (resultlist.Count > 0)
                    {
                        foreach (var item in resultlist)
                        {
                            ClientList.Client(item.Value).customerloginstatemessage(new { companyID = companyID, employeeid = employeeid, isonline = false });
                        }
                    }
                }
            }
            return base.OnDisconnected(true);

        }
    }
}
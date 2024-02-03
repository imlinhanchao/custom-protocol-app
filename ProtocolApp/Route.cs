using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;

namespace ProtocolApp
{
    internal class RouteMeta
    {
        /// <summary>
        /// 路由名称
        /// </summary>
        public string name;
        /// <summary>
        /// 路由路径，支持 :key 匹配 Param
        /// </summary>
        public string path;
        /// <summary>
        /// 路由元信息
        /// </summary>
        public Dictionary<string, object> meta;

        public RouteMeta(string name, string path, Dictionary<string, object> meta)
        {
            this.name = name;
            this.path = path;
            this.meta = meta;
        }
    }
    internal class Route
    {
        public string name;
        public string fullPath;
        public Dictionary<string, string> query;
        public Dictionary<string, string> param;
        public Dictionary<string, object> meta;

        public Route()
        {
            query = new Dictionary<string, string>();
            param = new Dictionary<string, string>();
        }

        public static Route Format(string url, RouteMeta[] routes)
        {
            Route route = new Route();
            var urls = url.Substring(url.IndexOf("://") + 2).Split('?');
            route.fullPath = new Regex(@"/+$").Replace(urls[0], "");
            var paramRegex = new Regex(@"/:\w+");

            for (int i = 0; i < routes.Length; i++)
            {
                RouteMeta routeMeta = routes[i];

                // 全匹配路由
                if (routeMeta.path == route.fullPath)
                {
                    route.name = routeMeta.name;
                    route.meta = routeMeta.meta;
                    break;
                }

                // 检查路由是否包含参数
                if (!paramRegex.IsMatch(routeMeta.path)) continue;

                // 检查地址是否匹配参数路由
                var pathRegex = new Regex("^" + paramRegex.Replace(routeMeta.path, "/([^/]*?)") + "/*$");
                if (!pathRegex.IsMatch(route.fullPath)) continue;

                // 匹配路由参数
                var matchs = pathRegex.Matches(route.fullPath);
                var paramKeys = paramRegex.Matches(routeMeta.path);

                for (var j = 1; j < matchs[0].Groups.Count; j++)
                {
                    route.param.Add(paramKeys[j - 1].Groups[0].Value.Replace("/:", ""), matchs[0].Groups[j].Value);
                }

                route.name = routeMeta.name;
                route.meta = routeMeta.meta;
            }

            // 解析 QueryString
            if (urls.Length > 1)
            {
                url = urls[1];
                var querys = url.Split('&');
                for (var j = 0; j < querys.Length; j++)
                {
                    var query = querys[j].Split('=');
                    route.query.Add(query[0], HttpUtility.UrlEncode(query.Length > 1 ? query[1] : ""));
                }
            }

            return route;
        }
    }
}

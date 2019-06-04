using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebApplication1
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "displayLocation",
                url: "display/{ip}/{port}",
                defaults: new { controller = "Main", action = "CheckURL" }
                );

            routes.MapRoute(
                name: "displayAnimation",
                url: "display/{ip}/{port}/{freq}",
                defaults: new { controller = "Main", action = "DisplayAnimation" }
                );

            //routes.MapRoute(
            //    name: "displayGetPoint",
            //    url: "display/getpoint",
            //    defaults: new { controller = "Main", action = "GetPoint" }
            //    );

            routes.MapRoute(
                name: "save",
                url: "display/{ip}/{port}/{freq}/{sec}/{fileName}",
                defaults: new { controller = "Main", action = "save" }
                );

            routes.MapRoute(
                name: "displayPath",
                url: "display/{fileName}/{freq}",
                defaults: new { controller = "Main", action = "CheckURL" }
                );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Main", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}

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
                defaults: new { controller = "Main", action = "DisplayLocation" },
                                new { ip = @"^(?:[0-9]{1,3}\.){3}[0-9]{1,3}$" }

                );

            routes.MapRoute(
                name: "displayAnimation",
                url: "display/{ip}/{port}/{freq}",
                defaults: new { controller = "Main", action = "DisplayAnimation" }
                );

            //routes.MapRoute(
            //    name:"saveData",
            //    url: "save/{ip}/{port}/{freq}/{sec}/{fileName}",
            //    defaults: new { controller = "Main", action = "SaveData" }
            //    );

            routes.MapRoute(
                name: "displayPath",
                url: "display/{fileName}/{freq}",
                defaults: new { controller = "Main", action = "DisplayPath" }
                );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Main", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "sava",
                url:"save/{ip}/{port}/{freq}/{sec}/{fileName}",
                defaults: new { controller = "Main", action = "Save" }
            );
        }
    }
}

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
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "displayLocation",
                url: "display/{ip}/{port}",
                defaults: new { controller = "MainController", action = "displayLocation" }
                );

            routes.MapRoute(
                name: "displayAnimation",
                url: "display/{ip}/{port}/{freq}",
                defaults: new { controller = "MainController", action = "displayAnimation" }
                );

            routes.MapRoute(
                name: "save",
                url: "display/{ip}/{port}/{freq}/{sec}/{fileName}",
                defaults: new { controller = "MainController", action = "save" }
                );

            routes.MapRoute(
                name: "displayPath",
                url: "display/{fileName}/{freq}",
                defaults: new { controller = "MainController", action = "displayPath" }
                );
        }
    }
}

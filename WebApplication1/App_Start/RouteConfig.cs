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

            /* 
             * In this routes we get ip and port,
             * and show the one point on the map
             **/
            routes.MapRoute(
                name: "displayLocation",
                url: "display/{ip}/{port}",
                defaults: new { controller = "Main", action = "DisplayLocation" },
                                new { ip = @"^(?:[0-9]{1,3}\.){3}[0-9]{1,3}$" }
                );

            /*
             * In this routes we get ip, port and freq,
             * and show the route of the plane,
             * according the freq we sample the simulator
             **/
            routes.MapRoute(
                name: "displayAnimation",
                url: "display/{ip}/{port}/{freq}",
                defaults: new { controller = "Main", action = "DisplayAnimation" }
                );
            /*
             * In this routes we get file name and freq'
             * and show the route of the plane 
             * according the points in the file.
             * the freq is the time to simple point from the file.
             **/
            routes.MapRoute(
                name: "displayPath",
                url: "display/{fileName}/{freq}",
                defaults: new { controller = "Main", action = "DisplayPath" }
                );

            /*
             * This is the defaults routes.
             **/ 
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Main", action = "Index", id = UrlParameter.Optional }
            );

            /*
             * In this routes we get ip,port,sec and file name,
             * and show the route of the plane and write the loction point to file.
             **/
            routes.MapRoute(
                name: "sava",
                url:"save/{ip}/{port}/{freq}/{sec}/{fileName}",
                defaults: new { controller = "Main", action = "Save" }
            );
        }
    }
}

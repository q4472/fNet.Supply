using System.Web.Mvc;
using System.Web.Routing;

namespace FNet
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: null,
                url: "supply/f0/getpricedetail/{*pathInfo}",
                defaults: new { controller = "F0", action = "GetPriceDetail" });

            routes.MapRoute(
                name: null,
                url: "supply/f0/getorderdetail/{*pathInfo}",
                defaults: new { controller = "F0", action = "GetOrderDetail" });

            routes.MapRoute(
                name: null,
                url: "supply/f0/applyfilter/{*pathInfo}",
                defaults: new { controller = "F0", action = "ApplyFilter" });

            routes.MapRoute(
                name: null,
                url: "supply/f0/{*pathInfo}",
                defaults: new { controller = "F0", action = "Index" });

            routes.MapRoute(
                name: null,
                url: "{*pathInfo}",
                defaults: new { controller = "Home", action = "Index" });
        }
    }
}

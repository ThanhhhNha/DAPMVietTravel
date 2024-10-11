using System.Web.Mvc;
using System.Web.Routing;

public class RouteConfig
{
    public static void RegisterRoutes(RouteCollection routes)
    {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Account",
                url: "Account/{action}/{id}",
                defaults: new { controller = "Account", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "VietTravel.Controllers" }
            );

            routes.MapRoute(
                name: "AdminCaNhan",
                url: "Admin/Account/AdminCaNhan/{id}",
                defaults: new { controller = "Account", action = "AdminCaNhan", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "ListHotels",
                url: "ListHotels/ListHotels={location}",
                defaults: new { controller = "ListHotels", action = "ListHotels" }
            );

    }
}


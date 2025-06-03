using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace RestaurantSystem_2_
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapMvcAttributeRoutes();

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            ).RouteHandler = new UnicodeRouteHandler(); ;


        }
    }
    public class UnicodeRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new UnicodeMvcHandler(requestContext);
        }
    }

    public class UnicodeMvcHandler : MvcHandler
    {
        public UnicodeMvcHandler(RequestContext requestContext) : base(requestContext)
        {
        }

        protected override void ProcessRequest(HttpContextBase httpContext)
        {
            // Unicode karakterleri destekleyen işlemler
            base.ProcessRequest(httpContext);
        }
    }



}

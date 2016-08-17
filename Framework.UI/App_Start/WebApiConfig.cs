using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;

namespace Framework.UI
{
  public static class WebApiConfig
  {
    public static void Register(HttpConfiguration config)
    {
      // Web API configuration and services
      // Configure Web API to use only bearer token authentication.
      config.SuppressDefaultHostAuthentication();
      config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

      // Web API routes
      config.MapHttpAttributeRoutes();

      config.Routes.MapHttpRoute(
                      name: "Api-1",
                      routeTemplate: "api/v1/Product/{action}/{id}",
                      defaults: new { controller = "ProductV1", id = RouteParameter.Optional }
                  );

      config.Routes.MapHttpRoute(
                        name: "Api-2",
                        routeTemplate: "api/v2/Product/{action}/{id}",
                        defaults: new { controller = "ProductV2", id = RouteParameter.Optional }
                    );

      //config.Routes.MapHttpRoute(
      //          name: "DefaultApi",
      //          routeTemplate: "api/{controller}/{id}",
      //          defaults: new { id = RouteParameter.Optional }
      //      );
    }
  }
}

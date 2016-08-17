using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Hosting;
using System.Web.Http.Routing;

namespace Framework.UI.Test.Base
{
  public static class FakeHttp
  {
    public static HttpControllerContext MultipartForm(string fileName)
    {
      var dir = Directory.GetCurrentDirectory() + "\\Photos\\" + fileName;

      var request = new HttpRequestMessage(HttpMethod.Post, "");
      var content = new MultipartFormDataContent();
      const int lengthStream = 1900;
      var contentBytes = new ByteArrayContent(File.ReadAllBytes(dir));

      contentBytes.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
      {
        FileName = fileName
      };
      contentBytes.Headers.Add("Content-Type", "image/jpeg");
      contentBytes.Headers.Add("Content-Length", lengthStream.ToString());
      content.Add(contentBytes);
      request.Content = content;

      return new HttpControllerContext(new HttpConfiguration(), new HttpRouteData(new HttpRoute("")), request);
    }

    public static HttpRequestMessage HttpRequestMessage()
    {
      var config = new HttpConfiguration();

      config.Routes.MapHttpRoute(
          name: "Default",
          routeTemplate: "api/{controller}/{id}",
          defaults: new { id = RouteParameter.Optional });

      var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost") { };
      request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;
      request.Properties[HttpPropertyKeys.HttpRouteDataKey] = new HttpRouteData(new HttpRoute());

      return request;
    }
  }

}

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Framework.Common.Extensions;
using System.Web.Http;
using Framework.UI.Core;
namespace Framework.UI.App_Start
{
  public class Bootstrapper
  {
    public static void RegisterMef()
    {
      AggregateCatalog catalog = Framework.Service.Bootstrap.MEFLoader.ServiceCatalog();
      catalog.Catalogs.Add(new AssemblyCatalog(Assembly.GetExecutingAssembly()));
      var container = new CompositionContainer(catalog);
      var resolver = new MefDependencyResolver(container);
      DependencyResolver.SetResolver(resolver);
      System.Web.Http.GlobalConfiguration.Configuration.DependencyResolver = resolver;
    }
  }
}
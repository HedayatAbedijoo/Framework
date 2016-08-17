using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Web.Mvc;
using Framework.Common.Extensions;
using System.Web.Http.Dependencies;
using System.ComponentModel.Composition;

namespace Framework.UI.Core
{
  public class MefDependencyResolver : System.Web.Http.Dependencies.IDependencyResolver, System.Web.Mvc.IDependencyResolver
  {
    public MefDependencyResolver(CompositionContainer container)
    {
      _Container = container;
    }

    CompositionContainer _Container;

    public object GetService(Type serviceType)
    {
      return _Container.GetExportedValueByType(serviceType);
    }

    public IEnumerable<object> GetServices(Type serviceType)
    {
      return _Container.GetExportedValuesByType(serviceType);
    }

    public IDependencyScope BeginScope()
    {
      return this;
    }

    public void Dispose()
    {

    }
  }
}
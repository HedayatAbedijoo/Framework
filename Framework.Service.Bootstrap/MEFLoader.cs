using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Service.Bootstrap
{
  public class MEFLoader
  {
    public static CompositionContainer Init()
    {
      CompositionContainer container = new CompositionContainer(ServiceCatalog());

      return container;
    }

    public static AggregateCatalog ServiceCatalog()
    {
      AggregateCatalog catalog = Framework.Repository.Bootstrap.MEFLoader.RepositoryCatalog();
      catalog.Catalogs.Add(new AssemblyCatalog(typeof(LogService).Assembly));
      return catalog;
    }
  }
}

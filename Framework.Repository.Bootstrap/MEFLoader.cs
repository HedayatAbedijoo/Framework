using Framework.Context;
using Framework.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Repository.Bootstrap
{
  public class MEFLoader
  {
    public static CompositionContainer Init()
    {
      var catalog = RepositoryCatalog();
      CompositionContainer container = new CompositionContainer(catalog);

      return container;
    }

    public static AggregateCatalog RepositoryCatalog()
    {
      AggregateCatalog catalog = new AggregateCatalog();
      catalog.Catalogs.Add(new AssemblyCatalog(typeof(UnitOfWork).Assembly));
      catalog.Catalogs.Add(new AssemblyCatalog(typeof(DbContextFactory).Assembly));
      catalog.Catalogs.Add(new AssemblyCatalog(typeof(LogRepository).Assembly)); 
      return catalog;
    }

  }
}

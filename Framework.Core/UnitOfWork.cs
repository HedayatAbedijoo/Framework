using Framework.Core.Contracts.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Core
{
  [Export(typeof(IUnitOfWork))]
  [PartCreationPolicy(CreationPolicy.NonShared)] // review and test this one
  public class UnitOfWork : IUnitOfWork, IDisposable
  {
    DbContext context;

    [ImportingConstructor]
    public UnitOfWork(IDbContextFactory contextFactory)
    {
      context = contextFactory.GetDbContext() as DbContext;
    }
    public System.Data.Entity.DbContext Context
    {
      get { return context; }
    }

    public void Dispose()
    {
      context.Dispose();
    }

  }
}

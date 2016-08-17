using Framework.Core.Contracts.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Context
{
  [Export(typeof(IDbContextFactory))]
  [PartCreationPolicy(CreationPolicy.Shared)] // review and test this one
  public class DbContextFactory : IDbContextFactory
  {
    public object GetDbContext()
    {
      return new FrameworkContext("FrameworkDBConnectionString");
    }

  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Core.Contracts.Repository
{
  public interface IDbContextFactory
  {
    object GetDbContext();
  }
}

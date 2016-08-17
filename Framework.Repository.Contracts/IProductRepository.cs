using Framework.Core.Contracts.Repository;
using Framework.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Repository.Contracts
{
  public interface IProductRepository : IRepositoryBase<Product>
  {
  }
}

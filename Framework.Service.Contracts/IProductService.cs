using Framework.Core.Contracts.Service;
using Framework.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Service.Contracts
{
  public interface IProductService : IServiceBase<Product>
  {
    Task<ServiceContract> DeleteById(string Id);
  }
}

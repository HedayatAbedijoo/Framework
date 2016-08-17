using SiutSupply.Repository;
using Framework.Core.Contracts.Repository;
using Framework.DomainModel;
using Framework.Repository.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Repository
{
  [Export(typeof(IProductRepository))]
  [PartCreationPolicy(CreationPolicy.NonShared)]
  public class ProductRepository : RepositoryBase<Product>, IProductRepository
  {
    [ImportingConstructor]
    public ProductRepository(IUnitOfWork unitOfWork, ILogRepository log)
            : base(unitOfWork, log)
    { }
  }
}

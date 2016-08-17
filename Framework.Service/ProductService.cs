using Framework.Core.Contracts.Service;
using Framework.DomainModel;
using Framework.Repository.Contracts;
using Framework.Service.Base;
using Framework.Service.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Service
{
  [Export(typeof(IProductService))]
  [PartCreationPolicy(CreationPolicy.NonShared)]
  public class ProductService : ServiceBase<Product>, IProductService
  {
    IProductRepository repository;
    [ImportingConstructor]
    public ProductService(IProductRepository repository)
            : base(repository)
    {
      this.repository = repository;
    }

    public override ServiceContract Create(Product entity)
    {
      entity.LastUpdated = DateTime.Now;
      return base.Create(entity);
    }

    public override ServiceContract Update(Product entity)
    {
      entity.LastUpdated = DateTime.Now;      
      return base.Update(entity);
    }
  }
}

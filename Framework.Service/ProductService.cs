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

    public async Task<ServiceContract> DeleteById(string Id)
    {
      var contract = new ServiceContract();
      Guid guId;
      var validation = Guid.TryParse(Id, out guId);
      if (!validation)
      {
        contract.Result = OperationResult.Error;
        contract.Message = "Id is not valid";
      }
      else
      {
        var currentItem = repository.GetSingle(i => i.Id == guId);
        if (currentItem == null)
        {
          contract.Result = OperationResult.Error;
          contract.Message = "Item Not Found";
          return contract;
        }
        repository.Delete(currentItem);

        try
        {
          await repository.SaveChangesAsync();
          contract.Result = OperationResult.Success;
        }
        catch (Exception exp)
        {

          contract.Result = OperationResult.Error;
          contract.Message = exp.Message;
        }


      }

      return contract;
    }

    public override ServiceContract Update(Product entity)
    {
      entity.LastUpdated = DateTime.Now;
      return base.Update(entity);
    }

    public async Task<ServiceContract> UpdateProduct(Guid Id, string Name, decimal Price, string Photo, string rowVersion)
    {
      var contract = new ServiceContract();


      var item = await repository.GetSingleAsync(i => i.Id == Id);
      if (item == null)
      {
        contract.Result = OperationResult.Error;
      }
      else
      {
        // Do Business rules
        if (Convert.ToBase64String(item.RowVersion) != rowVersion)
        {
          contract.Result = OperationResult.Error;
        }
        else
        {
          item.Name = Name;
          item.Price = Price;
          item.Photo = Photo;
          item.LastUpdated = DateTime.Now;
          repository.Update(item);
          try
          {
            await repository.SaveChangesAsync();
            contract.Result = OperationResult.Success;
          }
          catch (Exception exp)
          {
            contract.Result = OperationResult.Error;
            contract.Message = exp.Message;
          }
        }
      }


      return contract;
    }

  }
}

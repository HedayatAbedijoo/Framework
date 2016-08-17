using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Framework.Service.Contracts;
using Framework.Service.IntegrationTest.Base;
using Framework.DomainModel;
using System.Linq;
namespace Framework.Service.IntegrationTest
{
  [TestClass]
  public class ProductServiceTest : ServiceTestBase
  {
    IProductService productService;

    [TestInitialize]
    public void Initialize()
    {
      productService = this.Container.GetExportedValue<IProductService>();
    }

    [TestMethod]
    public void ProductService_Integration_ShouldCreateItem()
    {
      #region Setup
      var newId = Guid.NewGuid();
      var newProduct = CreateProduct(newId, Guid.NewGuid().ToString("N") + "PNG", "New Product", 120);
      #endregion

      #region Action
      var contractCreate = productService.Create(newProduct);
      var contractSave = productService.SaveChanges();
      var currentItem = productService.GetSingle(i => i.Id == newId);
      #endregion

      #region Assert
      Assert.AreEqual(contractCreate.Result, Core.Contracts.Service.OperationResult.Success);
      Assert.AreEqual(contractSave.Result, Core.Contracts.Service.OperationResult.Success);
      Assert.AreEqual(currentItem.Item, newProduct);
      #endregion
    }

    [TestMethod]
    public void ProductService_Integration_ShouldUpdateItem()
    {
      #region Setup
      var newId = Guid.NewGuid();
      var updatePhotoId = Guid.NewGuid().ToString("N") + "PNG";
      var updateName = "Hedayat Abedijoo";
      var newProduct = CreateProduct(newId, Guid.NewGuid().ToString("N") + "PNG", "New Product", 120);
      #endregion

      #region Action
      productService.Create(newProduct);
      var contractSave = productService.SaveChanges();

      var currentItem = productService.GetSingle(i => i.Id == newId);
      currentItem.Item.Name = updateName;
      currentItem.Item.Photo = updatePhotoId;
      productService.Update(currentItem.Item);
      var contractUpdate = productService.SaveChanges();
      #endregion

      #region Assert      
      Assert.AreEqual(contractSave.Result, Core.Contracts.Service.OperationResult.Success);
      Assert.AreEqual(currentItem.Item.Name, updateName);
      Assert.AreEqual(currentItem.Item.Photo, updatePhotoId);

      #endregion
    }

    [TestMethod]
    public void ProductService_Integration_ShouldDeleteItem()
    {
      #region Setup
      var newId = Guid.NewGuid();
      var newProduct = CreateProduct(newId, Guid.NewGuid().ToString("N") + "PNG", "New Product", 120);
      #endregion

      #region Action
      productService.Create(newProduct);
      var contractSave = productService.SaveChanges();
      var currentItem = productService.GetSingle(i => i.Id == newId);
      productService.Delete(currentItem.Item);
      var contractDelete = productService.SaveChanges();
      currentItem = productService.GetSingle(i => i.Id == newId);
      #endregion

      #region Assert      
      Assert.AreEqual(contractSave.Result, Core.Contracts.Service.OperationResult.Success);
      Assert.AreEqual(contractDelete.Result, Core.Contracts.Service.OperationResult.Success);
      Assert.AreEqual(currentItem.Item, null);

      #endregion
    }

    [TestMethod]
    public void ProductService_Integration_ShouldFailedInCreateItem()
    {
      #region Setup
      var newId = Guid.NewGuid();
      var newProduct = CreateProduct(newId, Guid.NewGuid().ToString("N") + "PNG", null, 120);
      #endregion

      #region Action
      var contractCreate = productService.Create(newProduct);
      var contractSave = productService.SaveChanges();
      #endregion

      #region Assert
      Assert.AreEqual(contractCreate.Result, Core.Contracts.Service.OperationResult.Success);
      Assert.AreEqual(contractSave.Result, Core.Contracts.Service.OperationResult.Error);
      #endregion
    }

    [TestMethod]
    public void ProductService_Integration_ShouldReturnItems()
    {
      #region Setup
      var name = Guid.NewGuid().ToString("N");
      var item1 = CreateProduct(Guid.NewGuid(), Guid.NewGuid().ToString("N") + "PNG", name, 1520);
      var item2 = CreateProduct(Guid.NewGuid(), Guid.NewGuid().ToString("N") + "PNG", name, 1520);
      var item3 = CreateProduct(Guid.NewGuid(), Guid.NewGuid().ToString("N") + "PNG", name, 1520);
      var item4 = CreateProduct(Guid.NewGuid(), Guid.NewGuid().ToString("N") + "PNG", name, 1520);

      #endregion

      #region Action
      productService.Create(item1);
      productService.Create(item2);
      productService.Create(item3);
      productService.Create(item4);

      productService.SaveChanges();

      var count = productService.Count(i => i.Name == name);
      var list = productService.GetAll(i => i.Name == name);
      #endregion

      #region Assert
      Assert.AreEqual(count.Item, 4);
      Assert.AreEqual(list.Item.Count(), 4);
      #endregion
    }

    [TestMethod]
    public void ProductService_Integration_ShouldNotSave_OnConcurrentAction()
    {
      #region Setup
      var newItem = CreateProduct();
      productService.Create(newItem);
      productService.SaveChanges();

      var item1 = productService.GetSingle(i => i.Id == newItem.Id).Item;
      var productService2 = this.Container.GetExportedValue<IProductService>();

      var item2 = productService2.GetSingle(i => i.Id == newItem.Id).Item;

      item1.Name = "First Attempt";

      productService.Update(item1);
      #endregion

      #region Action
      var contract1 = productService.SaveChanges();

      #endregion

      #region Assert
      Assert.AreEqual(contract1.Result, Core.Contracts.Service.OperationResult.Success);
      item2.Name = "Second Attemp";
      productService2.Update(item2);
      var contract2 = productService2.SaveChanges();

      Assert.AreEqual(contract2.Result, Core.Contracts.Service.OperationResult.Error);
      #endregion




    }

   
    #region Helpers
    private Product CreateProduct()
    {
      return CreateProduct(Guid.NewGuid(), "PhotoId", "Test Service Integration", 10);
    }
    private Product CreateProduct(Guid Id, string PhotoId, string Name, decimal price)
    {
      var item = new Product();
      item.Id = Id;
      item.Name = Name;
      item.Photo = PhotoId;
      item.Price = price;
      return item;
    }
    #endregion
  }
}

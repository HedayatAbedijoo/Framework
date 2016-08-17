using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Framework.Repository.Contracts;
using Framework.Service.Contracts;
using Framework.Core.Contracts.Repository;
using Framework.DomainModel;

namespace Framework.Service.UnitTest
{
  [TestClass]
  public class ProductServiceTest
  {
    Mock<IProductRepository> mockRepository;

    [TestInitialize]
    public void Initialize()
    {
      mockRepository = new Mock<IProductRepository>(); ;

    }

    [TestMethod]
    public void ProductServiceTest_Unit_ShouldFailedInCreateItem()
    {
      #region Setup
      var newId = Guid.NewGuid();
      var newProduct = CreateProduct(newId, Guid.NewGuid().ToString("N") + "PNG", "Test Name", 120);
      mockRepository.Setup(m => m.Add(newProduct));
      mockRepository.Setup(m => m.SaveChanges()).Throws(new Exception());      
      var service = new ProductService(mockRepository.Object);
      #endregion

      #region Action
      var createContract = service.Create(newProduct);
      var saveContract = service.SaveChanges();      
      #endregion

      #region Assert
      Assert.AreEqual(createContract.Result, Core.Contracts.Service.OperationResult.Success);
      Assert.AreEqual(saveContract.Result, Core.Contracts.Service.OperationResult.Error);      
      #endregion

    }

    [TestMethod]
    public void ProductServiceTest_Unit_ShouldCreateItem()
    {
      #region Setup
      var newId = Guid.NewGuid();
      var newProduct = CreateProduct(newId, Guid.NewGuid().ToString("N") + "PNG", "Test Name", 120);
      mockRepository.Setup(m => m.Add(newProduct));
      mockRepository.Setup(m => m.SaveChanges()).Returns(1);
      mockRepository.Setup(m => m.GetSingle(i => i.Id == newId)).Returns(newProduct);
      var service = new ProductService(mockRepository.Object);

      #endregion

      #region Action
      var createContract = service.Create(newProduct);
      var saveContract = service.SaveChanges();
      var currentItem = service.GetSingle(i => i.Id == newId);
      #endregion

      #region Assert
      Assert.AreEqual(createContract.Result, Core.Contracts.Service.OperationResult.Success);
      Assert.AreEqual(saveContract.Result, Core.Contracts.Service.OperationResult.Success);
      Assert.AreEqual(currentItem.Item, newProduct);
      #endregion

    }

    [TestMethod]
    public void ProductServiceTest_Unit_ShouldUpdateItem()
    {
      #region Setup
      var newId = Guid.NewGuid();
      var newProduct = CreateProduct(newId, Guid.NewGuid().ToString("N") + "PNG", "Test Name", 120);
      mockRepository.Setup(m => m.Update(newProduct));
      mockRepository.Setup(m => m.SaveChanges()).Returns(1);
      mockRepository.Setup(m => m.GetSingle(i => i.Id == newId)).Returns(newProduct);
      var service = new ProductService(mockRepository.Object);
      #endregion

      #region Action
      var createContract = service.Update(newProduct);
      var saveContract = service.SaveChanges();
      var currentItem = service.GetSingle(i => i.Id == newId);
      #endregion

      #region Assert
      Assert.AreEqual(createContract.Result, Core.Contracts.Service.OperationResult.Success);
      Assert.AreEqual(saveContract.Result, Core.Contracts.Service.OperationResult.Success);
      Assert.AreEqual(currentItem.Item, newProduct);
      #endregion
    }

    [TestMethod]
    public void ProductServiceTest_Unit_ShouldDeleteItem()
    {
      #region Setup
      var newId = Guid.NewGuid();
      var newProduct = CreateProduct(newId, Guid.NewGuid().ToString("N") + "PNG", "Test Name", 120);
      mockRepository.Setup(m => m.Delete(newProduct));
      mockRepository.Setup(m => m.SaveChanges()).Returns(1);
      mockRepository.Setup(m => m.GetSingle(i => i.Id == newId)).Returns((Product)null);
      var service = new ProductService(mockRepository.Object);
      #endregion

      #region Action
      var createContract = service.Update(newProduct);
      var saveContract = service.SaveChanges();
      var currentItem = service.GetSingle(i => i.Id == newId);
      #endregion

      #region Assert
      Assert.AreEqual(createContract.Result, Core.Contracts.Service.OperationResult.Success);
      Assert.AreEqual(saveContract.Result, Core.Contracts.Service.OperationResult.Success);
      Assert.AreEqual(currentItem.Item, null);
      #endregion

    }

    #region Helpers

    private Product CreateProduct(Guid Id, string PhotoId, string Name, decimal price)
    {
      var item = new Product();
      item.Id = Id;
      item.Name = Name;
      item.Photo = PhotoId;
      item.Price = price;
      item.LastUpdated = DateTime.Now;

      return item;
    }
    #endregion
  }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Framework.Core.Contracts.Repository;
using Framework.DomainModel;
using Framework.Repository.Contracts;
using Framework.Repository.Test.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Repository.Test
{
  [TestClass]
  public class ProductRepositoryTest : RepositoryTestBase
  {

    IProductRepository repository;
    [TestInitialize]
    public void Initialize()
    {
      repository = this.Container.GetExportedValue<IProductRepository>();
    }


    [TestMethod]
    [ExpectedException(typeof(Exception))]
    public void ProductRepository_ShouldTrhowException_OnNonValidProduct()
    {
      #region Setup
      var newItemdId = Guid.NewGuid();
      var item = CreateProduct(newItemdId, Guid.NewGuid().ToString("N") + "PNG", null, 1);
      #endregion

      #region Action
      repository.Add(item);
      repository.SaveChanges();
      #endregion

      #region Assert
      Assert.Fail();
      #endregion

    }


    [TestMethod]
    public void ProductRepository_ShouldCreateItem()
    {
      #region Setup
      var newItemdId = Guid.NewGuid();
      var item = CreateProduct(newItemdId, Guid.NewGuid().ToString("N") + "PNG", "New Prodcut", 1520);
      #endregion

      #region Action
      repository.Add(item);
      repository.SaveChanges();
      #endregion

      #region Assert
      var dbItem = repository.GetSingle(i => i.Id == newItemdId);
      Assert.AreEqual(item, dbItem);
      #endregion

    }

    [TestMethod]
    public void ProductRepository_ShouldDeleteItem()
    {

      #region Setup
      var newItemdId = Guid.NewGuid();
      var item = CreateProduct(newItemdId, Guid.NewGuid().ToString("N") + "PNG", "New Prodcut", 1520);
      repository.Add(item);
      repository.SaveChanges();
      #endregion

      #region Action
      var existItem = repository.GetSingle(i => i.Id == item.Id);
      repository.Delete(existItem);
      repository.SaveChanges();
      #endregion


      #region Assert
      var count = repository.Count(i => i.Id == newItemdId);
      Assert.AreEqual(count, 0);

      #endregion
    }

    [TestMethod]
    public void ProductRepository_ShouldUpdateItem()
    {
      #region Setup
      var newName = "Hedayat Abedijoo";
      var newPhotoId = Guid.NewGuid().ToString("N") + "PNG";

      var newItemdId = Guid.NewGuid();
      var item = CreateProduct(newItemdId, Guid.NewGuid().ToString("N") + "PNG", "New Prodcut", 1520);
      repository.Add(item);
      repository.SaveChanges();
      #endregion

      #region Action

      var currentItem = repository.GetSingle(i => i.Id == newItemdId);
      currentItem.Name = newName;
      currentItem.Photo = newPhotoId;
      repository.Update(currentItem);
      repository.SaveChanges();
      #endregion

      #region Assert
      var updatedItem = repository.GetSingle(i => i.Id == newItemdId);
      Assert.AreEqual(updatedItem.Name, newName);
      Assert.AreEqual(updatedItem.Photo, newPhotoId);
      #endregion
    }

    [TestMethod]
    public void ProductRepository_ShouldReturnItems()
    {
      #region Setup
      var name = Guid.NewGuid().ToString("N");
      var item1 = CreateProduct(Guid.NewGuid(), Guid.NewGuid().ToString("N") + "PNG", name, 1520);
      var item2 = CreateProduct(Guid.NewGuid(), Guid.NewGuid().ToString("N") + "PNG", name, 1520);
      var item3 = CreateProduct(Guid.NewGuid(), Guid.NewGuid().ToString("N") + "PNG", name, 1520);
      var item4 = CreateProduct(Guid.NewGuid(), Guid.NewGuid().ToString("N") + "PNG", name, 1520);

      #endregion

      #region Action
      repository.Add(item1);
      repository.Add(item2);
      repository.Add(item3);
      repository.Add(item4);

      repository.SaveChanges();

      var count = repository.Count(i => i.Name == name);
      var list = repository.GetAll(i => i.Name == name);
      #endregion

      #region Assert
      Assert.AreEqual(count, 4);
      Assert.AreEqual(list.Count(), 4);
      #endregion
    }
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

    [TestCleanup]
    public void FinalizeTest()
    {
      repository.Dispose();
    }

  }
}

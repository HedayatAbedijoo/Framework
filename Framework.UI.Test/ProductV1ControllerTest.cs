using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Framework.Service.Contracts;
using Framework.UI.Test.Base;
using Framework.UI.Controllers;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Framework.ViewModel;
using System.Collections.Generic;
using System.Web.Http;
using Framework.DomainModel;
using System.Linq;
using System.Net.Http;
using System.IO;
using System.Web.Http.Controllers;
using Moq;
using System.Web.Http.Hosting;
using System.Web.Http.Routing;

namespace Framework.UI.Test
{
  [TestClass]
  public class ProductV1ControllerTest : UITestBase
  {

    IProductService productService;
    List<Guid> listIds = new List<Guid>();
    ProductV1Controller controller;
    [TestInitialize]
    public void Initialize()
    {
      productService = this.Container.GetExportedValue<IProductService>();
      var pathProvider = new ServerMapPathProviderTest();
      controller = new ProductV1Controller(productService, pathProvider);

      for (int i = 0; i < 5; i++)
      {
        listIds.Add(Guid.NewGuid());
      }
      CreateSomeProducts();
    }
    [TestMethod]
    public async Task ProductGet_ShouldReturnListOfProduct()
    {
      controller.Request = FakeHttp.HttpRequestMessage();
      var result = await controller.Get() as OkNegotiatedContentResult<List<ProductViewModel>>;
      Assert.IsNotNull(result);
      Assert.IsTrue(result.Content.Count >= 5, "The product list should greater or equal than five");
    }

    [TestMethod]
    public async Task ProductGet_ShouldReturnProduct()
    {
      controller.Request = FakeHttp.HttpRequestMessage();
      var result = await controller.Get(listIds.First().ToString()) as OkNegotiatedContentResult<ProductViewModel>;
      Assert.IsNotNull(result);
      Assert.IsTrue(result.Content.Id == listIds.First());
    }

    [TestMethod]
    public async Task ProductGet_ShouldNotReturnProduct()
    {
      var result = await controller.Get(Guid.NewGuid().ToString()) as NotFoundResult;
      Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task ProductDelete_ShouldReturnOk()
    {
      var result = await controller.Delete(listIds.First().ToString()) as OkResult;
      Assert.IsNotNull(result);
      var result2 = await controller.Get(listIds.First().ToString()) as NotFoundResult;
      Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task ProductDelete_ShouldReturnNotFound()
    {
      var result = await controller.Delete(Guid.NewGuid().ToString()) as NotFoundResult;
      Assert.IsNotNull(result);
    }


    [TestMethod]
    public async Task ProductPost_ShouldReturnOK()
    {
      var prodcutViewModle = new ProductViewModel()
      {
        Name = "Web Api Test - Create new product",
        Price = 125
      };
      controller.Request = FakeHttp.HttpRequestMessage();

      var result = await controller.Post(prodcutViewModle);

      Assert.IsNotNull(result);
    }

    [TestMethod]
    public async Task ProductUploadPhoto_ShouldReturnOK()
    {
      string fileName = "4.jpg";
      controller.ControllerContext = FakeHttp.MultipartForm(fileName); ;

      var result = await controller.UploadPhoto() as OkNegotiatedContentResult<string>;

      Assert.AreEqual(result.Content, fileName);
      Assert.IsTrue(System.IO.File.Exists(Directory.GetCurrentDirectory() + "\\Photos\\" + fileName));
    }

    [TestMethod]
    public async Task ProductPut_ShoutReturnBadRequest_OnValidation()
    {
      var item = new ProductViewModel();
      controller.Request = FakeHttp.HttpRequestMessage();
      var result = await controller.Put(item) as StatusCodeResult;
      Assert.AreEqual(result.StatusCode, System.Net.HttpStatusCode.BadRequest);
    }

    [TestCleanup]
    public void FinalizeTest()
    {
      this.controller.Dispose();
    }
    #region Helper
    private void CreateSomeProducts()
    {
      foreach (var item in listIds)
      {
        var p1 = new Product() { Id = item, Photo = item.ToString(), Name = "Controller Test", Price = 120 };
        this.productService.Create(p1);
      }

      this.productService.SaveChanges();
    }
    #endregion
  }
}

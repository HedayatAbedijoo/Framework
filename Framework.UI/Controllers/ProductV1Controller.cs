using Framework.Core.Contracts.Service;
using Framework.DomainModel;
using Framework.Service.Contracts;
using Framework.UI.Core;
using Framework.UI.Core.Swagger;
using Framework.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace Framework.UI.Controllers
{
  [Export]
  [PartCreationPolicy(CreationPolicy.NonShared)]
  [Description("Framework WebApi Version 1: Provides access to products.")]
  public class ProductV1Controller : ApiController
  {
    IProductService productService;
    IServerMapPathProvider pathProvider;
    [ImportingConstructor]
    public ProductV1Controller(IProductService productService, IServerMapPathProvider pathProvider)
    {
      this.productService = productService;
      this.pathProvider = pathProvider;
    }


    [Description("Returns the list of products.")]
    [RestResponse("The products.")]
    [HttpGet]
    public async Task<IHttpActionResult> Get()
    {
      var viewModel = new List<ProductViewModel>();
      var contract = await productService.GetAllAsync();

      if (contract.Result == OperationResult.Error)
      {
        return NotFound();
      }
      else
      {
        viewModel = Mapping.CreateViewModels(contract.Item, Url);
        return Ok(viewModel);
      }
    }

    [Description("Returns a specific product.")]
    [RestParameter("id", "The product identifier.")]
    [RestResponse("The product, if any.")]
    public async Task<IHttpActionResult> Get(string id)
    {
      var viewModel = new ProductViewModel();

      var contract = await productService.GetSingleAsync(i => i.Id == new Guid(id));

      if (contract.Result == OperationResult.Error)
      {
        return NotFound();
      }
      else
      {
        if (contract.Item == null) return NotFound();
        viewModel = Mapping.CreateViewModel(contract.Item, Url);
        return Ok(viewModel);
      }
    }

    [Description("Adds a new product.")]
    [RestParameter("value", "The new product.")]
    [RestResponse("The product.")]
    [ResponseType(typeof(void))]
    public async Task<IHttpActionResult> Post(ProductViewModel value)
    {
      Validate<ProductViewModel>(value);
      if (!ModelState.IsValid)
      {
        return StatusCode(HttpStatusCode.BadRequest);
      }
      var newProduct = Mapping.CreateProduct(value);
      productService.Create(newProduct);

      var saveContract = await Task<ServiceContract>.Factory.StartNew(() =>
      {
        return productService.SaveChanges();
      });

      if (saveContract.Result == OperationResult.Error)
        return StatusCode(HttpStatusCode.InternalServerError);
      else
        return Ok();
    }


    [Description("Updates a product.")]
    [RestParameter("value", "The updated product.")]
    [ResponseType(typeof(void))]
    public async Task<IHttpActionResult> Put(ProductViewModel value)
    {
      Validate<ProductViewModel>(value);
      if (!ModelState.IsValid) return StatusCode(HttpStatusCode.BadRequest);

      var item = await Task<Product>.Factory.StartNew(() =>
      {
        return productService.GetSingle(i => i.Id == value.Id).Item;
      });

      if (item == null) return StatusCode(HttpStatusCode.BadRequest);
      if (Convert.ToBase64String(item.RowVersion) != value.RowVersion)
        return StatusCode(HttpStatusCode.BadRequest);

      Mapping.UpdateProduct(item, value);
      productService.Update(item);

      var contract = await Task<ServiceContract>.Factory.StartNew(() =>
      {
        return productService.SaveChanges();
      });

      if (contract.Result == OperationResult.Success)
      {
        return Ok();
      }
      else
      {
        return StatusCode(HttpStatusCode.InternalServerError);
      }
    }

    [Description("Removes a product.")]
    [RestParameter("id", "The product identifier.")]
    public async Task<IHttpActionResult> Delete(string id)
    {
      var contract = await productService.DeleteById(id);

      if (contract.Result == OperationResult.Success)
        return Ok();
      else
        return NotFound();
    }


    [Description("Upload the photo of product.")]
    [ResponseType(typeof(string))]
    public async Task<IHttpActionResult> UploadPhoto()
    {

      // If there is no Attachment, it means the photo has not been changed
      if (Request.Content.IsMimeMultipartContent())
      {
        var provider = new CustomUploadMultipartFormProvider(pathProvider.MapPath("~/UploadPhotos"));
        var rr = await Request.Content.ReadAsMultipartAsync(provider);

        var photo = provider.FileData.FirstOrDefault();
        var fileInfo = new FileInfo(photo.LocalFileName);

        return Ok<string>(fileInfo.Name);
      }
      else
      {
        return StatusCode(HttpStatusCode.NotFound);
      }
    }
  }
}

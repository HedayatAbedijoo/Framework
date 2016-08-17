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
  [Description("Framework WebApi Version 2: Provides access to products.")]
  public class ProductV2Controller : ApiController
  {

    [Description("Returns the list of products.")]
    [RestResponse("The products.")]
    [HttpGet]
    public async Task<IHttpActionResult> Get()
    {
      return Ok();
    }

    [Description("Returns a specific product.")]
    [RestParameter("id", "The product identifier.")]
    [RestResponse("The product, if any.")]
    public async Task<IHttpActionResult> Get(string id)
    {
      return Ok();
    }


    [Description("Adds a new product.")]
    [RestParameter("value", "The new product.")]
    [RestResponse("The product.")]
    [ResponseType(typeof(void))]
    public async Task<IHttpActionResult> Post(ProductV2ViewModel value)
    {
      return Ok();
    }


    [Description("Updates a product.")]
    [RestParameter("value", "The updated product.")]
    [ResponseType(typeof(void))]
    public async Task<IHttpActionResult> Put(ProductV2ViewModel value)
    {
      return Ok();
    }

    [Description("Removes a product.")]
    [RestParameter("id", "The product identifier.")]
    public async Task<IHttpActionResult> Delete(string id)
    {
      return Ok();
    }


    [Description("Upload the photo of product.")]
    [ResponseType(typeof(string))]
    public async Task<IHttpActionResult> UploadPhoto()
    {
      return Ok<string>(string.Empty);
    }
  }
}


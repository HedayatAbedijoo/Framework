using Framework.DomainModel;
using Framework.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Framework.UI.Core
{
  /// <summary>
  /// Here should be AutoMapper
  /// </summary>
  public static class Mapping
  {
    /// <summary>
    /// Mapping Product ViewModel based on Product domain model
    /// </summary>
    /// <param name="product">Product Domain Model</param>
    /// <param name="url">Routing helping which is using to find absolute path of photo which is using by client</param>
    /// <returns>Product View Model</returns>
    public static ProductViewModel CreateViewModel(Product product, System.Web.Http.Routing.UrlHelper url)
    {
      if (product == null) return null;
      var item = new ProductViewModel();
      item.Id = product.Id;
      item.Name = product.Name;
      item.Photo = product.Photo;
      item.LastUpdated = product.LastUpdated;
      item.PhotoUrl = string.IsNullOrEmpty(product.Photo) ? string.Empty : url.Content("~/UploadPhotos/" + product.Photo);
      item.Price = product.Price;
      item.RowVersion = Convert.ToBase64String(product.RowVersion, Base64FormattingOptions.None);
      return item;
    }

    /// <summary>
    /// Mapping Products ViewModel based on Products domain model
    /// </summary>
    /// <param name="products">List of Product Domain Model</param>
    /// <param name="url">Routing helping which is using to find absolute path of photo which is using by client</param>
    /// <returns>Product View Model</returns>
    public static List<ProductViewModel> CreateViewModels(IEnumerable<Product> products, System.Web.Http.Routing.UrlHelper url)
    {
      if (products == null || products.Count() == 0) return null;
      var items = new List<ProductViewModel>();
      foreach (var item in products)
      {
        items.Add(CreateViewModel(item, url));
      }
      return items;
    }

    /// <summary>
    /// Mapping a new product domain model based on received view model from client
    /// </summary>
    /// <param name="viewModel"></param>
    /// <returns></returns>
    public static Product CreateProduct(ProductViewModel viewModel)
    {
      if (viewModel == null) return null;
      var product = new Product();
      product.Id = Guid.NewGuid();
      product.Name = viewModel.Name;
      product.Photo = viewModel.Photo;
      product.Price = viewModel.Price;
      return product;
    }

    /// <summary>
    /// Mapping an existing prodcut domain model based on receved view model from client
    /// </summary>
    /// <param name="prodcut"></param>
    /// <param name="viewModel"></param>
    public static void UpdateProduct(Product prodcut, ProductViewModel viewModel)
    {
      prodcut.Price = viewModel.Price;
      prodcut.Name = viewModel.Name;
      prodcut.Photo = viewModel.Photo;
    }
  }
}
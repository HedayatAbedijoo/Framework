using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.ViewModel
{
  public class ProductViewModel
  {
    [IgnoreDocumentation]
    public System.Guid Id { get; set; }

    [Required]
    public string Name { get; set; }
    public string Photo { get; set; }

    [Required]
    public decimal Price { get; set; }
    [IgnoreDocumentation]
    public DateTime LastUpdated { get; set; }
    [IgnoreDocumentation]
    public string PhotoUrl { get; set; }

    public string RowVersion { get; set; }
  }


  /// <summary>
  /// This product will be used for version 2 of WebApi
  /// </summary>
  public class ProductV2ViewModel
  {
    public string NewProperty { get; set; }

    [IgnoreDocumentation]
    public System.Guid Id { get; set; }

    [Required]
    public string Name { get; set; }
    public string Photo { get; set; }

    [Required]
    public decimal Price { get; set; }
    [IgnoreDocumentation]
    public DateTime LastUpdated { get; set; }
    [IgnoreDocumentation]
    public string PhotoUrl { get; set; }
  }


}
